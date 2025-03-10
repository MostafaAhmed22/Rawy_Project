using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rawy.APIs.Dtos;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;

namespace Rawy.APIs.Controllers
{
	public class WriterController : BaseApiController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;

		public WriterController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
		}

		// Get Writer By ID
		[HttpGet("{id}")]
		public async Task<IActionResult> GetWriterById(string id)
		{
			var writer = await _unitOfWork.WriterRepository.GetByIdAsync(id);
			if (writer == null)
				return NotFound(new ApiResponse(404));

			return Ok(writer);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllWriters()
		{
			var writers = await _unitOfWork.WriterRepository.GetAllAsync();
			return Ok(writers);
		}

		[HttpDelete("{id}")]
		//[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> DeleteWriter(string id)
		{
			var writer = await _unitOfWork.WriterRepository.GetByIdAsync(id);
			if (writer == null)
				return NotFound(new { message = "Writer not found" });

			_unitOfWork.WriterRepository.DeleteAsync(writer.WriterId);
			

			return Ok(new { message = "Writer deleted successfully" });
		}
	}
}
