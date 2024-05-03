using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.BusinessModels.Requests.TagRequests
{
    public class DeleteTagRequest
    {
        public int TagId { get; set; }

        [Required(ErrorMessage = "Поле Название обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Название")]
        public string Name { get; set; }
    }
}
