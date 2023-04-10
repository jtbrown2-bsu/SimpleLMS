using System.Net;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApi.Controllers;
using WebApi.Models;
using WebApi.RequestModels;
using WebApi.Repository;
using WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace WebApi.Tests
{
    public class AssignmentControllerTests : IAsyncLifetime
    {
        IHost? host;

        public Task DisposeAsync()
        {
            var db = host.Services.GetService<LMSDbContext>();
            db.Database.EnsureDeleted();
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            host = await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .ConfigureTestServices(services =>
                    {
                        var assignmentAssembly = typeof(AssignmentController).Assembly;
                        var courseAssembly = typeof(CourseController).Assembly;
                        var moduleAssembly = typeof(ModuleController).Assembly;
                        services.AddControllers().AddApplicationPart(assignmentAssembly).AddApplicationPart(courseAssembly).AddApplicationPart(moduleAssembly);
                        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
                        services.AddScoped<IModuleRepository, ModuleRepository>();
                        services.AddScoped<ICourseRepository, CourseRepository>();
                        var connection = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = ":memory:" }.ToString());
                        services.AddDbContext<LMSDbContext>(options => options.UseSqlite(connection));
                    })
                    .Configure(app =>
                    {
                        using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                        {
                            var dbContext = serviceScope.ServiceProvider.GetService<LMSDbContext>();
                            dbContext.Database.OpenConnection();
                            dbContext.Database.EnsureDeleted();
                            dbContext.Database.Migrate();
                        }
                    }).UseEnvironment("Development");
            })
               .StartAsync();
        }

        [Fact]
        public async Task CreateReadCourse()
        {
            var course = new Course
            {
                Name = "Test",
                Modules = new List<Models.Module>()
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(course), Encoding.UTF8, "application/json");
            var response = await host.GetTestClient().PostAsync("/Course", stringContent);
            Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
            response = await host.GetTestClient().GetAsync("/Course/1");
            var courseResponse = JsonConvert.DeserializeObject<Course>(await response.Content.ReadAsStringAsync());
            Assert.Equal(course.Name, courseResponse.Name);
        }

        [Fact]
        public async Task CreateReadCourseModules()
        {
            var course = new Course
            {
                Name = "Test",
                Modules = new List<Models.Module>
                {
                    new Models.Module
                    {
                        Name = "TestModule",
                        Assignments = new List<Assignment>()
                    },
                    new Models.Module
                    {
                        Name = "TestModule2",
                        Assignments = new List<Assignment>()
                    }
                }
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(course), Encoding.UTF8, "application/json");
            var response = await host.GetTestClient().PostAsync("/Course", stringContent);
            Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
            response = await host.GetTestClient().GetAsync("/Course/1");
            var courseResponse = JsonConvert.DeserializeObject<Course>(await response.Content.ReadAsStringAsync());
            Assert.Equal(2, courseResponse.Modules.Count);
            Assert.Equal("TestModule", courseResponse.Modules[0].Name);
            Assert.Equal("TestModule2", courseResponse.Modules[1].Name);
        }

        [Fact]
        public async Task CreateThreeCourses()
        {
            var courses = new List<Course> {
                new Course
                {
                    Name = "Test",
                    Modules = new List<Models.Module>
                    {
                        new Models.Module
                        {
                            Name = "TestModule",
                            Assignments = new List<Assignment>()
                        },
                        new Models.Module
                        {
                            Name = "TestModule2",
                            Assignments = new List<Assignment>()
                        }
                    }
                },
                new Course
                {
                    Name = "Test2",
                    Modules = new List<Models.Module>()
                },
                new Course
                {
                    Name = "Test3",
                    Modules = new List<Models.Module>
                    {
                        new Models.Module
                        {
                            Name = "TestModule",
                            Assignments = new List<Assignment>()
                        }
                    }
                },
            };
            var stringContent1 = new StringContent(JsonConvert.SerializeObject(courses[0]), Encoding.UTF8, "application/json");
            var response = await host.GetTestClient().PostAsync("/Course", stringContent1);

            var stringContent2 = new StringContent(JsonConvert.SerializeObject(courses[1]), Encoding.UTF8, "application/json");
            var response2 = await host.GetTestClient().PostAsync("/Course", stringContent1);

            var stringContent3 = new StringContent(JsonConvert.SerializeObject(courses[2]), Encoding.UTF8, "application/json");
            var response3 = await host.GetTestClient().PostAsync("/Course", stringContent1);

            response = await host.GetTestClient().GetAsync("/Course");
            var courseResponse = JsonConvert.DeserializeObject<List<Course>>(await response.Content.ReadAsStringAsync());
            Assert.Equal("Test", courseResponse[0].Name);
            Assert.Equal("Test2", courseResponse[1].Name);
            Assert.Equal("Test3", courseResponse[2].Name);
            Assert.Equal(2, courseResponse[0].Modules.Count);
            Assert.Single(courseResponse[2].Modules);
        }
        [Fact]
        public async Task CreateThreeAssignments()
        {
            var assignments = new List<Assignment> {
                new Assignment
                {
                    Name = "Test",
                    Grade = 90,
                    DueDate = new DateTime()
                },
                new Assignment
                {
                    Name = "Test2",
                    Grade = 85,
                    DueDate = new DateTime()
                },
                new Assignment
                {
                    Name = "Test4",
                    Grade = 75,
                    DueDate = new DateTime()
                }
            };
            var stringContent1 = new StringContent(JsonConvert.SerializeObject(assignments[0]), Encoding.UTF8, "application/json");
            var response = await host.GetTestClient().PostAsync("/Assignment", stringContent1);

            var stringContent2 = new StringContent(JsonConvert.SerializeObject(assignments[1]), Encoding.UTF8, "application/json");
            var response2 = await host.GetTestClient().PostAsync("/Assignment", stringContent1);

            var stringContent3 = new StringContent(JsonConvert.SerializeObject(assignments[2]), Encoding.UTF8, "application/json");
            var response3 = await host.GetTestClient().PostAsync("/Assignment", stringContent1);

            response = await host.GetTestClient().DeleteAsync("/Assignment/1");
            response = await host.GetTestClient().GetAsync("/Assignment");
            var assignmentResponse = JsonConvert.DeserializeObject<List<Assignment>>(await response.Content.ReadAsStringAsync());
            Assert.Equal("Test2", assignmentResponse[0].Name);
            Assert.Equal("Test3", assignmentResponse[1].Name);
            Assert.Equal(2, assignmentResponse.Count);
        }

        public async Task UpdateAssignmentThenGet()
        {
            var assignment =
                new Assignment
                {
                    Name = "Test",
                    Grade = 90,
                    DueDate = new DateTime()
                };
            var stringContent1 = new StringContent(JsonConvert.SerializeObject(assignment), Encoding.UTF8, "application/json");
            var response = await host.GetTestClient().PostAsync("/Assignment", stringContent1);
            assignment.Name = "Updated";
            var stringContent2 = new StringContent(JsonConvert.SerializeObject(assignment), Encoding.UTF8, "application/json");
            var response2 = await host.GetTestClient().PutAsync("/Assignment/1", stringContent1);

            response = await host.GetTestClient().GetAsync("/Assignment/1");
            var assignmentResponse = JsonConvert.DeserializeObject<Assignment>(await response.Content.ReadAsStringAsync());
            Assert.Equal("Updated", assignmentResponse.Name);
        }

        public async Task GetWrongIdReturnsNotFound()
        {
            var response = await host.GetTestClient().GetAsync("/Assignment/8");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        public async Task UpdateWrongIdReturnsNotFound()
        {
            var assignment =
                new Assignment
                {
                    Name = "Test",
                    Grade = 90,
                    DueDate = new DateTime()
                };
            var stringContent1 = new StringContent(JsonConvert.SerializeObject(assignment), Encoding.UTF8, "application/json");
            var response = await host.GetTestClient().PostAsync("/Assignment", stringContent1);
            assignment.Name = "Updated"; 
            var stringContent2 = new StringContent(JsonConvert.SerializeObject(assignment), Encoding.UTF8, "application/json");
            response = await host.GetTestClient().PutAsync("/Assignment/7", stringContent2);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}