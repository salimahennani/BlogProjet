using System.Collections.Generic;
using System.Linq;

namespace MyPersonnalBlog.Models
{
    public class TagsRepo
    {
        private MyPersonnalBlogEntities1 _db;

        public TagsRepo()
        {
            _db = new MyPersonnalBlogEntities1();
        }

        public List<Tag> GetTags() => _db.Tags.ToList();

        public List<BlogPost> GetTagBlogPosts(int id) => _db.Tags.Find(id).BlogTags.Select(d => d.BlogPost).ToList();

        public void AddTags(Tag entry)
        {
            _db.Tags.Add(entry);
            _db.SaveChanges();
        }
    }
}