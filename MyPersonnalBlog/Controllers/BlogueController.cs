using MyPersonnalBlog.api.Controllers;
using MyPersonnalBlog.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;

namespace MyPersonnalBlog.Controllers
{
    public class BlogueController : Controller
    {
        private BlogRepo _bloguesRepo = new BlogRepo();
        private UsersRepo _usersRepo = new UsersRepo();
        private TagsRepo _tagsRepo = new TagsRepo();
        public ActionResult Index()
        {
            string nomUsager = Session["LoggedUser"] as string;
            User usager = _usersRepo.GetByUserName(nomUsager);

            if (usager != null)
                return View(usager.BlogPosts.ToList());

            Session["LoggedUser"] = null;
            return RedirectToAction("Index", "Authentification");
        }

        public ActionResult Details(int? id = null)
        {
            var entite = ExtraireEntite(id);
            ViewBag.Tags = _tagsRepo.GetTags();

            return View(entite);
        }

        [HttpPost]
        public ActionResult Details(BlogPost entite)
        {
            ViewBag.Tags = _tagsRepo.GetTags();
            string nomUsager = Session["LoggedUser"] as string;
            User usager = _usersRepo.GetByUserName(nomUsager);
            entite.UserId = usager.UserId;
            if (!ModelState.IsValid)
                return View(entite);

            TryUpdateModel(entite);
            var tags = Request["Tags"] != null ?
                Request["Tags"].Split(new[] { "," }, System.StringSplitOptions.RemoveEmptyEntries) :
                new List<string>().ToArray();
            int resultat;
            if (entite.Id == 0)
            {
                entite.CreateTime = DateTime.Now;
                resultat = _bloguesRepo.AddBlog(entite, tags);
            }
            else
            {
                resultat = _bloguesRepo.UpdateBlog(entite, tags);
            }

            if (resultat != 0) return RedirectToAction("Index");
            ModelState.AddModelError(string.Empty, "oops! veuillez réessayer de nouveau!");
            return View(entite);
        }

        private BlogPost ExtraireEntite(int? id)
        {
            return id == null ?
            new BlogPost()
            : _bloguesRepo.GetBlogById(id.GetValueOrDefault());
        }
    }
}