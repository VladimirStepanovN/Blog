using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Entities
{
    /// <summary>
    /// Сущность статьи
    /// </summary>
    [Table("Articles")]
    public class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public virtual User Author { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
