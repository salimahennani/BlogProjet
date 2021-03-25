using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPersonnalBlog.Models
{
    public class BlogRepo
    {
        private MyPersonnalBlogEntities1 _db;

        public BlogRepo()
        {
            _db = new MyPersonnalBlogEntities1();
        }

        public BlogPost GetBlogById(int id) => _db.BlogPosts.Find(id);

        public int AddBlog(BlogPost entry, IList<string> tags)
        {
            _db.BlogPosts.Add(entry);
            Save();
            return AddTags(entry.Id, tags);
        }

        public int UpdateBlog(BlogPost entry, IList<string> tags)
        {
            Save();
            return AddTags(entry.Id, tags);
        }

        public int DeleteBlog(BlogPost entry)
        {
            RemoveTags(entry.Id);
            _db.BlogPosts.Remove(entry);
            return Save();
        }

        private int AddTags(int blogId,IList<string> tags)
        {
            RemoveTags(blogId);

            foreach (var tag in tags)
            {
                var entry = new BlogTag();
                entry.BlogId = blogId;
                entry.TagId = _db.Tags.Find(Convert.ToInt32(tag)).Id;
                _db.BlogTags.Add(entry);
            }
            return Save();
        }

        private void RemoveTags(int blogId)
        {
            var rowsToRemove = (from tag in _db.BlogTags
                                where tag.BlogId == blogId
                                select tag).ToList();
            _db.BlogTags.RemoveRange(rowsToRemove);
            Save();
        }

        public int Save() => _db.SaveChanges();
    }
}