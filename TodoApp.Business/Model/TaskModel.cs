using System;
using System.ComponentModel.DataAnnotations;
using TaskStatus = TodoApp.Common.TaskStatus;

namespace TodoApp.Business.Model
{
    public class TaskModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(36)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Content")]
        public string Content { get; set; }

        public DateTime CreatedTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string UpdatedBy { get; set; }

        public TaskStatus Status { get; set; }

        public DateTime Deadline { get; set; }
    }
}

