using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Interfaces
{
   public interface IFollowRepository: IGenericRepository<WriterFollow>
	{
		Task<List<int>> GetFollowedUserIdsAsync(int currentUserId);


	}
}
