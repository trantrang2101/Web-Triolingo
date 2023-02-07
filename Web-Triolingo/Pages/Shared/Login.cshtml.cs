using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.User;
using Web_Triolingo.ModelDto;

namespace Web_Triolingo.Pages.Shared
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserService _userService;
        public LoginModel(ILogger<IndexModel> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        public List<UserLoginDto> UserLogin { get; set; }
        public void OnGet()
        {

        }
        public IActionResult OnPost(UserLoginDto userLogin)
        {
            try
            {
                var user = _userService.Login(userLogin).Result;
                if (user != null)
                {
                    return RedirectToPage("./SettingList");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
