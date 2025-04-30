
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
	public class AppUser : IdentityUser<int>
	{
		//  public Writer Writer { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string? Bio { get; set; }
		public string? ProfilePictureUrl { get; set; }

		public string? ProfilePicturePublicId { get; set; }
		//	public string? ProfilePicturePublicId { get; set; }
		// Comments
		public ICollection<WriterFollow> Followings { get; set; }

		// الموظفين اللي بيتابعوه
		public ICollection<WriterFollow> Followers { get; set; }

		// Comments
		public ICollection<Comment> Comments { get; set; }

		//Rating 
		public ICollection<Rating> Ratings { get; set; }
		public ICollection<Story> Stories { get; set; }
	}
}
