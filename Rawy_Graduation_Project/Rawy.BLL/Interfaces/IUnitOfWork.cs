using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Interfaces
{
	public interface IUnitOfWork
	{
        public IStoryRepository StoryRepository { get;  }
      //  public IWriterRepository WriterRepository { get; }
		public IUserRepository UseerRepository { get; }
		public IRatingRepository RatingRepository { get; }
        public ICommentRepository CommentRepository { get;  }
		public IFollowRepository FollowRepository { get; }
	}
}
