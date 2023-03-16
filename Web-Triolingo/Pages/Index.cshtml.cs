using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.Users;

namespace Web_Triolingo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserService _userService;


        public IndexModel(ILogger<IndexModel> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public void OnGet()
        {

        }

        public ActionResult OnPostLogin(User userLogin)
        {
            try
            {
                var user = _userService.Login(userLogin).Result;
                if (user != null)
                {
                    //Set session
                    string jsonStr = JsonConvert.SerializeObject(user);
                    HttpContext.Session.SetString("user", jsonStr);
                    return RedirectToAction("Index");
                }
                else
                {
                    HttpContext.Session.SetString("loginError", "Email or Password is incorrect");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public ActionResult OnPostRegis(User userRegis)
        {
            try
            {
                var user = _userService.Regis(userRegis).Result;
                if (user)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    HttpContext.Session.SetString("regisError", "This email is already in use");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}