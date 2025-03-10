using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
	public class Admin 
	{
		[ForeignKey(nameof(AppUser))]
		public string AdminId { get; set; }
		public string FName { get; set; }
		public string LName { get; set; }
        public AppUser AppUser { get; set; }
    }

}
