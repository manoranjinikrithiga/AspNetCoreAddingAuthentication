using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WishList.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WishList.Models;
using System.Threading.Tasks;

namespace WishList.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var loggedInUser =  _userManager.GetUserAsync(HttpContext.User).Result;
            var model = _context.Items.ToList().Where(s => s.User.Id == loggedInUser.Id);

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public  IActionResult Create(Models.Item item)
        {
            var loggedInUser = _userManager.GetUserAsync(HttpContext.User).Result;
            item.User = loggedInUser;
            _context.Items.Add(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var loggedInUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var item = _context.Items.FirstOrDefault(e => e.Id == id);
            if (item.User == loggedInUser)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
            else
            {
                return Unauthorized();
            }
            return RedirectToAction("Index");
        }
    }
}
