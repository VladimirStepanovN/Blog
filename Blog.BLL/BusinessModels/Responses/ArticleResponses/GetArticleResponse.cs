using Blog.BLL.BusinessModels.Requests.CommentRequests;
using Blog.BLL.BusinessModels.Requests.TagRequests;
using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.DAL.Entities;

namespace Blog.BLL.BusinessModels.Responses.ArticleResponses
{
    public class GetArticleResponse
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public TagRequest[] Tags { get; set; }
        public virtual GetUserRequest Author { get; set; }
        public virtual ICollection<GetCommentRequest> Comments { get; set; }
    }
}
