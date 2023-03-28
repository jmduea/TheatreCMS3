using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheatreCMS3.Areas.Blog.Models;
using TheatreCMS3.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace TheatreCMS3.Areas.Blog.Views
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Blog/Comments
        public ActionResult Index()
        {
            // Get comments with their replies
            var comments = db.Comments.Include(c => c.Replies).Where(c => c.ParentCommentId == null).ToList();

            return View(comments);
        }

        // GET: Blog/Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Blog/Comments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blog/Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentId,Message,CommentDate,Likes,Dislikes,ParentId")] Comment comment, bool isReply = false)
        {
        if (ModelState.IsValid)
        {
            // Set the current user as the author of the comment
            string currentUserId = User.Identity.GetUserId();
            comment.AuthorId = currentUserId; // Set AuthorId

            // Set the current date and time as the comment date
            comment.CommentDate = DateTime.Now;

            db.Comments.Add(comment);
            db.SaveChanges();

            // Retrieve the author's username after saving the comment
            var authorUsername = db.Users.FirstOrDefault(u => u.Id == currentUserId)?.UserName;

            if (isReply)
            {
                // Calculate the time ago for the comment
                var timeAgo = DateTime.Now - comment.CommentDate;

                // Return a JSON object with the necessary data
                return Json(new
                {
                    commentId = comment.CommentId,
                    author = authorUsername,
                    timeAgo = timeAgo.ToString("dd\\:hh\\:mm")
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Render and return the PartialView for the new comment
                return PartialView("_Comment", comment);
            }
        }

        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Blog/Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Blog/Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentId,Message,CommentDate,Likes,Dislikes")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // GET: Blog/Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Blog/Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Comment comment = db.Comments.Find(id);

            if (comment == null)
            {
                return Json(new { success = false, message = "Comment not found" }, JsonRequestBehavior.AllowGet);
            }

            db.Comments.Remove(comment);
            db.SaveChanges();
            return Json(new { success = true, message = "Comment deleted successfully" }, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Action method for liking a comment
        /// </summary>
        [HttpPost]
        public ActionResult Like(int commentId)
        {
            var comment = db.Comments.Find(commentId);
            if (comment != null)
            {
                comment.Likes++;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { Likes = comment.Likes, LikeRatio = comment.LikeRatio() }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Action method for disliking a comment
        /// </summary>
        [HttpPost]
        public ActionResult Dislike(int commentId)
        {
            var comment = db.Comments.Find(commentId);
            if (comment != null)
            {
                comment.Dislikes++;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { Dislikes = comment.Dislikes, LikeRatio = comment.LikeRatio() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadComments()
        {
            // Fetch the comments from the database
            var comments = db.Comments.ToList();

            // Return the comments as a PartialView
            return PartialView("_Comments", comments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReply(string Message, int? ParentCommentId)
        {
            if (ModelState.IsValid)
            {
                // Set the current user as the author of the comment
                string currentUserId = User.Identity.GetUserId();

                // Create a new comment object and set the properties
                Comment comment = new Comment
                {
                    AuthorId = currentUserId,
                    Message = Message,
                    ParentCommentId = ParentCommentId,
                    CommentDate = DateTime.Now
                };

                db.Comments.Add(comment);
                db.SaveChanges();

                // Retrieve the author's username after saving the comment
                var authorUsername = db.Users.FirstOrDefault(u => u.Id == currentUserId)?.UserName;

                // Calculate the time ago for the comment
                var timeAgo = DateTime.Now - comment.CommentDate;

                // Return a JSON object with the necessary data
                return Json(new
                {
                    commentId = comment.CommentId,
                    author = authorUsername,
                    timeAgo = timeAgo.ToString("dd\\:hh\\:mm")
                }, JsonRequestBehavior.AllowGet);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}
