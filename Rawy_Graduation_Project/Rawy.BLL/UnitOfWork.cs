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
		private IUserRepository userRepository;
		//private IWriterRepository writerRepository;
		private IRatingRepository ratingRepository;
		private ICommentRepository commentRepository;
		private IFollowRepository followRepository;

		public UnitOfWork(RawyDBContext context)
        {

			_context = context;
			storyRepository = new StoryRepository(_context);
			//writerRepository = new WriterRepository(_context);
			userRepository = new UserRepository(_context);
			ratingRepository = new RatingRepository(_context);
			commentRepository = new CommentRepository(_context);
			followRepository  = new FollowRepository(_context);

		}
        public IStoryRepository StoryRepository => storyRepository;

		//public IWriterRepository WriterRepository => writerRepository;
		

		public IRatingRepository RatingRepository => ratingRepository;
		public ICommentRepository CommentRepository => commentRepository;

		public IFollowRepository FollowRepository => followRepository;

		public IUserRepository UserRepository => userRepository;

		public int Complete()
		{
			return _context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
