using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.BusinessModels.Requests.CommentRequests
{
    public class AddCommentRequest
    {
		[Required(ErrorMessage = "Поле Комментарий обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "Комментарий", Prompt = "Введите ваш комментарий")]
		public string Content { get; set; }

        public int UserId { get; set; }
        public int ArticleId { get; set; }
    }
}
