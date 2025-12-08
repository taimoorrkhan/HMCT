namespace HMCT.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string Title { get; set; }

        public string? Description { get; set; }

        public string Taskstatus { get; set; } = "NotStarted";

        public required DateTime DueDateTime { get; set; }
    }
}
