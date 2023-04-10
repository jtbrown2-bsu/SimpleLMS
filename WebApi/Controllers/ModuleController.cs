using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository;
using WebApi.RequestModels;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleRepository _repository;

        public ModuleController(IModuleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<List<Module>> Get()
        {
            return Ok(_repository.Get());
        }

        [HttpGet("{id}")]
        public ActionResult<Module> Get(int id)
        {
            var assignment = _repository.Get(id);
            if (assignment == null)
            {
                return NotFound();
            }
            return Ok(assignment);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, ModuleRequest request)
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
        public ActionResult Create(ModuleRequest request)
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