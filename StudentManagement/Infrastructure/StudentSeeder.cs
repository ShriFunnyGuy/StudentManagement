using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using StudentManagement.Models;
using System.Threading;

namespace StudentManagement.Infrastructure;

public sealed class StudentSeeder
{
    private readonly ILogger<StudentSeeder> _logger;
    private readonly IMongoClient _client;
    private readonly IStudentStoreDatabaseSettings _settings;

    public StudentSeeder(ILogger<StudentSeeder> logger, IMongoClient client, IStudentStoreDatabaseSettings settings)
    {
        _logger = logger;
        _client = client;
        _settings = settings;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        var db = _client.GetDatabase(_settings.DatabaseName);
        var students = db.GetCollection<Student>(_settings.StudentCoursesCollectionName);

        var count = await students.EstimatedDocumentCountAsync(cancellationToken: ct);
        if (count > 0) return;

        var seed = GetSampleStudents();

        await students.InsertManyAsync(seed, cancellationToken: ct);
        _logger.LogInformation("Seeded {Count} students", seed.Count);
    }

    private static List<Student> GetSampleStudents() =>
    [
        new Student { Id = ObjectId.GenerateNewId().ToString(), Name = "Alice Johnson", IsGraduated = false, Course = new[] { "Mathematics", "Physics" }, Gender = "Female", Age = 20 },
        new Student { Id = ObjectId.GenerateNewId().ToString(), Name = "Bob Smith",   IsGraduated = true,  Course = new[] { "Computer Science", "Databases" }, Gender = "Male", Age = 23 },
        new Student { Id = ObjectId.GenerateNewId().ToString(), Name = "Carol Davis", IsGraduated = false, Course = new[] { "Chemistry", "Biology" }, Gender = "Female", Age = 19 },
        new Student { Id = ObjectId.GenerateNewId().ToString(), Name = "Daniel Lee",  IsGraduated = true,  Course = new[] { "Software Engineering", "Distributed Systems" }, Gender = "Male", Age = 24 },
        new Student { Id = ObjectId.GenerateNewId().ToString(), Name = "Eve Torres",  IsGraduated = false, Course = new[] { "Statistics", "Machine Learning" }, Gender = "Female", Age = 21 },
        new Student { Id = ObjectId.GenerateNewId().ToString(), Name = "Frank Wright",IsGraduated = false, Course = new[] { "Economics", "Accounting" }, Gender = "Male", Age = 22 },
        new Student { Id = ObjectId.GenerateNewId().ToString(), Name = "Grace Kim",   IsGraduated = true,  Course = new[] { "Information Security", "Networking" }, Gender = "Female", Age = 25 },
        new Student { Id = ObjectId.GenerateNewId().ToString(), Name = "Henry Clark", IsGraduated = false, Course = new[] { "History", "Philosophy" }, Gender = "Male", Age = 20 }
    ];
}