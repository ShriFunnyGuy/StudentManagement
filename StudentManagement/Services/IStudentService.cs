using StudentManagement.Models;
using StudentManagementAPI.Models;

namespace StudentManagement.Services;

public interface IStudentService
{
    Task<Result<List<Student>>> GetAsync();
    Task<Result<Student>> GetAsync(string id);
    Task<Result<Student>> CreateAsync(Student student);
    Task<Result> UpdateAsync(string id, Student student);
    Task<Result> RemoveAsync(string id);
}