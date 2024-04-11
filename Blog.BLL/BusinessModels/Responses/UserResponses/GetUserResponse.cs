namespace Blog.BLL.BusinessModels.Responses.UserResponses
{
    public class GetUserResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }
}
