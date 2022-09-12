using System.ComponentModel.DataAnnotations;

namespace TodoApp.Application.Dtos
{
    public class UpdateTodoItemDto
    {
        public string Content { get; set; }
        [Required]
        public bool Done { get; set; }
    }
}
