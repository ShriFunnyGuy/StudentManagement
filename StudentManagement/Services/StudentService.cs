using MongoDB.Bson;
using MongoDB.Driver;
using StudentManagement.Models;
using StudentManagementAPI.Models;

namespace StudentManagement.Services
{
    public class StudentService : IStudentService
    {
        private readonly IMongoCollection<Student> _students;

        public StudentService(IStudentStoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _students = database.GetCollection<Student>(settings.StudentCoursesCollectionName);
        }

        public async Task<Result<List<Student>>> GetAsync()
        {
            try
            {
                var items = await _students.Find(FilterDefinition<Student>.Empty).ToListAsync();
                return Result.Success(items, "Students fetched successfully.");
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Student>>($"Failed to read students: {ex.Message}");
            }
        }

        public async Task<Result<Student>> GetAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Result.Failure<Student>("Id is required.");

            try
            {
                var student = await _students.Find(s => s.Id == id).FirstOrDefaultAsync();
                if (student is null)
                    return Result.Failure<Student>($"Student with Id={id} not found.");

                return Result.Success(student, "Student fetched successfully.");
            }
            catch (Exception ex)
            {
                return Result.Failure<Student>($"Failed to read student: {ex.Message}");
            }
        }

        public async Task<Result<Student>> CreateAsync(Student student)
        {
            if (student is null)
                return Result.Failure<Student>("Student payload is required.");

            try
            {
                // Ensure Id is set to a valid ObjectId string
                if (string.IsNullOrWhiteSpace(student.Id))
                {
                    student.Id = ObjectId.GenerateNewId().ToString();
                }

                await _students.InsertOneAsync(student);
                return Result.Success(student, "Student created successfully.");
            }
            catch (MongoWriteException mwx) when (mwx.WriteError?.Category == ServerErrorCategory.DuplicateKey)
            {
                return Result.Failure<Student>("A student with the same Id already exists.");
            }
            catch (Exception ex)
            {
                return Result.Failure<Student>($"Failed to create student: {ex.Message}");
            }
        }

        // Use ReplaceOne → overwrite entire document (except _id)
        public async Task<Result> UpdateAsync(string id, Student student)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Result.Failure("Id is required.");
            if (student is null)
                return Result.Failure("Student payload is required.");

            try
            {
                student.Id = id; // keep route id as source of truth
                var result = await _students.ReplaceOneAsync(s => s.Id == id, student);

                if (result.MatchedCount == 0)
                    return Result.Failure($"Student with Id={id} not found.");

                return Result.Success("Student updated successfully.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to update student: {ex.Message}");
            }
        }

        public async Task<Result> RemoveAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Result.Failure("Id is required.");

            try
            {
                var result = await _students.DeleteOneAsync(s => s.Id == id);
                if (result.DeletedCount == 0)
                    return Result.Failure($"Student with Id={id} not found.");

                return Result.Success("Student deleted successfully.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to delete student: {ex.Message}");
            }
        }

        // Sample targeted update helpers (kept private)
        private async Task<Result> UpdateOneAsync(string id, Student student)
        {
            try
            {
                var update = Builders<Student>.Update.Set(s => s.Name, student.Name);
                var result = await _students.UpdateOneAsync(s => s.Id == id, update);

                if (result.MatchedCount == 0)
                    return Result.Failure($"Student with Id={id} not found.");

                return Result.Success("Student updated successfully.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to update student: {ex.Message}");
            }
        }

        private async Task<Result> UpdateManyAsync(Student student)
        {
            try
            {
                var update = Builders<Student>.Update.Set(s => s.Name, student.Name);
                var result = await _students.UpdateManyAsync(s => s.Gender == "True", update);
                return Result.Success("Students updated successfully.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to update students: {ex.Message}");
            }
        }
    }
}
