using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class TodoViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateDue { get; set; }
        public String RemamingText { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateCompleted { get; set; }
    }
}