using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<LibraryManagement_WebApi>("LibraryManagementApi");

builder.Build().Run();