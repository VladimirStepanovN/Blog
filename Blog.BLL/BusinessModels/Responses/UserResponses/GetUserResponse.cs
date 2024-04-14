using Blog.DAL.Entities;

namespace Blog.BLL.BusinessModels.Responses.UserResponses
{
    public class GetUserResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }
}
