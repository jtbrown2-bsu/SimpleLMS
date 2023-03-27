using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.Controllers;
using WebApi.Models;
using WebApi.RequestModels;

namespace WebApi.Tests
{
    public class AssignmentControllerTests
    {
        AssignmentController _controller = new AssignmentController();
        
        [Fact]
        public void GetRetrievesCorrectCount()
        {
            var result = _controller.Get();
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetRetrievesCorrectName()
        {
            var result = _controller.Get();
            Assert.Equal("Test 1", result[0].Name);
        }

        [Fact]
        public void GetRetrievesCorrectGrade()
        {
            var result = _controller.Get();
            Assert.Equal(100, result[0].Grade);
        }

        [Fact]
        public void GetRetrievesCorrectDueDate()
        {
            var result = _controller.Get();
            Assert.Equal(new DateTime(2023, 03, 25).ToString("dd-MM-yyyy HH:mm:ss.fffff"), result[0].DueDate.ToString("dd-MM-yyyy HH:mm:ss.fffff"));
        }

        [Fact]
        public void GetSingleRetrievesCorrectName()
        {
            var result = _controller.Get(0).Value;
            Assert.Equal("Test 1", result.Name);
        }

        [Fact]
        public void GetSingleRetrievesCorrectGrade()
        {
            var result = _controller.Get(0).Value;
            Assert.Equal(100, result.Grade);
        }

        [Fact]
        public void GetSingleRetrievesCorrectDueDate()
        {
            var result = _controller.Get(0).Value;
            Assert.Equal(new DateTime(2023, 03, 25).ToString("dd-MM-yyyy HH:mm:ss.fffff"), result.DueDate.ToString("dd-MM-yyyy HH:mm:ss.fffff"));
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
            var response = _controller.Update(0, new AssignmentRequest
            {
                Name = "Test 1",
                Grade = 100,
                DueDate = new DateTime(2023, 03, 25),
            });
            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public void UpdateIdUnderZero()
        {
            var response = _controller.Update(-1, new AssignmentRequest
            {
                Name = "Test 1",
                Grade = 100,
                DueDate = new DateTime(2023, 03, 25),
            });
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void UpdateIdHigherThanCount()
        {
            var response = _controller.Update(11, new AssignmentRequest
            {
                Name = "Test 1",
                Grade = 100,
                DueDate = new DateTime(2023, 03, 25),
            });
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void CreateReturnsOnSuccess()
        {
            var response = _controller.Create(new AssignmentRequest
            {
                Name = "Test 1",
                Grade = 100,
                DueDate = new DateTime(2023, 03, 25),
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