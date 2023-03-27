using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.RequestModels;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssignmentController : ControllerBase
    {
        private List<Assignment> _assignments = new List<Assignment>
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
            },
            new Assignment
            {
                Id=2,
                Name="Test 3",
                Grade=92,
                DueDate= new DateTime(2023, 03, 27),
            },
        };

        [HttpGet]
        public List<Assignment> Get()
        {
            return _assignments;
        }

        [HttpGet("{id}")]
        public ActionResult<Assignment> Get(int id)
        {
            if (id >= 0 && id < _assignments.Count())
            {
                return _assignments[id];
            }
            return NotFound();
        }

        //This doesn't actually do anything because we are not expected to have data persistence.
        [HttpPut("{id}")]
        public ActionResult Update(int id, AssignmentRequest request)
        {
            if (id < 0 || id >= _assignments.Count())
            {
                return NotFound();
            }
            var assignment = new Assignment
            {
                Id = id,
                Name = request.Name,
                Grade = request.Grade,
                DueDate = request.DueDate
            };
            _assignments[id] = assignment;
            return Ok();
        }

        //This doesn't actually do anything because we are not expected to have data persistence.
        [HttpPost]
        public ActionResult Create(AssignmentRequest request)
        {
            var assignment = new Assignment
            {
                Id = _assignments.Count(),
                Name = request.Name,
                Grade = request.Grade,
                DueDate = request.DueDate
            };
            _assignments.Add(assignment);
            return Ok();
        }

        //This doesn't actually do anything because we are not expected to have data persistence.
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0 || id >= _assignments.Count())
            {
                return NotFound();
            }
            
            _assignments.RemoveAt(id);
            return Ok();
        }
    }
}