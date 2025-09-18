using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudentManagement.Models;

namespace StudentManagement.Infrastructure;

public sealed class StudentDataSeeder : IHostedService
{
    private readonly ILogger<StudentDataSeeder> _logger;
    private readonly IMongoClient _client;
    private readonly IStudentStoreDatabaseSettings _settings;

    public StudentDataSeeder(ILogger<StudentDataSeeder> logger,
                             IMongoClient client,
                             IStudentStoreDatabaseSettings settings)
    {
        _logger = logger;
        _client = client;
        _settings = settings;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var db = _client.GetDatabase(_settings.DatabaseName);
        var students = db.GetCollection<Student>(_settings.StudentCoursesCollectionName);

        var count = await students.EstimatedDocumentCountAsync(cancellationToken: cancellationToken);
        if (count > 0)
        {
            _logger.LogInformation("StudentCourses collection already has {Count} documents. Skipping seed.", count);
            return;
        }

        var seed = GetSampleStudents();
        await students.InsertManyAsync(seed, cancellationToken: cancellationToken);
        _logger.LogInformation("Seeded {Count} sample students into {Collection}.", seed.Count, _settings.StudentCoursesCollectionName);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static List<Student> GetSampleStudents() =>
    [
        new Student
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Alice Johnson",
            IsGraduated = false,
            Course = new[] { "Mathematics", "Physics" },
            Gender = "Female",
            Age = 20
        },
        new Student
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Bob Smith",
            IsGraduated = true,
            Course = new[] { "Computer Science", "Databases" },
            Gender = "Male",
            Age = 23
        },
        new Student
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Carol Davis",
            IsGraduated = false,
            Course = new[] { "Chemistry", "Biology" },
            Gender = "Female",
            Age = 19
        },
        new Student
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Daniel Lee",
            IsGraduated = true,
            Course = new[] { "Software Engineering", "Distributed Systems" },
            Gender = "Male",
            Age = 24
        },
        new Student
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Eve Torres",
            IsGraduated = false,
            Course = new[] { "Statistics", "Machine Learning" },
            Gender = "Female",
            Age = 21
        },
        new Student
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Frank Wright",
            IsGraduated = false,
            Course = new[] { "Economics", "Accounting" },
            Gender = "Male",
            Age = 22
        },
        new Student
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Grace Kim",
            IsGraduated = true,
            Course = new[] { "Information Security", "Networking" },
            Gender = "Female",
            Age = 25
        },
        new Student
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Henry Clark",
            IsGraduated = false,
            Course = new[] { "History", "Philosophy" },
            Gender = "Male",
            Age = 20
        }
    ];
}