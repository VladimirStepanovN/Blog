using Blog.BLL.BusinessModels.Requests.TagRequests;
using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.BusinessModels.Requests.ArticleRequests
{
    public class DeleteArticleRequest
    {
        public int ArticleId { get; set; }
        [Required(ErrorMessage = "Поле Заголовок обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Заголовок", Prompt = "Заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Поле Контент обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Контент", Prompt = "Контент")]
        public string Content { get; set; }

        public TagRequest[] Tags { get; set; }
    }
}
