using MyPersonnalBlog.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace MyPersonnalBlog.Controllers
{
    public class AuthentificationController : Controller
    {
        private UsersRepo _usersRepo = new UsersRepo();

        public ActionResult Index()
        {
            string nomUsager = Session["LoggedUser"] as string;

            if (!string.IsNullOrEmpty(nomUsager))
                return RedirectToAction("Index", "Blogue");

            return View(new User());
        }

        [HttpPost]
        public ActionResult Index(User entree)
        {
            string motdepassechiffrer = GenererMd5(entree.Password);
            var usager = _usersRepo.GetByUserNameAndPassword(entree.Username, motdepassechiffrer);
            if (usager != null)
            {
                Session["LoggedUser"] = usager.Username;
                return RedirectToAction("Index", "Blogue");
            }
            ModelState.AddModelError(string.Empty, new Exception("user not found"));
            return View(entree);
        }

        private static string GenererMd5(string password)
        {
            string result;
            using (MD5 hash = MD5.Create())
            {
                result = String.Join
                (
                    "",
                    from ba in hash.ComputeHash
                    (
                        Encoding.UTF8.GetBytes(password)
                    )
                    select ba.ToString("x2")
                );
            }

            return result;
        }
    }
}