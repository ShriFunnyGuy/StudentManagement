using Microsoft.AspNetCore.Mvc;
using StudentManagement.Infrastructure;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ApiControllerBase
    {
        private readonly IStudentService studentService;

        public StudentsController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Student>>>> Get()
        {
            var result = await studentService.GetAsync();
            return From(result);
        }

        // GET: api/students/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Student>>> Get(string id)
        {
            var result = await studentService.GetAsync(id);
            return From(result);
        }

        // POST: api/students
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Student>>> Post([FromBody] Student student)
        {
            var result = await studentService.CreateAsync(student);
            return From(result);
        }

        // PUT: api/students/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object?>>> Put(string id, [FromBody] Student student)
        {
            var result = await studentService.UpdateAsync(id, student);
            return From(result);
        }

        // DELETE: api/students/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object?>>> Delete(string id)
        {
            var result = await studentService.RemoveAsync(id);
            return From(result);
        }
    }
}
