using Blog.BLL.BusinessModels.Requests.UsersRequests;

namespace Blog.BLL.BusinessModels.Requests.CommentRequests
{
    public class GetCommentRequest
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }
        public virtual GetUserRequest Author { get; set; }
    }
}
