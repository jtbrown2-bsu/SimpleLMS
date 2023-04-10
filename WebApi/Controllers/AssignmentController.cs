using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository;
using WebApi.RequestModels;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentRepository _repository;

        public AssignmentController(IAssignmentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<List<Assignment>> Get()
        {
            return Ok(_repository.Get());
        }

        [HttpGet("{id}")]
        public ActionResult<Assignment> Get(int id)
        {
            var assignment = _repository.Get(id);
            if(assignment == null)
            {
                return NotFound();
            }
            return Ok(assignment);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, AssignmentRequest request)
        {
            try
            {
                _repository.Update(id, request);
                return Ok();
            } catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult Create(AssignmentRequest request)
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