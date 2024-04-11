using Blog.DAL.Entities;

namespace Blog.BLL.BusinessModels.Requests.ArticleRequests
{
    public class UpdateArticleRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
