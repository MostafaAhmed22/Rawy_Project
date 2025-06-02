using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;

namespace Rawy.APIs.Services.StoryLikeService
{
	public class StoryLikeService : IStoryLikeService
	{
		private readonly IUnitOfWork _unitOfWork;

		public StoryLikeService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task ReactToStoryAsync(int storyId, int userId, bool isLike)
		{
			var reaction = await _unitOfWork.StoryLikeRepository.GetUserReactionAsync(storyId, userId);

			if (reaction == null)
			{
				var newLike = new StoryLike
				{
					StoryId = storyId,
					AppUserId = userId,
					IsLike = isLike
				};
				await _unitOfWork.StoryLikeRepository.AddAsync(newLike);
			}
			else
			{
				if (reaction.IsLike == isLike)
				{
					await _unitOfWork.StoryLikeRepository.Delete(reaction); // toggle off
				}
				else
				{
					reaction.IsLike = isLike;
					await _unitOfWork.StoryLikeRepository.UpdateAsync(reaction); // switch reaction
				}
			}

			_unitOfWork.Complete();
		}

		public async Task<string> GetReactionStatusAsync(int storyId, int userId)
		{
			var reaction = await _unitOfWork.StoryLikeRepository.GetUserReactionAsync(storyId, userId);
			return reaction == null ? "none" : (reaction.IsLike ? "like" : "dislike");
		}
	}
}
