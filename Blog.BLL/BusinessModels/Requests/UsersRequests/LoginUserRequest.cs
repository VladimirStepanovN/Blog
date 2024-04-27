using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.BusinessModels.Requests.UsersRequests
{
    public class LoginUserRequest
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Логин", Prompt = "Введите логин")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        public string Password { get; set; }
    }
}
