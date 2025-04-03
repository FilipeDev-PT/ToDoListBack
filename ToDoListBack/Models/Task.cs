namespace ToDoListBack.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? CategoryId { get; set; }
        public Categories? Categories { get; set; }
        public Boolean IsCompleted { get; set; }
    }
}
