using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.Controllers;
using WebApi.Models;
using WebApi.RequestModels;

namespace WebApi.Tests
{
    public class CourseControllerTests
    {
        CourseController _controller = new CourseController();

        [Fact]
        public void GetRetrievesCorrectCount()
        {
            var result = _controller.Get();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetRetrievesCorrectName()
        {
            var result = _controller.Get();
            Assert.Equal("Test 1", result[0].Name);
        }

        [Fact]
        public void GetRetrievesCorrectModules()
        {
            var result = _controller.Get();
            Assert.Equal("Test 2", result[1].Modules[0].Name);
        }

        [Fact]
        public void GetSingleRetrievesCorrectName()
        {
            var result = _controller.Get(0).Value;
            Assert.Equal("Test 1", result.Name);
        }

        [Fact]
        public void GetSingleRetrievesCorrectAssignments()
        {
            var result = _controller.Get(1).Value;
            Assert.Equal("Test 2", result.Modules[0].Name);
        }

        [Fact]
        public void GetSingleIdLowerThanZero()
        {
            var response = _controller.Get(-2);
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public void GetSingleIdHigherThanCount()
        {
            var response = _controller.Get(11);
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public void UpdateReturnsOnSuccess()
        {
            var response = _controller.Update(0, new CourseRequest
            {
                Name = "Test 2",
                Modules = new List<Module>
                {
                    new Module
                    {
                        Id = 1,
                        Name = "Test 2",
                        Assignments = new List<Assignment>()
                    }
                }
            });
            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public void UpdateIdUnderZero()
        {
            var response = _controller.Update(-1, new CourseRequest
            {
                Name = "Test 2",
                Modules = new List<Module>
                {
                    new Module
                    {
                        Id = 1,
                        Name = "Test 2",
                        Assignments = new List<Assignment>()
                    }
                }
            });
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void UpdateIdHigherThanCount()
        {
            var response = _controller.Update(11, new CourseRequest
            {
                Name = "Test 2",
                Modules = new List<Module>
                {
                    new Module
                    {
                        Id = 1,
                        Name = "Test 2",
                        Assignments = new List<Assignment>()
                    }
                }
            });
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void CreateReturnsOnSuccess()
        {
            var response = _controller.Create(new CourseRequest
            {
                Name = "Test 2",
                Modules = new List<Module>
                {
                    new Module
                    {
                        Id = 1,
                        Name = "Test 2",
                        Assignments = new List<Assignment>()
                    }
                }
            });
            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public void DeleteReturnsOnSuccess()
        {
            var response = _controller.Delete(0);
            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public void DeleteIdUnderZero()
        {
            var response = _controller.Delete(-1);
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void DeleteIdHigherThanCount()
        {
            var response = _controller.Delete(11);
            Assert.IsType<NotFoundResult>(response);
        }
    }
}