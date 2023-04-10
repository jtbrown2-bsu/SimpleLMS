
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Grade { get; set; }
        public DateTime DueDate { get; set; }
        [JsonIgnore]
        public Module? Module { get; set; }
    }
}
