using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.CommentDto;
using Rawy.APIs.Dtos.ModerationDtos;
using Rawy.APIs.Dtos.StoryDtos;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Data;
using Rawy.DAL.Models;
using Rawy.DAL.Models.Hubs;
using Rawy.DAL.Models.StorySpec;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using static System.Net.WebRequestMethods;
using Rawy.DAL.Models.WriterSpec;

namespace Rawy.APIs.Controllers
{
	
	public class StoryController : BaseApiController
	{
		//private readonly IGenericRepository<Story> _storyRepo;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IHubContext<PostHub> _hubContext;
		private readonly RawyDBContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor ;
		private readonly HttpClient _httpClient;
		private readonly string _moderationApiUrl;
		public StoryController(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<PostHub> hubContext,RawyDBContext context, IHttpContextAccessor httpContextAccessor, HttpClient httpClient, IConfiguration configuration)
		{
			//_storyRepo = StoryRepo;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_hubContext = hubContext;
			_context = context;
			_httpContextAccessor = httpContextAccessor;
			_httpClient = httpClient;
			_moderationApiUrl = "https://walker11-rawipostreview.hf.space";
				//configuration["ModerationService:ApiUrl"] ?? "https://walker11-rawipostreview.hf.space/moderate";
		}
		[HttpGet]

