using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Signal_R.DAL;
using Signal_R.Hubs;
using Signal_R.Models;
using Signal_R.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Signal_R.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly Context _context;

        public HomeController(Context context, UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, IHubContext<ChatHub> hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hubContext = hubContext;
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Chat()
        {
            ViewBag.Users = _context.Users.ToList();
            return View();
        }

        public async Task<IActionResult> CreateUser()
        {
            var user1 = new AppUser { FullName = "Gunel",UserName="_Gunel" };
            var user2 = new AppUser { FullName = "Dilber", UserName = "_Dilber" };
            var user3 = new AppUser { FullName = "Emil", UserName = "_Emil" };

            await _userManager.CreateAsync(user1, "12345@Gu");
            await _userManager.CreateAsync(user2, "12345@Di");
            await _userManager.CreateAsync(user3, "12345@Em");

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            AppUser user = await _userManager.FindByNameAsync(loginVM.UserName);

           var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, true, true);

            if (user == null) return NotFound();


            return RedirectToAction(nameof(Chat));
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task ShowUserAlert( string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            await _hubContext.Clients.Client(user.ConnectionId).SendAsync("ShowAlert", user.Id);

        }
    }
}
