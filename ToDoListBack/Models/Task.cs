namespace ToDoListBack.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int CategoryId { get; set; }
        public string? Category { get; set; }
        public Boolean IsCompleted { get; set; }
    }
}