		public async Task<ActionResult<Story>> GetAll([FromQuery] StorySpecParams specParams)
		{ 
			var spec = new StoryWithReview(specParams);
			var Stories = await _unitOfWork.StoryRepository.GetAllWithSpecAsync(spec);

			var responseDtos = Stories.OrderByDescending(s=>s.CreatedAt).Select(story => new StoryResponseDto
			{
				Id = story.Id,
				Title = story.Title,
				Content = story.Content.Length > 200
							? story.Content.Substring(0, 200) + "..."
							: story.Content,
				Category = story.Category,
				CreatedAt = story.CreatedAt,
				WriterId = story.AppUserId,
				WriterName = $"{story.AppUser.FirstName} {story.AppUser.LastName}",
				PhotoUrl = story.AppUser.ProfilePictureUrl,
				PhotoPublicId = story.AppUser.ProfilePicturePublicId,
				AverageRating = _unitOfWork.RatingRepository.GetAverageRatingByStoryIdAsync(story.Id).Result, // Ensure async handling in a real case
				RatingstCount = _unitOfWork.RatingRepository.CountRatingsAsync(story.Id).Result,
			//	LikestCount = _unitOfWork.StoryLikeRepository.CountLikesAsync(story.Id).Result,	
				//DisLikeCount = _unitOfWork.StoryLikeRepository.CountDislikesAsync(story.Id).Result,
				CommentCount = story.Comments?.Count ?? 0

			}).ToList();

			return Ok(responseDtos);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Story>> GetById(int id)
		{
			var spec = new StoryWithReview(id);
			var story = await _unitOfWork.StoryRepository.GetByIdWithSpecAsync(spec);
			if (story == null)
			{
				return NotFound(new ApiResponse(404));
			}

			//var averageScore = await _unitOfWork.RatingRepository.GetAverageRatingByStoryIdAsync(id);

			var responseDto = new StoryByIdDto
			{
				Id = story.Id,
				Title = story.Title,
				Content = story.Content,
				Category = story.Category,
				CreatedAt = story.CreatedAt,
				WriterId = story.AppUserId,
				WriterName = $"{story.AppUser.FirstName} {story.AppUser.LastName}",
				PhotoUrl = story.AppUser.ProfilePictureUrl,
				PhotoPublicId = story.AppUser.ProfilePicturePublicId,
				//AverageRating = averageScore,
				AverageRating = _unitOfWork.RatingRepository.GetAverageRatingByStoryIdAsync(story.Id).Result, // Ensure async handling in a real case
				RatingstCount = _unitOfWork.RatingRepository.CountRatingsAsync(story.Id).Result,
				//LikestCount = _unitOfWork.StoryLikeRepository.CountLikesAsync(story.Id).Result,
				//DisLikeCount = _unitOfWork.StoryLikeRepository.CountDislikesAsync(story.Id).Result,
				Comments = story.Comments?.Select(c => new StoryCommentDto
				{
					Id = c.Id,
					Content = c.Content,
					WriterId = c.AppUserId,
					WriterName = $"{c.AppUser?.FirstName} {c.AppUser?.LastName}",
					PhotoPublicId = c.AppUser.ProfilePicturePublicId,
					PhotoUrl = c.AppUser.ProfilePictureUrl,
					CreatedAt = c.CreatedAt
				}).ToList()
			};

			return Ok(responseDto);
		}



		[HttpGet("followed-stories")]
		public async Task<ActionResult<List<StoryResponseDto>>> GetFollowedStories()
		{
			var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

			if (userIdClaim == null)
				return Unauthorized("User is not authenticated");

			var userId = int.Parse(userIdClaim.Value);

			// Get IDs of writers the user follows
			var followedWriterIds = await _unitOfWork.FollowRepository.GetFollowedUserIdsAsync(userId);

			if (!followedWriterIds.Any())
				return Ok(new List<StoryDto>()); // Empty result

			// Use specification
			var spec = new FollowedStoriesSpec(followedWriterIds);
			var stories = await _unitOfWork.StoryRepository.GetAllWithSpecAsync(spec);

			var responseDtos = stories.OrderByDescending(s => s.CreatedAt).Select(story => new StoryResponseDto
			{
				Id = story.Id,
				Title = story.Title,
				Content = story.Content.Length > 200
						? story.Content.Substring(0, 200) + "..."
						: story.Content,
				Category = story.Category,
				CreatedAt = story.CreatedAt,
				WriterId = story.AppUserId,
				WriterName = $"{story.AppUser.FirstName} {story.AppUser.LastName}",
				PhotoUrl = story.AppUser.ProfilePictureUrl,
				PhotoPublicId = story.AppUser.ProfilePicturePublicId,
				AverageRating = _unitOfWork.RatingRepository.GetAverageRatingByStoryIdAsync(story.Id).Result, // Ensure async handling in a real case
				RatingstCount = _unitOfWork.RatingRepository.CountRatingsAsync(story.Id).Result,
				//	LikestCount = _unitOfWork.StoryLikeRepository.CountLikesAsync(story.Id).Result,	
				//DisLikeCount = _unitOfWork.StoryLikeRepository.CountDislikesAsync(story.Id).Result,
				CommentCount = story.Comments?.Count ?? 0

			}).ToList();
			return Ok(responseDtos);
		}


		[HttpPost]
		public async Task<ActionResult<Story>> AddStory(AddStoryDto _story)
		{
			// Validate Request Body
			if (_story == null)
				return BadRequest("Story data is required.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);



			//  var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
			var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

			if (userIdClaim == null)
				return Unauthorized("User is not authenticated");

			var userId = int.Parse(userIdClaim.Value);
			//Validate WriterId
			if (userId == 0)   
				return BadRequest("WriterId is required.");
			//if (_story.AppUserId == 0)
			//	return BadRequest("WriterId is required.");

			//  Ensure Title and Content Are Not Empty
			if (string.IsNullOrWhiteSpace(_story.Title))
				return BadRequest("Story title cannot be empty.");

			if (string.IsNullOrWhiteSpace(_story.Content))
				return BadRequest("Story content cannot be empty.");


			// ========== CONTENT MODERATION CHECK ==========
			var moderationResult = await ModerateStoryContent(_story.Content);

			if (!moderationResult.IsApproved)
			{
				return BadRequest(new ApiResponse(400, ".إن قصتك تنتهك إرشادات المجتمع والمعايير الثقافية. يرجى مراجعة سياسة المحتوى لدينا وتعديل قصتك قبل إعادة تقديمها"));
			}
			// ============================================

			var story = _mapper.Map<Story>(_story);
			story.AppUserId = userId;
			await _unitOfWork.StoryRepository.AddAsync(story);
			var added = _unitOfWork.Complete();


			// Broadcast the story to all connected clients without calling Get Endpoint => SignalR
			await _hubContext.Clients.All.SendAsync("ReceiveStory", story);


			var responseDto = new AddStoryResponseDto
			{
				Id = story.Id,
				Title = story.Title,
				Content = story.Content,
				Category = story.Category,
				CreatedAt = story.CreatedAt
			};
			return Ok(responseDto);
		}


		[HttpPut("{id}")]
		public async Task<ActionResult<Story>> UpdateStory(int id, UpdateStoryDto storyDto)
		{
			if (storyDto == null)
				return BadRequest("Story data is required.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			//  Ensure the Story Exists

			var story = await _unitOfWork.StoryRepository.GetByIdAsync(id);
			if (story == null)
			{
				return NotFound(new ApiResponse(404));
			}

			// ========== CONTENT MODERATION CHECK FOR UPDATES ==========
			var contentToCheck = !string.IsNullOrWhiteSpace(storyDto.Content) ? storyDto.Content : story.Content;
			var moderationResult = await ModerateStoryContent(contentToCheck);

			if (!moderationResult.IsApproved)
			{
				return BadRequest(new ApiResponse(400, ".إن قصتك تنتهك إرشادات المجتمع والمعايير الثقافية. يرجى مراجعة سياسة المحتوى لدينا وتعديل قصتك قبل إعادة تقديمها"));
			}
			// ========================================================

			_mapper.Map(storyDto, story);
			story.UpdatedAt = DateTime.Now;
			await _unitOfWork.StoryRepository.UpdateAsync(story);
			var Updated = _unitOfWork.Complete();

			// Broadcast the story to all connected clients without calling Get Endpoint => SignalR
			await _hubContext.Clients.All.SendAsync("ReceiveStory", story);

			var responseDto = new UpdateStoryResonse
			{
				Id = story.Id,
				Title = story.Title,
				Content = story.Content,
				Category = story.Category,
				UpdatedAt = story.UpdatedAt

			};
			return Ok(responseDto);

		}

		[HttpDelete]
		public async Task<ActionResult<Story>> DeleteStory(int id)
		{
			var story = await _unitOfWork.StoryRepository.GetByIdAsync(id);
			if (story == null)
			{
				return NotFound(new ApiResponse(404));
			}
			//var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
			//if (userId == null || story.AppUserId != userId)
			//{
			//	//return Forbid(); // User is not allowed to delete this story
			//	return Unauthorized("User is not allowed to delete this story");
			//}

			await _unitOfWork.StoryRepository.DeleteAsync(story.Id);
			var deleted = _unitOfWork.Complete();
			return Ok(story);

		}

		[HttpPost("Save")]
		public async Task<IActionResult> SaveStories( int storyId)
		{
			var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

			var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
			var story = await _unitOfWork.StoryRepository.GetByIdAsync(storyId);

			if (user is null || story is null)
				return BadRequest("Invalid user or story.");

			var alreadySaved = await _context.savedStories
			.AnyAsync(ss => ss.UserId == userId && ss.StoryId == storyId);

			if (alreadySaved)
				return BadRequest("This story is already saved by the user.");

			var savedStory = new SavedStory
			{
				UserId = userId,
				StoryId = storyId,
				SavedAt = DateTime.Now
			};

			await _unitOfWork.SavedStoryRepository.AddAsync(savedStory);
			var added = _unitOfWork.Complete();

			return Ok("Story saved successfully.");
		}

		[HttpDelete("Unsave")]
		public async Task<IActionResult> UnsaveStory( int storyId)
		{
			var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

			var savedStory = await _context.savedStories
				.FirstOrDefaultAsync(ss => ss.UserId == userId && ss.StoryId == storyId);

			if (savedStory == null)
				return NotFound("This story is not saved by the user.");

			_unitOfWork.SavedStoryRepository.Delete(savedStory);
			var deleted = _unitOfWork.Complete();

			return Ok("Story unsaved successfully.");
		}


		[HttpGet("savedStories")]

		public async Task<ActionResult<IEnumerable<SavedStory>>> GetMySavedStories()
		{
			var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

			if (userIdClaim == null)
				return Unauthorized("User is not authenticated");

			var userId = int.Parse(userIdClaim.Value);

			var spec = new SavedStorySpec(userId);
			var savedStories = await _unitOfWork.SavedStoryRepository.GetAllWithSpecAsync(spec);
			var responseDtos = savedStories.Select(story => new StoryResponseDto
			{
				Id = story.StoryId,
				Title = story.Story.Title,
				Content = story.Story.Content.Length > 200
							? story.Story.Content.Substring(0, 200) + "..."
							: story.Story.Content,
				Category = story.Story.Category,
				CreatedAt = story.Story.CreatedAt,
				WriterId = story.Story.AppUserId,
				WriterName = $"{story.Story.AppUser.FirstName} {story.Story.AppUser.LastName}",
				PhotoUrl = story.Story.AppUser.ProfilePictureUrl,
				PhotoPublicId = story.Story.AppUser.ProfilePicturePublicId,
				AverageRating = _unitOfWork.RatingRepository.GetAverageRatingByStoryIdAsync(story.StoryId).Result, // Ensure async handling in a real case
				RatingstCount = _unitOfWork.RatingRepository.CountRatingsAsync(story.StoryId).Result,
				//LikestCount = _unitOfWork.StoryLikeRepository.CountLikesAsync(story.StoryId).Result,
				//DisLikeCount = _unitOfWork.StoryLikeRepository.CountDislikesAsync(story.Id).Result,
				CommentCount = story.Story.Comments?.Count ?? 0

			}).ToList();

			return Ok(responseDtos);
		}


		/// Test endpoint to check moderation service connectivity
		/// </summary>
		[HttpPost("test-moderation")]
		public async Task<ActionResult> TestModeration([FromBody] TestModerationDto testDto)
		{
			if (string.IsNullOrWhiteSpace(testDto?.Content))
			{
				return BadRequest("Content is required for testing");
			}

			var result = await ModerateStoryContent(testDto.Content);

			return Ok(new
			{
				content = testDto.Content,
				moderation_result = result,
				service_url = _moderationApiUrl
			});
		}
		////=====================================================================================================
		//private async Task<ModerationResult> ModerateStoryContent(string content)
		//{
		//	try
		//	{
		//		var request = new { story_content = content };
		//		var json = JsonSerializer.Serialize(request);
		//		var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

		//		using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
		//		var response = await _httpClient.PostAsync($"{_moderationApiUrl}/moderate", httpContent, cts.Token);

		//		if (response.IsSuccessStatusCode)
		//		{
		//			var result = JsonSerializer.Deserialize<ModerationApiResponse>(
		//				await response.Content.ReadAsStringAsync(),
		//				new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });



		//			return new ModerationResult
		//			{
		//				IsApproved = result?.Approved ?? false,
		//				Response = result?.Response ?? "no",
		//				Reason = result?.Reason,
		//				Timestamp = result?.Timestamp ?? DateTime.Now.ToString("O")
		//			};
		//		}

		//		return new ModerationResult
		//		{
		//			IsApproved = false,
		//			Response = "no",
		//			Reason = "Moderation service unavailable",
		//			Timestamp = DateTime.Now.ToString("O")
		//		};
		//	}
		//	catch (Exception ex)
		//	{
		//		return new ModerationResult
		//		{
		//			IsApproved = false,
		//			Response = "no",
		//			Reason = $"Error: {ex.Message}",
		//			Timestamp = DateTime.Now.ToString("O")
		//		};
		//	}
		//}






		////	============================================================================================================
		private async Task<ModerationResult> ModerateStoryContent(string content)
		{
			try
			{
				var moderationRequest = new
				{
					story_content = content
				};

				var json = JsonSerializer.Serialize(moderationRequest);
				var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

				// Set timeout for the HTTP request
				using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

				var response = await _httpClient.PostAsync($"{_moderationApiUrl}/moderate", httpContent, cts.Token);

				if (response.IsSuccessStatusCode)
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					var moderationResponse = JsonSerializer.Deserialize<ModerationApiResponse>(responseContent, new JsonSerializerOptions
					{
						PropertyNamingPolicy = JsonNamingPolicy.CamelCase
					});

					return new ModerationResult
					{
						IsApproved = moderationResponse?.Approved ?? false,
						Response = moderationResponse?.Response ?? "no",
						Reason = moderationResponse?.Reason,
						Timestamp = moderationResponse?.Timestamp ?? DateTime.Now.ToString("O")
					};
				}
				else
				{
					// Log the error but don't block the story posting
					// In production, you might want to have a fallback strategy
					Console.WriteLine($"Moderation service error: {response.StatusCode}");

					// Return false to be safe - reject stories when moderation fails
					return new ModerationResult
					{
						IsApproved = false,
						Response = "no",
						Reason = "Moderation service temporarily unavailable",
						Timestamp = DateTime.Now.ToString("O")
					};
				}
			}
			catch (TaskCanceledException)
			{
				// Timeout occurred
				Console.WriteLine("Moderation service timeout");
				return new ModerationResult
				{
					IsApproved = false,
					Response = "no",
					Reason = "Moderation service timeout",
					Timestamp = DateTime.Now.ToString("O")
				};
			}
			catch (Exception ex)
			{
				// Log the exception
				Console.WriteLine($"Moderation service exception: {ex.Message}");

				// Return false to be safe - reject stories when moderation fails
				return new ModerationResult
				{
					IsApproved = false,
					Response = "no",
					Reason = "Moderation service error",
					Timestamp = DateTime.Now.ToString("O")
				};
			}
		}


		#region StoryService
		//[HttpPost]
		//public async Task<ActionResult<Story>> AddStory(AddStoryDto _story)
		//{
		//	var story = _storyService.AddStory(_story);
		//	return Ok(story);
		//}


		//[HttpPut("{id}")]
		//public async Task<ActionResult<Story>> UpdateStory(int id, UpdateStoryDto storyDto, int userId)
		//{
		//	//var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
		//	var story = _storyService.UpdateStory(id, storyDto, userId);
		//	return Ok(story);

		//}

		//[HttpDelete]
		//public async Task<ActionResult<Story>> DeleteStory(int id)
		//{
		//	var story = _storyService.DeleteStory(id);
		//	return Ok(story);

		//}
		#endregion

	}
}
