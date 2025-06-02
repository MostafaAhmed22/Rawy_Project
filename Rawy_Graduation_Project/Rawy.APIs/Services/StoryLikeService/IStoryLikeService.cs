namespace Rawy.APIs.Services.StoryLikeService
{
	public interface IStoryLikeService
	{
		Task ReactToStoryAsync(int storyId, int userId, bool isLike);
		Task<string> GetReactionStatusAsync(int storyId, int userId);
	}
}
