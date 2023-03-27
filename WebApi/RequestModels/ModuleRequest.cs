using WebApi.Models;

namespace WebApi.RequestModels
{
    public class ModuleRequest
    {
        public string Name { get; set; } = string.Empty;
        public List<Assignment>? Assignments { get; set; }
    }
}
