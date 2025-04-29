using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
	public class Writer 
	{

		[ForeignKey(nameof(AppUser))]
		public int WriterId { get; set; }
		public string FName { get; set; }
		public string LName { get; set; }

        // الموظفين اللي بيتابعهم
        public ICollection<WriterFollow> Following { get; set; }

        // الموظفين اللي بيتابعوه
        public ICollection<WriterFollow> Followers { get; set; }
        public string? PreferedLanguage { get; set; }
		public string? WritingStyle { get; set; }
		// Comments
		public ICollection<Comment> Comments { get; set; }

		//Rating 
		public ICollection<Rating> Ratings { get; set; }
		public ICollection<Story> Stories { get; set; }

		public AppUser AppUser { get; set; } // Navigation property
	}
}
