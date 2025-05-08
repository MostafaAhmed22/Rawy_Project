using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
	public class ResetPassword
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Code { get; set; }
		public DateTime CreatedAt { get; set; }	
		public DateTime Expiration { get; set; }
		public bool IsExpired() => CreatedAt.AddMinutes(10) < DateTime.UtcNow;
	}
}
