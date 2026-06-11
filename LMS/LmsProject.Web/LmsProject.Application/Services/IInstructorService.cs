using System.Collections.Generic;
using System.Threading.Tasks;
using LmsProject.Application.DTOs;
using LmsProject.Domain.Entities;

namespace LmsProject.Application.Services
{
    public interface IInstructorService
    {
        Task<IEnumerable<Instructor>> GetAllInstructorsAsync();
        Task RegisterInstructorAsync(string name);
        Task RegisterCourseAsync(string title, string domain, string description, List<int> instructorIds);

        // NEW METHOD ADDITION FOR CORRECT TRACK UPDATES
        Task UpdateCourseDetailsAsync(int id, string title, string domain, string description, List<int> instructorIds);

        Task SaveBulkSyllabusAsync(int courseId, List<SyllabusItemDto> materialsDto);
    }
}

//using System.Collections.Generic;
//using System.Threading.Tasks;
//using LmsProject.Application.DTOs;
//using LmsProject.Domain.Entities;

//namespace LmsProject.Application.Services
//{
//    public interface IInstructorService
//    {
//        Task<IEnumerable<Instructor>> GetAllInstructorsAsync();
//        Task RegisterInstructorAsync(string name);
//        Task RegisterCourseAsync(string title, string domain, string description, List<int> instructorIds);
//        Task SaveBulkSyllabusAsync(int courseId, List<SyllabusItemDto> materialsDto);
//    }
//}