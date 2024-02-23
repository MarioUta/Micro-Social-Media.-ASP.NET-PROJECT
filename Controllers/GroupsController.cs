using Mello.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;


namespace WebApplication1.Controllers
{

    public class GroupsController : Controller
    {
        private readonly AppDbContext db;

        private readonly UserManager<User> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private IWebHostEnvironment _env;

        public GroupsController(AppDbContext context,
                              UserManager<User> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IWebHostEnvironment env)

        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New(Group group)
        {
            try
            {
                Member member = new Member();
                Admin admin = new Admin();


                group.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                db.Groups.Add(group);
                db.SaveChanges();

                member.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                admin.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                member.GroupId = group.Id;
                admin.GroupId = group.Id;

                db.Admins.Add(admin);
                db.Members.Add(member);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            catch (Exception)
            {
                return RedirectToAction("New");
            }
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var groups = from m in db.Members
                         where m.UserId == userId
                         select m.Group;
            //var groups = db.Groups.Include("User");
            ViewBag.Groups = groups;
            return View();
        }

        public IActionResult Show(int id)
        {
            var group = db.Groups.Include("Messages").Include("User").Where(group => group.Id == id).FirstOrDefault();
            ViewBag.Group = group;
            var messages = from m in db.Messages
                           where m.GroupId == id
                           select m;
            ViewBag.Messages = messages.Include("User");
            ViewBag.User = db.Users.Find(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return View(group);
        }



        public IActionResult AddMember(int id)
        {
            var search = "";
            IQueryable<User> users1 = null;

            var alreadyInGroup = (from m in db.Members
                                  where m.GroupId == id
                                  select m.User).ToList();

            var user = db.Users.Find(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim();
                List<String> profiles = db.Users.Where(pf => pf.UserName.Contains(search)).Select(a => a.Id).ToList();
                //users1 = db.Users.Where(profile => profiles.Contains(profile.Id));
                var users = from f in db.Follows
                            where (f.Following == user) 
                            && profiles.Contains(f.Follower.Id) 
                            && !alreadyInGroup.Contains(f.Follower)
                            select f.Follower;
                ViewBag.Profiles = users;
            }
            ViewBag.SearchString = search;
            ViewBag.GroupId = id;

            return View();
        }

        [HttpPost]
        public IActionResult AddMember(Member user)
        {
            db.Members.Add(user);
            db.SaveChanges();
            return RedirectToAction("Show", new { id = user.GroupId });
        }

    }
}
