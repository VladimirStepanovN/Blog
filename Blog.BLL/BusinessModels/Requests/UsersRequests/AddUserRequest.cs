using Blog.DAL.Entities;

namespace Blog.BLL.BusinessModels.Requests.UsersRequests
{
    public class AddUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } 
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
