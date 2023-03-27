namespace WebApi.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Assignment>? Assignments { get; set; }
    }
}
