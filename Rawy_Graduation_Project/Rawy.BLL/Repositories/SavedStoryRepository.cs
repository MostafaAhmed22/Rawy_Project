using Rawy.BLL.Interfaces;
using Rawy.DAL.Data;
using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Repositories
{
	public class SavedStoryRepository :GenericRepository<SavedStory>,ISavedStoryRepository
	{
        public SavedStoryRepository(RawyDBContext context) :base(context)
        {
            
        }
    }
}
