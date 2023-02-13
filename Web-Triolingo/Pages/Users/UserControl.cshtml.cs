using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.Interface.User;
using Web_Triolingo.ModelDto;
using Web_Triolingo.Pages.Settings;

namespace Web_Triolingo.Pages.Users
{
    public class UserControlModel : PageModel
    {
        private readonly ILogger<UserControlModel> _logger;
        private readonly IUserControlService _service;
        public List<UserDto> _cacheUsers;

        public UserControlModel (ILogger<UserControlModel> logger, IUserControlService service)
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
                throw;
            }
        }
    }
}
