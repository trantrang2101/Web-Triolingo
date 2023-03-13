using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.Interface.Users;
using Web_Triolingo.Pages.Settings;
using System.Linq;
using Triolingo.Core.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_Triolingo.Interface.UserRoles;

namespace Web_Triolingo.Pages.Users
{
	public class UserControlModel : PageModel
	{
		private readonly ILogger<UserControlModel> _logger;
		private readonly IUserControlService _service;
		private readonly IUserRoleService _roleService;
		public List<User> _cacheUsers;
		public List<SelectListItem> _cacheRoles = new List<SelectListItem>();

		public UserControlModel(ILogger<UserControlModel> logger, IUserControlService service, IUserRoleService userRoleService)
		{
			_logger = logger;
			_service = service;
			_roleService = userRoleService;
		}

		public void RenderRoles()
		{
			_cacheRoles = new List<SelectListItem>();
			foreach (var role in _roleService.GetAllRoles())
			{
				_cacheRoles.Add(new SelectListItem
				{
					Text = role.Value,
					Value = role.Key.ToString(),
				});
			}
		}

		public void OnGet()
		{
			try
			{
				_cacheUsers = _service.GetUsers().Result;
				RenderRoles();
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


		public async Task<IActionResult> OnPostNewUser(User user, int roleSettingId, string roleNote)
		{
			try
			{
				var existUser = await _service.GetUser(user.Email);
				if (existUser == null)
				{
					if (await _service.CreateUser(user))
					{
						_roleService.AddRoleForUser(new UserRole
						{
							Note = roleNote,
							UserId = user.Id,
							User = user,
							Setting = _roleService.GetRoleSetting(roleSettingId),
							RoleType = roleSettingId,
						});
					}
					_cacheUsers = await _service.GetUsers();
					RenderRoles();
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

		public async Task<IActionResult> OnPostEditUser(User user, int roleSettingId, string roleNote)
		{
			try
			{
				var existUser = _service.GetUsers(user.Email);
				if (existUser == null || !existUser.Any(_user => _user.Id != user.Id))
				{
					await _service.UpdateUser(user);
					var role = _roleService.GetRoleOfUser(user.Id);
					if (role == null)
					{
						// add new role
						_roleService.AddRoleForUser(new UserRole
						{
							Note = roleNote,
							UserId = user.Id,
							User = user,
							Setting = _roleService.GetRoleSetting(roleSettingId),
							RoleType = roleSettingId,
						});
					}
					else
					{
						role.Note = roleNote;
						role.RoleType = roleSettingId;
						role.Setting = _roleService.GetRoleSetting(roleSettingId);
						_roleService.UpdateRoleOfUser(role);
					}
					_cacheUsers = await _service.GetUsers();
					RenderRoles();
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
		public IActionResult OnGetUserRoleInfo(int id)
		{
			try
			{
				var role = _roleService.GetRoleOfUser(id);
				int roleId;
				string roleNote = null;
				if (role == null)
				{
					// return BadRequest($"User id: {id} Does not have a role!");
					roleId = _roleService.GetAllRoles().FirstOrDefault().Key;
				}
				else
				{
					roleId = role.RoleType;
					roleNote = role.Note;
				}
				return new JsonResult(new { id = roleId, note = roleNote});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return BadRequest(ex.ToString());
			}
		}
	}
}
