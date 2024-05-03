using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.BusinessModels.Requests.UsersRequests
{
    public class LoginUserRequest
    {
        [Required(ErrorMessage = "Поле Логин обязательно")]
        [DataType(DataType.Text)]
        [Display(Name = "Логин", Prompt = "Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле Пароль обязательно")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        public string Password { get; set; }
    }
}
