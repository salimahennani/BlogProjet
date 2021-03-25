using MyPersonnalBlog.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace MyPersonnalBlog.api.Controllers
{
    public class BlogPostsController : ApiController
    {
        private UsersRepo _usersRepo;
        private BlogRepo _blogRepo;
        public BlogPostsController()
        {
            _usersRepo = new UsersRepo();
            _blogRepo = new BlogRepo();
        }

        // GET api/<controller>
        public IDictionary<int,string> Get(string username)
        {
            return _usersRepo.GetBlogsByUser(username).Any() ?
                 _usersRepo.GetBlogsByUser(username).ToDictionary(d =>  d.Id, d => d.Title)
                 : new Dictionary<int, string>();
        }

        [System.Web.Mvc.HttpDelete]
        [ValidateAntiForgeryToken]
        public IHttpActionResult Delete(int id)
        {
            var entry = _blogRepo.GetBlogById(id);
            if (_blogRepo.DeleteBlog(entry) == 1)
                return Ok();
            else 
                return NotFound();
        }
    }
}