using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WishList.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WishList.Models;

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
            ApplicationUser loggedInUser = _userManager.GetUserAsync(HttpContext.User);
            var model = _context.Items.ToList().Where(s => s.User == loggedInUser);

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Models.Item item)
        {
            var loggedInUser = _userManager.GetUserAsync(HttpContext.User);
            item.User = loggedInUser;
            _context.Items.Add(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            ApplicationUser loggedInUser = _userManager.GetUserAsync(HttpContext.User);
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
