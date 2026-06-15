using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmsProject.Application.DTOs;
using LmsProject.Domain.Entities;
using LmsProject.Domain.Repositories;

namespace LmsProject.Application.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly ISyllabusRepository _syllabusRepository;

        public InstructorService(
            ICourseRepository courseRepository,
            IInstructorRepository instructorRepository,
            ISyllabusRepository syllabusRepository)
        {
            _courseRepository = courseRepository;
            _instructorRepository = instructorRepository;
            _syllabusRepository = syllabusRepository;
        }

        public async Task<IEnumerable<Instructor>> GetAllInstructorsAsync()
        {
            return await _instructorRepository.GetAllInstructorsAsync();
        }

        // Bridges authentication user identity key with your relational infrastructure table
        public async Task RegisterInstructorAsync(string name, string identityUserId)
        {
            var instructor = new Instructor
            {
                Name = name,
                IdentityUserId = identityUserId
            };
            await _instructorRepository.AddInstructorAsync(instructor);
        }

        public async Task RegisterCourseAsync(string title, string domain, string description, List<int> instructorIds)
        {
            var course = new Course
            {
                Title = title,
                Domain = domain,
                Description = description
            };

            if (instructorIds != null)
            {
                foreach (var id in instructorIds)
                {
                    var instructor = await _instructorRepository.GetInstructorByIdAsync(id);
                    if (instructor != null)
                    {
                        course.Instructors.Add(instructor);
                    }
                }
            }

            await _courseRepository.AddCourseAsync(course);
        }

        public async Task UpdateCourseDetailsAsync(int id, string title, string domain, string description, List<int> instructorIds)
        {
            var existingCourse = await _courseRepository.GetCourseByIdWithDetailsAsync(id);
            if (existingCourse == null) return;

            // 1. Assign values to tracked domain core state properties
            existingCourse.Title = title;
            existingCourse.Domain = domain;
            existingCourse.Description = description;

            // 2. Clear old relationships elements mapping records
            existingCourse.Instructors.Clear();

            // 3. Rebuild updated assigned faculty maps references
            if (instructorIds != null)
            {
                foreach (var instId in instructorIds)
                {
                    var instructor = await _instructorRepository.GetInstructorByIdAsync(instId);
                    if (instructor != null)
                    {
                        existingCourse.Instructors.Add(instructor);
                    }
                }
            }

            await _courseRepository.UpdateCourseAsync(existingCourse);
        }

        public async Task SaveBulkSyllabusAsync(int courseId, List<SyllabusItemDto> materialsDto)
        {
            var course = await _courseRepository.GetCourseByIdWithDetailsAsync(courseId);
            if (course == null) return;

            if (course.CourseMaterials != null && course.CourseMaterials.Any())
            {
                var materialsToDelete = course.CourseMaterials.Select(cm => cm.Material).ToList();
                await _syllabusRepository.RemoveCourseMaterialsRangeAsync(course.CourseMaterials);

                foreach (var oldMat in materialsToDelete)
                {
                    await _syllabusRepository.RemoveMaterialAsync(oldMat);
                }
            }

            if (materialsDto != null)
            {
                foreach (var item in materialsDto)
                {
                    var material = new Material
                    {
                        Title = item.Title,
                        YouTubeLink = item.YouTubeLink
                    };

                    await _syllabusRepository.AddMaterialAsync(material);

                    var courseMaterialLink = new CourseMaterial
                    {
                        CourseId = courseId,
                        MaterialId = material.Id,
                        Position = item.Position
                    };

                    await _syllabusRepository.AddCourseMaterialLinkAsync(courseMaterialLink);
                }
            }
        }

        // FIXED: Replaced raw _context dependencies with clean repository abstraction routing pipelines
        public async Task CreateUserProfileAsync(string identityUserId, string fullName, string emailAddress, string accountType)
        {
            var customProfile = new UserProfile
            {
                IdentityUserId = identityUserId,
                FullName = fullName,
                EmailAddress = emailAddress,
                AccountType = accountType,
                RegisteredOn = DateTime.UtcNow
            };

            // Dispatches execution directly through your abstract infrastructure tier methods
            await _instructorRepository.AddUserProfileAsync(customProfile);
        }
    }
}