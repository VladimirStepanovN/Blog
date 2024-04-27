using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.BusinessModels.Requests.UsersRequests
{
    public class UpdateUserRequest
    {
        public int UserId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Имя")]
        public string? FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Фамилия", Prompt = "Фамилия")]
        public string? LastName { get; set; }

        [EmailAddress]
        [Display(Name = "Почта")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string? Password { get; set; }

        //[Display(Name = "Роли")]
        //public List<RoleRequest> Roles { get; set; }
    }
}
