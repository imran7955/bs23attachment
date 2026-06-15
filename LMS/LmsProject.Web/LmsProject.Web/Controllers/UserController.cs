using LmsProject.Domain.Entities;
using LmsProject.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmsProject.Application.Services; // Ensure this matches your service layer namespace

namespace LmsProject.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IInstructorService _instructorService; // FIXED: Declared dependency field

        // FIXED: Injected IInstructorService explicitly into the constructor dependencies
        public UserController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IInstructorService instructorService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _instructorService = instructorService;
        }

        // GET: User/SystemUsers
        public async Task<IActionResult> SystemUsers()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var userListWithRoles = new List<UserManagementViewModel>();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userListWithRoles.Add(new UserManagementViewModel
                {
                    UserId = user.Id,
                    Email = user.Email ?? string.Empty,
                    Username = user.UserName ?? string.Empty,
                    Role = roles.FirstOrDefault() ?? "General User"
                });
            }

            return View(userListWithRoles);
        }

        // POST: User/UpdateUserCredentials
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserCredentials(string userId, string email, string newPassword, string targetRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Target user record context not found.");
            }

            // 1. Update basic profile tracking structures
            user.Email = email;
            user.UserName = email;
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                AddErrorsToModelState(updateResult);
                return await RedirectToSystemUsersWithErrors();
            }

            // 2. Update Passwords securely if provided
            if (!string.IsNullOrEmpty(newPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (!passwordResult.Succeeded)
                {
                    AddErrorsToModelState(passwordResult);
                    return await RedirectToSystemUsersWithErrors();
                }
            }

            // 3. Update RBAC Role Assignment Groups
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(targetRole))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, targetRole);
            }

            return RedirectToAction(nameof(SystemUsers));
        }

        // POST: User/CreateNewUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewUser(string name, string email, string password, string targetRole)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(targetRole))
            {
                ModelState.AddModelError(string.Empty, "Email, Password, and Target Security Role are required fields.");
                return await RedirectToSystemUsersWithErrors();
            }

            var identityUser = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
            var identityResult = await _userManager.CreateAsync(identityUser, password);

            if (identityResult.Succeeded)
            {
                // 1. Assign chosen identity authentication security group level role
                await _userManager.AddToRoleAsync(identityUser, targetRole);

                string profileName = !string.IsNullOrEmpty(name) ? name : email.Split('@')[0];

                // 2. Route profile saving through the service layer instead of direct context calls
                await _instructorService.CreateUserProfileAsync(identityUser.Id, profileName, email, targetRole);

                // 3. If instructor role is chosen, map them to the instructor domain architecture table
                if (targetRole == "Instructor")
                {
                    await _instructorService.RegisterInstructorAsync(profileName, identityUser.Id);
                }

                return RedirectToAction(nameof(SystemUsers));
            }

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return await RedirectToSystemUsersWithErrors();
        }

        private void AddErrorsToModelState(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private async Task<IActionResult> RedirectToSystemUsersWithErrors()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var userListWithRoles = new List<UserManagementViewModel>();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userListWithRoles.Add(new UserManagementViewModel
                {
                    UserId = user.Id,
                    Email = user.Email ?? string.Empty,
                    Username = user.UserName ?? string.Empty,
                    Role = roles.FirstOrDefault() ?? "General User"
                });
            }
            return View(nameof(SystemUsers), userListWithRoles);
        }
    }
}