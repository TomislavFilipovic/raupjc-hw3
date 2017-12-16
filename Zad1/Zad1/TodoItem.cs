using System;
using System.Collections.Generic;

namespace Zad1
{
    public class TodoItem
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        /// <summary >
        /// User id that  owns  this  TodoItem
        ///  </summary >
        public Guid UserId { get; set; }

        /// <summary >
        /// List of  labels  associated  with  TodoItem
        ///  </summary >
        public List<TodoItemLabel> Labels { get; set; }
       
        /// <summary >
        /// Date  due. If null , no date  was  set by the  user
        ///  </summary >
        public DateTime? DateDue { get; set; }

        //  Shorter  syntax  for  single  line  getters  in C#6
        // public  bool  IsCompleted => DateCompleted.HasValue;
        public bool IsCompleted
        {
            get { return DateCompleted.HasValue; }
        }

        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }

        public TodoItem(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            Text = text;
            DateCreated = DateTime.UtcNow;
            UserId = userId;
            Labels = new List<TodoItemLabel>();
        }
        public TodoItem()
        {
            //  entity  framework  needs  this  one
            // not  for  use :)
        }
        public bool MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                DateCompleted = DateTime.Now;
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TodoItem))
            {
                return false;
            }
            if (((TodoItem) obj).GetHashCode() != GetHashCode())
            {
                return false;
            }
            return Id.Equals(((TodoItem) obj).Id);
        }
    }
}