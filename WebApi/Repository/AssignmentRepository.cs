using Microsoft.AspNetCore.Mvc;
using WebApi.Context;
using WebApi.Models;
using WebApi.RequestModels;

namespace WebApi.Repository
{
    public interface IAssignmentRepository
    {
        List<Assignment> Get();
        Assignment Get(int id);
        void Add(AssignmentRequest request);
        void Update(int id, AssignmentRequest request);
        void Delete(int id);
    }
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly LMSDbContext _context;

        public AssignmentRepository(LMSDbContext context)
        {
            _context = context;
        }

        public List<Assignment> Get()
        {
            return _context.Assignments.ToList();
        }

        public Assignment? Get(int id)
        {
            return _context.Assignments.Find(id);
        }

        public void Update(int id, AssignmentRequest request)
        {
            var assignmentToEdit = _context.Assignments.Find(id);
            if (assignmentToEdit != null)
            {
                _context.Entry(assignmentToEdit).CurrentValues.SetValues(request);
                _context.SaveChanges();
            } else
            {
                throw new Exception("No assignment found.");
            }
        }

        public void Add(AssignmentRequest request)
        {
            var assignment = new Assignment
            {
                Name = request.Name,
                Grade = request.Grade,
                DueDate = request.DueDate
            };
            _context.Assignments.Add(assignment);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var assignment = _context.Assignments.Find(id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("No assignment found.");
            }
        }
    }
}
