using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Domain.Entities
{
    public class TodoItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Content { get; set; }
        public bool Done { get; set; }
    }
}
