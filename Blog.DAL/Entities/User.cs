using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Entities
{
    /// <summary>
    /// Сущность пользователя
    /// </summary>
    [Table("Users")]
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
