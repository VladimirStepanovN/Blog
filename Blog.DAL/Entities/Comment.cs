using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Entities
{
    /// <summary>
    /// Сущность комментария
    /// </summary>
    [Table("Comments")]
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }
        public virtual User Author { get; set; }
        public virtual Article Article { get; set; }
    }
}
