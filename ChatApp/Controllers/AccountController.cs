using System.Threading.Tasks;
using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp {
    public class AccountController : Controller {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password){
            var user = await _userManager.FindByNameAsync(username);            //finding rgistered user by username
            if (user != null) {                                                 //remember this type of work is done by usermanager
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);    //here all signin signout work is done by signmanager
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }                        
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var newUser = new User(){
                UserName = username,

            };
            var result = await _userManager.CreateAsync(newUser, password);         //creating user in db
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser,false);
                return RedirectToAction("Index", "Home");
            } 
            return RedirectToAction("Register", "Account");
        }

        public async Task<IActionResult> Logout(){
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}