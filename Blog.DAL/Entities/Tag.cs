using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Entities
{
    /// <summary>
    /// Сущность тег
    /// </summary>
    [Table("Tags")]
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
