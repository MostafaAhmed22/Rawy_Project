
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
	public class AppUser : IdentityUser
	{
        public Writer Writer { get; set; }
        public Admin Admin { get; set; }
    }
}
