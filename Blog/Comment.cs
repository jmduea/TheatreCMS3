using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TheatreCMS3.Models;

namespace TheatreCMS3.Areas.Blog.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public string Message { get; set; }
        public DateTime CommentDate { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int? ParentCommentId { get; set; }

        [ForeignKey("ParentCommentId")]
        public virtual Comment ParentComment { get; set; }
        public virtual ICollection<Comment> Replies { get; set; }
        public Comment()
        {
            CommentDate = DateTime.Now;
        }

        public double LikeRatio()
        {
            int totalVotes = Likes + Dislikes;
            if (totalVotes == 0)
            {
                return 0;
            }

            return (double)Likes / totalVotes * 100;
        }
    }
}
