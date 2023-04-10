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
using System.Data.Common;

namespace WebApi.Tests
{
    public class LMSTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private HttpClient _client;
        private readonly CustomWebApplicationFactory<Program>
            _factory;

        public LMSTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _factory = factory;
        }

        public async Task DisposeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LMSDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.CloseConnectionAsync();
            var connection = scope.ServiceProvider.GetRequiredService<DbConnection>();
            await connection.CloseAsync();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public async Task InitializeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LMSDbContext>();
            await dbContext.Database.OpenConnectionAsync();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();
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
            var response = await _client.PostAsync("/Course", stringContent);
            Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
            response = await _client.GetAsync("/Course/1");
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
            var response = await _client.PostAsync("/Course", stringContent);
            Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
            response = await _client.GetAsync("/Course/1");
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
                            Name = "TestModule3",
                            Assignments = new List<Assignment>()
                        }
                    }
                },
            };
            var stringContent1 = new StringContent(JsonConvert.SerializeObject(courses[0]), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/Course", stringContent1);

            var stringContent2 = new StringContent(JsonConvert.SerializeObject(courses[1]), Encoding.UTF8, "application/json");
            var response2 = await _client.PostAsync("/Course", stringContent2);

            var stringContent3 = new StringContent(JsonConvert.SerializeObject(courses[2]), Encoding.UTF8, "application/json");
            var response3 = await _client.PostAsync("/Course", stringContent3);

            response = await _client.GetAsync("/Course");
            var courseResponse = JsonConvert.DeserializeObject<List<Course>>(await response.Content.ReadAsStringAsync()).ToList();
            Assert.Equal("Test", courseResponse[0].Name);
            Assert.Equal("Test2", courseResponse[1].Name);
            Assert.Equal("Test3", courseResponse[2].Name);
            Assert.Equal(2, courseResponse[0].Modules.Count);
            Assert.Empty(courseResponse[1].Modules);
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
                    Name = "Test3",
                    Grade = 75,
                    DueDate = new DateTime()
                }
            };
            var stringContent1 = new StringContent(JsonConvert.SerializeObject(assignments[0]), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/Assignment", stringContent1);

            var stringContent2 = new StringContent(JsonConvert.SerializeObject(assignments[1]), Encoding.UTF8, "application/json");
            var response2 = await _client.PostAsync("/Assignment", stringContent2);

            var stringContent3 = new StringContent(JsonConvert.SerializeObject(assignments[2]), Encoding.UTF8, "application/json");
            var response3 = await _client.PostAsync("/Assignment", stringContent3);

            response = await _client.DeleteAsync("/Assignment/1");
            response = await _client.GetAsync("/Assignment");
            var assignmentResponse = JsonConvert.DeserializeObject<IEnumerable<Assignment>>(await response.Content.ReadAsStringAsync()).ToList();
            Assert.Equal("Test2", assignmentResponse[0].Name);
            Assert.Equal("Test3", assignmentResponse[1].Name);
            Assert.Equal(2, assignmentResponse.Count);
        }

        [Fact]
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
            var response = await _client.PostAsync("/Assignment", stringContent1);
            assignment.Name = "Updated";
            var stringContent2 = new StringContent(JsonConvert.SerializeObject(assignment), Encoding.UTF8, "application/json");
            var response2 = await _client.PutAsync("/Assignment/1", stringContent2);

            response = await _client.GetAsync("/Assignment/1");
            var assignmentResponse = JsonConvert.DeserializeObject<Assignment>(await response.Content.ReadAsStringAsync());
            Assert.Equal("Updated", assignmentResponse.Name);
        }

        [Fact]
        public async Task GetWrongIdReturnsNotFound()
        {
            var response = await _client.GetAsync("/Assignment/8");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
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
            var response = await _client.PostAsync("/Assignment", stringContent1);
            assignment.Name = "Updated"; 
            var stringContent2 = new StringContent(JsonConvert.SerializeObject(assignment), Encoding.UTF8, "application/json");
            response = await _client.PutAsync("/Assignment/7", stringContent2);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}