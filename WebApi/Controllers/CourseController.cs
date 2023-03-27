using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.RequestModels;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private List<Course> _courses = new List<Course>
        {
            new Course
            {
                Id = 0,
                Name = "Test 1",
                Modules = new List<Module>
                {
                    new Module
                    {
                        Id = 0,
                        Name = "Test 1",
                        Assignments = new List<Assignment>
                        {
                            new Assignment
                            {
                                Id=0,
                                Name="Test 1",
                                Grade=100,
                                DueDate= new DateTime(2023, 03, 25),
                            },
                            new Assignment
                            {
                                Id=1,
                                Name="Test 2",
                                Grade=95,
                                DueDate= new DateTime(2023, 03, 26),
                            }
                        }
                    }
                }
            },
            new Course
            {
                Id = 1,
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
            }
        };

        [HttpGet]
        public List<Course> Get()
        {
            return _courses;
        }

        [HttpGet("{id}")]
        public ActionResult<Course> Get(int id)
        {
            if (id >= 0 && id < _courses.Count())
            {
                return _courses[id];
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, CourseRequest request)
        {
            if (id < 0 || id >= _courses.Count())
            {
                return NotFound();
            }
            var course = new Course
            {
                Id = id,
                Name = request.Name,
                Modules = request.Modules
            };
            _courses[id] = course;
            return Ok();
        }

        [HttpPost]
        public ActionResult Create(CourseRequest request)
        {
            var module = new Course
            {
                Id = _courses.Count(),
                Name = request.Name,
                Modules = request.Modules
            };
            _courses.Add(module);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0 || id >= _courses.Count())
            {
                return NotFound();
            }

            _courses.RemoveAt(id);
            return Ok();
        }
    }
}