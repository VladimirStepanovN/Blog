using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.BusinessModels.Requests.TagRequests
{
    public class AddTagRequest
    {
        [Required(ErrorMessage = "Поле Название обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Название")]
        public string Name { get; set; }
    }
}
