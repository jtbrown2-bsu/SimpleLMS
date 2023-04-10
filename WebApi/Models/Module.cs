using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Assignment>? Assignments { get; set; }
        [JsonIgnore]
        public virtual int CourseId { get; set; }
        [JsonIgnore]
        public virtual Course Course { get; set; }
    }
}
