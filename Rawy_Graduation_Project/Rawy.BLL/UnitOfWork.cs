using Rawy.BLL.Interfaces;
using Rawy.BLL.Repositories;
using Rawy.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL
{
	public class UnitOfWork : IUnitOfWork,IDisposable
	{
		private readonly RawyDBContext _context;
		private IStoryRepository storyRepository;
		private IWriterRepository writerRepository;
		private IAdminRepository adminRepository;

        public UnitOfWork(RawyDBContext context)
        {

			_context = context;
			storyRepository = new StoryRepository(_context);
			writerRepository = new WriterRepository(_context);
			adminRepository = new AdminRepository(_context);
			
		}
        public IStoryRepository StoryRepository => storyRepository;

		public IWriterRepository WriterRepository => writerRepository;

		public IAdminRepository AdminRepository => adminRepository;	

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
