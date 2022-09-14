using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TodoApp.Domain.Entities;

namespace TodoApp.Data.Configuration
{
    public class TodoItemConfig : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.HasKey(todoItem => todoItem.Id);
            builder
                .Property(todoItem => todoItem.Content)
                .HasMaxLength(250);
        }
    }
}
