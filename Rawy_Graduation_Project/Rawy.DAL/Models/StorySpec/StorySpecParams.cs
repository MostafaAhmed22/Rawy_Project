using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models.StorySpec
{
	public class StorySpecParams
	{

		public string? search
		{
			get { return search; }
			set { search = value; }
		}

		private const int MaxPagesize = 5;
		public int PageIndex { get; set; } = 1;
        private int pagesize = 5;
		public int Pagesize
		{
			get { return pagesize; }
			set { pagesize = value > MaxPagesize ? MaxPagesize : value; }
		}
		public string? Sort { get; set; }
	}
}
