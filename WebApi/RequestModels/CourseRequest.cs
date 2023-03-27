using WebApi.Models;

namespace WebApi.RequestModels
{
    public class CourseRequest
    {
        public string Name { get; set; } = string.Empty;
        public List<Module> Modules { get; set; } = new List<Module>();
    }
}
