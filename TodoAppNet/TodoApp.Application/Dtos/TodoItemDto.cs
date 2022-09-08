namespace TodoApp.Application.Dtos
{
    public class TodoItemDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public bool Done { get; set; }
    }
}
