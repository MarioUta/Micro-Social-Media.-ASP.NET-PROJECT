using Mello.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class MessagesController : Controller
    {

        private readonly AppDbContext db;

        private readonly UserManager<User> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private IWebHostEnvironment _env;

        public MessagesController(AppDbContext context,
                              UserManager<User> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IWebHostEnvironment env)

        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
        }
        [HttpPost]
        public IActionResult New(Message message)
        {

            try
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return Redirect($"/Groups/Show/{message.GroupId}");
            }

            catch (Exception)
            {
                return Redirect($"/Groups/Show/{message.GroupId}");
            }

        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return Redirect($"/Groups/Show/{message.GroupId}");
        }

        public IActionResult Edit(int id)
        {
            Message message = db.Messages.Find(id);

            ViewBag.Message = message;

            return View();
        }

        // Se adauga articolul modificat in baza de date
        [HttpPost]
        public IActionResult Edit(int id, Message message)
        {
            Message message1 = db.Messages.Find(id);

            try
            {
                message1.Content = message.Content;
                db.SaveChanges();
                return Redirect($"/Groups/Show/{message1.GroupId}");
            }
            catch (Exception)
            {
                return RedirectToAction("Edit", id);
            }
        }

    }
}
