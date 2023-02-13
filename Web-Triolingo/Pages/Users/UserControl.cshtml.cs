using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.Interface.User;
using Web_Triolingo.Model;
using Web_Triolingo.ModelDto;
using Web_Triolingo.Pages.Settings;
using System.Linq;

namespace Web_Triolingo.Pages.Users
{
	public class UserControlModel : PageModel
	{
		private readonly ILogger<UserControlModel> _logger;
		private readonly IUserControlService _service;
		public List<UserDto> _cacheUsers;

		public UserControlModel(ILogger<UserControlModel> logger, IUserControlService service)
		{
			_logger = logger;
			_service = service;
		}

		public void OnGet()
		{
			try
			{
				_cacheUsers = _service.GetUsers().Result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				throw;
			}
		}

		public async Task<IActionResult> OnGetChgStatusAsync(int id)
		{
			try
			{
				await _service.SwitchStatus(id);
				var user = await _service.GetUser(id);
				if (user == null)
				{
					return BadRequest($"User id: {id} is no longer exist");
				}
				return Content(user.Status.ToString());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return BadRequest(ex.ToString());
			}
		}

		public async Task<IActionResult> OnGetCheckEmail(string email)
		{
			try
			{
				var user = await _service.GetUser(email);
				if (user == null)
				{
					return Content("true");
				}
				return Content("false");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return BadRequest(ex.ToString());
			}
		}


		public async Task<IActionResult> OnPostNewUser(UserDto user)
		{
			try
			{
				var existUser = await _service.GetUser(user.Email);
				if (existUser == null)
				{
					await _service.CreateUser(user);
					_cacheUsers = await _service.GetUsers();
					return Page();
				}
				return new EmptyResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return BadRequest(ex.ToString());
			}
		}

		public async Task<IActionResult> OnPostEditUser(UserDto user)
		{
			try
			{
				var existUser = _service.GetUsers(user.Email);
				if (existUser == null || !existUser.Any(_user => _user.Id != user.Id))
				{
					await _service.UpdateUser(user);
					_cacheUsers = await _service.GetUsers();
					return Page();
				}
				return new EmptyResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return BadRequest(ex.ToString());
			}
		}

		public async Task<IActionResult> OnGetUserInfo(int id)
		{
			try
			{
				var existUser = await _service.GetUser(id);
				if (existUser == null)
				{
					return BadRequest($"User id: {id} not existed in database!");
				}
				return new JsonResult(existUser);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return BadRequest(ex.ToString());
			}
		}
	}
}
