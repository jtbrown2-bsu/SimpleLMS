using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository;
using WebApi.RequestModels;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _repository;

        public CourseController(ICourseRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<List<Course>> Get()
        {
            return Ok(_repository.Get());
        }

        [HttpGet("{id}")]
        public ActionResult<Course> Get(int id)
        {
            var assignment = _repository.Get(id);
            if (assignment == null)
            {
                return NotFound();
            }
            return Ok(assignment);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, CourseRequest request)
        {
            try
            {
                _repository.Update(id, request);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult Create(CourseRequest request)
        {
            _repository.Add(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}