namespace TodoApp.Domain.Entities
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public bool Done { get; set; }
    }
}
