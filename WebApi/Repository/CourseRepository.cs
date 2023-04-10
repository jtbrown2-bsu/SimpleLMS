using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.Models;
using WebApi.RequestModels;

namespace WebApi.Repository
{
    public interface ICourseRepository
    {
        List<Course> Get();
        Course Get(int id);
        void Add(CourseRequest request);
        void Update(int id, CourseRequest request);
        void Delete(int id);
    }
    public class CourseRepository : ICourseRepository
    {
        private readonly LMSDbContext _context;

        public CourseRepository(LMSDbContext context)
        {
            _context = context;
        }

        public List<Course> Get()
        {
            return _context.Courses.Include(course => course.Modules).ToList();
        }

        public Course? Get(int id)
        {
            return _context.Courses.Include(course => course.Modules).FirstOrDefault(m => m.Id == id);
        }

        public void Update(int id, CourseRequest request)
        {
            var courseToEdit = _context.Courses.Include(course => course.Modules).FirstOrDefault(m => m.Id == id);
            if (courseToEdit != null)
            {
                _context.Entry(courseToEdit).CurrentValues.SetValues(request);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("No course found.");
            }
        }

        public void Add(CourseRequest request)
        {
            var course = new Course
            {
                Name = request.Name,
                Modules = request.Modules
            };
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var course = _context.Courses.Include(course => course.Modules).FirstOrDefault(m => m.Id == id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("No course found.");
            }
        }
    }
}
