using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models.StorySpec
{
	public class StorySpecParams
	{

		private string _search;
		public string? Search
		{
			get => _search;
			set => _search = value.ToLower();
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
