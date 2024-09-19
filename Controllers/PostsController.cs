using EduCrew.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EduCrew.Controllers
{

    [Authorize]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Post
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "title_asc";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var posts = db.Posts.Include(p => p.Category).Include(p => p.User);

            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(p => p.Title.Contains(searchString) || p.Content.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    posts = posts.OrderByDescending(p => p.Title);
                    break;
                case "title_asc":
                    posts = posts.OrderBy(p => p.Title);
                    break;
                case "Date":
                    posts = posts.OrderBy(p => p.DatePosted);
                    break;
                case "date_desc":
                    posts = posts.OrderByDescending(p => p.DatePosted);
                    break;
                default:
                    posts = posts.OrderBy(p => p.Title);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(posts.ToPagedList(pageNumber, pageSize));
        }

        // GET: Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Include(p => p.Category).Include(p => p.User)
                                .FirstOrDefault(p => p.PostId == id);

            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,Title,Content,DatePosted,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.UserId = User.Identity.GetUserId();
                post.DatePosted = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);
            return View(post);
        }

        // GET: Post/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);
            return View(post);
        }

        // POST: Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,Title,Content,DatePosted,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Attach the post entity to the context
                    var existingPost = db.Posts.Find(post.PostId);

                    if (existingPost == null)
                    {
                        return HttpNotFound();
                    }

                    // Update fields manually (to avoid updating fields that might not be intended)
                    existingPost.Title = post.Title;
                    existingPost.Content = post.Content;
                    existingPost.DatePosted = DateTime.Now;
                    existingPost.CategoryId = post.CategoryId;

                    db.Entry(existingPost).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency exception
                    ModelState.AddModelError("", "The record you attempted to edit was modified by another user after you got the original values. Please try again.");
                }
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);
            return View(post);
        }


        // GET: Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Include(p => p.Category).FirstOrDefault(p => p.PostId == id);
            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}