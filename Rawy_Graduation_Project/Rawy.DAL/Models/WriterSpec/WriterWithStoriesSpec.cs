using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models.WriterSpec
{
	public class WriterWithStoriesSpec : BaseSpecifications<AppUser> // BaseSpecifications<Writer>
	{
	

		public WriterWithStoriesSpec(int writerId) : base(w => w.Id == writerId) //base(w => w.WriterId == writerId)
		{
			//Includes.Add(w => w.AppUser);
			Includes.Add(w => w.Stories);
			Includes.Add(w => w.Followers);
			Includes.Add(w => w.Followings);
		}
	

	}
}
