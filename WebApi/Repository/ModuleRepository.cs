using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.Models;
using WebApi.RequestModels;

namespace WebApi.Repository
{
    public interface IModuleRepository
    {
        List<Module> Get();
        Module Get(int id);
        void Add(ModuleRequest request);
        void Update(int id, ModuleRequest request);
        void Delete(int id);
    }
    public class ModuleRepository : IModuleRepository
    {
        private readonly LMSDbContext _context;

        public ModuleRepository(LMSDbContext context)
        {
            _context = context;
        }

        public List<Module> Get()
        {
            return _context.Modules.Include(module => module.Assignments).ToList();
        }

        public Module? Get(int id)
        {
            return _context.Modules.Include(module => module.Assignments).FirstOrDefault(m => m.Id == id);
        }

        public void Update(int id, ModuleRequest request)
        {
            var moduleToEdit = _context.Modules.Include(module => module.Assignments).FirstOrDefault(m => m.Id == id);
            if (moduleToEdit != null)
            {
                _context.Entry(moduleToEdit).CurrentValues.SetValues(request);
                _context.SaveChanges();
            } else
            {
                throw new Exception("No module found.");
            }
        }

        public void Add(ModuleRequest request)
        {
            var module = new Module
            {
                Name = request.Name,
                Assignments = request.Assignments
            };
            _context.Modules.Add(module);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var module = _context.Modules.Include(module => module.Assignments).FirstOrDefault(m => m.Id == id);
            if (module != null)
            {
                _context.Modules.Remove(module);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("No module found.");
            }
        }
    }
}
