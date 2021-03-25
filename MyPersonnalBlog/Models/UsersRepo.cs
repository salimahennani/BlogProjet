using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPersonnalBlog.Models
{
    public class UsersRepo
    {
        private MyPersonnalBlogEntities1 _db;
        public UsersRepo()
        {
            _db = new MyPersonnalBlogEntities1(); 
        }

        public User GetByUserName(string username) => 
            _db.Users.SingleOrDefault(d => d.Username.ToLower() == username);

        public User GetByUserNameAndPassword(string username,string password) =>
           _db.Users.SingleOrDefault(d => d.Username.ToLower() == username && d.Password == password);

        public List<BlogPost> GetBlogsByUser(string username) => 
            GetByUserName(username)?.BlogPosts.ToList();

        public void AddUser(User entry)
        {
            _db.Users.Add(entry);
            _db.SaveChanges();
        }
    }
}