using System;
using System.Data.Entity;


namespace Zad1
{
    public class TodoDbContext :DbContext
    {
        public TodoDbContext(string cnnstr) : base(cnnstr)
        {
        }

        public IDbSet<TodoItem> TodoItems { get; set; }
        public IDbSet<TodoItemLabel> TodoLabels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>().HasKey(s => s.Id);
            modelBuilder.Entity<TodoItem>().Property(s => s.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(s => s.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(s => s.UserId).IsRequired();
            modelBuilder.Entity<TodoItem>().HasMany(s => s.Labels).WithMany(m => m.LabelTodoItems);
            modelBuilder.Entity<TodoItemLabel>().HasKey(s => s.Id);
            modelBuilder.Entity<TodoItemLabel>().Property(s => s.Value).IsRequired();

        }
    }
}