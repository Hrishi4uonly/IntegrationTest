using DemoProject.Models;

namespace DemoProject.Services
{
    public interface IStudentService
    {
        Task<bool> AddStudentAsync(Student newStudent);
        Task<bool> DeleteStudentAsync(int Id);
        Task<bool> EditStudentAsync(Student editedStudent);
        Task<Student> GetStudentAsync(int Id);
        Task<IEnumerable<Student>> GetStudentsAsync();
    }
}