using Blog.DAL.Entities;

namespace Blog.BLL.BusinessModels.Responses.UserResponses
{
    public class AuthenticateResponse
    {
        public int UsertId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
