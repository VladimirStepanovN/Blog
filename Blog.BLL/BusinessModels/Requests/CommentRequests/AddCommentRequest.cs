namespace Blog.BLL.BusinessModels.Requests.CommentRequests
{
    public class AddCommentRequest
    {
        public string Content { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }
    }
}
