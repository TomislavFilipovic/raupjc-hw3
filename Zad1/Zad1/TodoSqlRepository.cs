using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Zad1
{
    public class TodoSqlRepository :ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            TodoItem item = _context.TodoItems.FirstOrDefault(e => e.Id.Equals(todoId));
            if (item==null)
            {
                return null;
            }
            if (!item.UserId.Equals(userId))
            {
                throw new TodoAccessDeniedException("User is not the owner of the item");
            }
            return item;
        }

        public void Add(TodoItem todoItem)
        {
            if (Get(todoItem.Id, todoItem.UserId) != null)
            {
                throw new DuplicateTodoItemException();
            }
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            
            
            TodoItem itemToRemove = Get(todoId, userId);
            _context.TodoItems.Remove(itemToRemove);
            _context.SaveChanges();
            return true;
        }

        public void Update(TodoItem todoItem)
        {
            _context.Entry(todoItem).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            TodoItem toMark=Get(todoId, userId);
            toMark.MarkAsCompleted();
            _context.SaveChanges();
            return true;
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            List<TodoItem> items = _context.TodoItems.Where(e => e.UserId.Equals(userId)).OrderByDescending(e=>e.DateCreated).ToList();
            return items;
            
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            List<TodoItem> items = _context.TodoItems.Where(e => e.UserId.Equals(userId) &&!e.DateCompleted.HasValue ).ToList();
            return items;
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            List<TodoItem> items = _context.TodoItems.Where(e => e.UserId.Equals(userId) && e.DateCompleted.HasValue).ToList();
            return items;
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            List<TodoItem> items = _context.TodoItems.Where(e => e.UserId.Equals(userId) && filterFunction.Invoke(e)).ToList();
            return items;
        }

        public bool ConatinsLabel(string value)
        {
            var label=_context.TodoLabels.FirstOrDefault(h => h.Value.Equals(value));
            return label != null;
        }

        public void AddLabel(string value)
        {
            TodoItemLabel label=new TodoItemLabel(value);
            _context.TodoLabels.Add(label);
            _context.SaveChanges();
        }

        public void AddLabelToTodoItem(String value,Guid itemId,Guid userGuid)
        {
            if (!ConatinsLabel(value))
            {
                AddLabel(value);
            }
            TodoItem item= Get(itemId, userGuid);
           item.Labels.Add(_context.TodoLabels.First(e=>e.Value.Equals(value)));
           _context.SaveChanges();
        }
    }
}