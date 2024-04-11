namespace Blog.BLL.BusinessModels.Requests.ArticleRequests
{
    public class AddArticleRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
    }
}
