using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.BusinessModels.Requests.UsersRequests
{
	public class DeleteUserRequest
	{
		public int UserId { get; set; }

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
		[Display(Name = "Имя", Prompt = "Имя")]
		public string? FirstName { get; set; }

        [Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
        [DataType(DataType.Text)]
		[Display(Name = "Фамилия", Prompt = "Фамилия")]
		public string? LastName { get; set; }

        [Required(ErrorMessage = "Поле Email обязательно для заполнения")]
        [EmailAddress]
		[Display(Name = "Почта")]
		public string? Email { get; set; }
	}
}
