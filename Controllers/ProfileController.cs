using Mello.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext db;

        private readonly UserManager<User> _userManager;

        private IWebHostEnvironment _env;

        private readonly RoleManager<IdentityRole> _roleManager;
        public ProfileController(AppDbContext context,
                               UserManager<User> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IWebHostEnvironment env)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
        }
        public IActionResult Show(string id = "-1")
        {
            if(id == "-1")
            {
                id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            var user = db.Users.Find(id);
            ViewBag.User = user;
            var currentUser = db.Users.Find(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            ViewBag.CurrentUserId = currentUser.Id;
            Follow follow = new Follow();
            follow.Follower = currentUser;
            follow.Following = user;
            ViewBag.AlreadyFollows = db.Follows.Contains(follow);
            var follows = from f in db.Follows
                          where (f.Follower == currentUser) && (f.Following == user)
                          select f;
            ViewBag.AlreadyFollows = true;
            if(follows.Count() == 0)
            {
                ViewBag.AlreadyFollows = false;
            }
            var requested = from r in db.Requests
                            where (r.Requester == currentUser) && (r.Target == user)
                            select r;
            ViewBag.AlreadyRequested = true;
            if(requested.Count() == 0)
            {
                ViewBag.AlreadyRequested = false;
            }

            var posts = from p in db.Posts
                        where p.UserId == id
                        orderby p.Date_created descending
                        select p;



            ViewBag.Posts = posts.Include("Comments").Include("Comments.User");

            if (User.IsInRole("Admin"))
            {
                ViewBag.Role = "Admin";
            }

            return View();
        }

        public IActionResult Edit(string id)
        {
            var user = db.Users.Find(id);
            ViewBag.User = user;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, User requestUser, IFormFile? PostImage)
        {
            User user = db.Users.Find(id);
            //try
            //{
                user.UserName = requestUser.UserName;
                //user.NormalizedUserName = (requestUser.UserName).ToUpper();
                user.Email = requestUser.Email;
                user.NormalizedEmail = (requestUser.Email).ToUpper();
                user.PhoneNumber = requestUser.PhoneNumber;
                if (requestUser.Public == true)
                {
                    user.Public = true;
                }
                else
                {
                    user.Public = false;
                }

                if(PostImage == null && user.Link == null)
            {
                user.Link = null;
            }else if(PostImage == null && user.Link != null)
            {
                user.Link = user.Link;
            }

               else if (PostImage.Length > 0)
                {
                    var storagePath = Path.Combine(_env.WebRootPath, "images", PostImage.FileName);
                    var databaseFileName = "/images/" + PostImage.FileName;
                    using (var fileStream = new FileStream(storagePath, FileMode.Create))
                    {
                        await PostImage.CopyToAsync(fileStream);
                    }
                    user.Link = databaseFileName;

                }

                db.SaveChanges();
                return RedirectToAction("Show", new { id = id });
            //}
            //catch
            //{
            //    return RedirectToAction("Edit", new { id = id });
            //}
        }

        public IActionResult Follow(string id)
        {
            var targetUser = db.Users.Find(id);
            var currentUser = db.Users.Find(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (targetUser.Public == true)
            {
                Follow follow = new Follow();
                follow.Follower = currentUser;
                follow.Following = targetUser;
                db.Follows.Add(follow);
            }
            else
            {
                Request request = new Request();
                request.Requester = currentUser;
                request.Target = targetUser;
                db.Requests.Add(request);
            }
            db.SaveChanges();

            return RedirectToAction("Show", new { id = targetUser.Id });
        }

        public IActionResult Unfollow(string id) {
            var targetUser = db.Users.Find(id);
            var currentUser = db.Users.Find(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var follow = from f in db.Follows
                          where (f.Follower == currentUser) && (f.Following == targetUser)
                          select f;

            db.Follows.Remove(follow.ToArray()[0]);
            db.SaveChanges();

            return RedirectToAction("Show", new { id = id });
        }

        public IActionResult ShowRequests(){
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var requests = from r in db.Requests
                           where r.Target.Id == currentUserId
                           select r;

            ViewBag.Requests = requests.Include("Requester");

            return View();
        }

        public IActionResult AcceptRequest(string id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var requests = from r in db.Requests
                           where (r.Target.Id == currentUserId) && (r.Requester.Id == id)
                           select r;
            db.Requests.Remove(requests.ToArray()[0]);

            Follow follow = new Follow();
            follow.Follower = db.Users.Find(id);
            follow.Following = db.Users.Find(currentUserId);

            db.Follows.Add(follow);

            db.SaveChanges();

            return RedirectToAction("ShowRequests");
        }
    }
}
