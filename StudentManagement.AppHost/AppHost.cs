using Aspire.Hosting;
using System.IO;
    
var builder = DistributedApplication.CreateBuilder(args);

// Choose a host folder for Mongo data (adjust the path as you prefer)
//var hostDataPath = OperatingSystem.IsWindows()
//    //? @"C:\mongo-db"
//    : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "mongo-data");

//// Ensure the directory exists
//Directory.CreateDirectory(hostDataPath);

var mongoDB = builder.AddMongoDB("mongodb")
    .WithImageTag("7.0.14-jammy")
    // Bind-mount host directory -> container data dir
    //.WithBindMount(hostDataPath, "/data/db")
    .WithVolume("/data/db")
    .WithEnvironment("MONGO_INITDB_ROOT_USERNAME", "admin")
    .WithEnvironment("MONGO_INITDB_ROOT_PASSWORD", "");

var studentDb = mongoDB.AddDatabase("myFirstDatabase");

builder.AddProject<Projects.StudentManagementAPI>("studentmanagementapi")
    .WithReference(studentDb)
    .WithEnvironment("StudentStoreDatabaseSettings__DatabaseName", "myFirstDatabase")
    .WithEnvironment("StudentStoreDatabaseSettings__StudentCoursesCollectionName", "StudentCourses");

builder.Build().Run();
