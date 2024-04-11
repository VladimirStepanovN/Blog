using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.DAL.Entities;
using AutoMapper;
using Blog.BLL.BusinessModels.Responses.UserResponses;
using Blog.BLL.BusinessModels.Requests.ArticleRequests;
using Blog.BLL.BusinessModels.Responses.ArticleResponses;
using Blog.BLL.BusinessModels.Requests.CommentRequests;
using Blog.BLL.BusinessModels.Responses.CommentResponses;

namespace Blog.BLL.BlogConfiguration
{
    public class BusinessMappingProfile : Profile
    {
        /// <summary>
        /// В конструкторе настроим соответствие сущностей при маппинге
        /// </summary>
        public BusinessMappingProfile()
        {
            CreateMap<AddUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, GetUserResponse>();
            CreateMap<AddArticleRequest, Article>();
            CreateMap<UpdateArticleRequest, Article>();
            CreateMap<Article, GetArticleResponse>();
            CreateMap<AddCommentRequest, Comment>();
            CreateMap<UpdateCommentRequest, Comment>();
            CreateMap<Comment, GetCommentResponse>();
        }
    }
}
