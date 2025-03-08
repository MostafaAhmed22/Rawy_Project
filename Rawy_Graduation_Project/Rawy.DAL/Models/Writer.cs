using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
	public class Writer : AppUser
	{
		public string? PreferedLanguage { get; set; }
		public string? WritingStyle { get; set; }
		public ICollection<Story> Stories { get; set; }
	}
}
