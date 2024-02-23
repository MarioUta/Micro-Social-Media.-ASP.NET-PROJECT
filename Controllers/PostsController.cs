using Mello.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    public class PostsController : Controller
    {
        private readonly AppDbContext db;

        private readonly UserManager<User> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private IWebHostEnvironment _env;

        public PostsController(AppDbContext context,
                               UserManager<User> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IWebHostEnvironment env)

        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = db.Users.Find(userId);
            ViewBag.User = db.Users.Find(userId);


            var following = (from f in db.Follows
                             where f.Follower == user
                             select f.Following).ToList();

            following.Add(user);

            var posts = db.Posts
                        .Include("Comments")
                        .Include("User")
                        .Include("Comments.User")
                        .Where(post => following.Contains(post.User))
                        .OrderByDescending(post => post.Date_created);

            var search = "";
            IQueryable<User> users1 = null;

            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {

                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim();
                List<String> profiles = db.Users.Where(pf => pf.UserName.Contains(search)).Select(a => a.Id).ToList();
                users1 = db.Users.Where(profile => profiles.Contains(profile.Id));

            }
            ViewBag.SearchString = search;
            ViewBag.Profiles = users1;

            ViewBag.Posts = posts;

            if (User.IsInRole("Admin"))
            {
                ViewBag.Role = "Admin";
            }
            else if (User.IsInRole("User"))
            {
                ViewBag.Role = "User";
            }

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.IsLoggedIn = true;
            }

            return View();
        }


        public IActionResult New()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> New(Post post, IFormFile? PostImage)
        {
            try
            {
                post.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                post.Date_created = DateTime.Now;
                if (post.Type == "Image" && PostImage.Length > 0)
                {
                    var storagePath = Path.Combine(_env.WebRootPath, "images", PostImage.FileName);
                    var databaseFileName = "/images/" + PostImage.FileName;
                    using (var fileStream = new FileStream(storagePath, FileMode.Create))
                    {
                        await PostImage.CopyToAsync(fileStream);
                    }
                    post.Link = databaseFileName;

                }
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            catch (Exception)
            {
                return RedirectToAction("New");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);
            var coms = db.Coments.Where(com => com.PostId == id);
            foreach (var com in coms)
            {
                db.Coments.Remove(com);
            }
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);

            ViewBag.Post = post;

            return View();
        }

        // Se adauga articolul modificat in baza de date
        [HttpPost]
        public IActionResult Edit(int id, Post post)
        {
            Post post1 = db.Posts.Find(id);

            try
            {
                post1.Title = post.Title;
                post1.Description = post.Description;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Edit", id);
            }
        }

    }
}
