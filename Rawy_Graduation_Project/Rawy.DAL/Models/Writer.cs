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
		public string WriterId { get; set; }
		public string FName { get; set; }
		public string LName { get; set; }
        public string? PreferedLanguage { get; set; }
		public string? WritingStyle { get; set; }
		public ICollection<Story> Stories { get; set; }

		public AppUser AppUser { get; set; } // Navigation property
	}
}
