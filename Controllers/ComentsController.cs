using Mello.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    public class ComentsController : Controller
    {


        private readonly AppDbContext db;
        public ComentsController(AppDbContext context)
        {
            db = context;
        }

        [HttpPost]
        public IActionResult New(Coment comm)
        {

            try
            {
                comm.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                db.Coments.Add(comm);
                db.SaveChanges();
                return Redirect("/Posts");
            }

            catch (Exception)
            {
                return Redirect("/Posts");
            }

        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Coment comment = db.Coments.Find(id); 
            db.Coments.Remove(comment);
            db.SaveChanges();
            return Redirect("/Posts");
        }

        public IActionResult Edit(int id)
        {
            Coment comm = db.Coments.Find(id);

            ViewBag.comm = comm;

            return View();
        }

        // Se adauga articolul modificat in baza de date
        [HttpPost]
        public IActionResult Edit(int id, Coment com)
        {
            Coment com1 = db.Coments.Find(id);

            try
            {
                com1.Content = com.Content;
                db.SaveChanges();
                return Redirect("/Posts");
            }
            catch (Exception)
            {
                return RedirectToAction("Edit", id);
            }
        }

    }
}
