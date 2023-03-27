namespace WebApi.RequestModels
{
    public class AssignmentRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Grade { get; set; }
        public DateTime DueDate { get; set; }
    }
}
