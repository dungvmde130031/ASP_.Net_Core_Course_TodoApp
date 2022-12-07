using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Database
{
	[Table("TaskTable")]
	public class Task
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Title { get; set; }

        [MaxLength(100)]
        public string Content { get; set; }

		public DateTime DueDate { get; set; }

        public DateTime CreatedTime { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime UpdatedTime { get; set; }

        [MaxLength(100)]
        public string UpdatedBy { get; set; }

    }
}

