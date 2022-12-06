using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Database
{
	[Table("AccountTable")]
	public class Account
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(36)]
		public string FirstName { get; set; }

        [MaxLength(36)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(36)]
        public string LastName { get; set; }

		public DateTime DateOfBirth { get; set; }

		[NotMapped]
		public string TempProperty { get; set; }

		//public Account()
		//{
		//}
	}
}

