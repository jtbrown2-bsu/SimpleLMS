using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.RequestModels;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModuleController : ControllerBase
    {
        private List<Module> _modules = new List<Module>
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
            },
            new Module
            {
                Id = 1,
                Name = "Test 2",
                Assignments = new List<Assignment>()
            }
        };

        [HttpGet]
        public List<Module> Get()
        {
            return _modules;
        }

        [HttpGet("{id}")]
        public ActionResult<Module> Get(int id)
        {
            if (id >= 0 && id < _modules.Count())
            {
                return _modules[id];
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, ModuleRequest request)
        {
            if (id < 0 || id >= _modules.Count())
            {
                return NotFound();
            }
            var module = new Module
            {
                Id = id,
                Name = request.Name,
                Assignments = request.Assignments
            };
            _modules[id] = module;
            return Ok();
        }

        [HttpPost]
        public ActionResult Create(ModuleRequest request)
        {
            var module = new Module
            {
                Id = _modules.Count(),
                Name = request.Name,
                Assignments = request.Assignments
            };
            _modules.Add(module);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0 || id >= _modules.Count())
            {
                return NotFound();
            }

            _modules.RemoveAt(id);
            return Ok();
        }
    }
}