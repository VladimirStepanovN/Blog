using AutoMapper;
using Blog.BLL.BusinessModels.Requests.ArticleRequests;
using Blog.BLL.BusinessModels.Requests.CommentRequests;
using Blog.BLL.BusinessModels.Requests.TagRequests;
using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.BLL.BusinessModels.Responses.ArticleResponses;
using Blog.BLL.BusinessModels.Responses.CommentResponses;
using Blog.BLL.BusinessModels.Responses.TagResponses;
using Blog.BLL.BusinessModels.Responses.UserResponses;
using Blog.DAL.Entities;

namespace Blog.PLL
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// В конструкторе настроим соответствие сущностей при маппинге
        /// </summary>
        public MappingProfile()
        {
            CreateMap<AddUserRequest, User>();
            CreateMap<GetUserResponse, UpdateUserRequest>();
            CreateMap<GetUserResponse, DeleteUserRequest>();
            CreateMap<GetTagResponse, UpdateTagRequest>();
            CreateMap<GetTagResponse, DeleteTagRequest>();
            CreateMap<GetTagResponse, TagRequest>();
            CreateMap<GetCommentResponse, UpdateCommentRequest>();
            CreateMap<GetCommentResponse, DeleteCommentRequest>();
            CreateMap<GetArticleResponse, UpdateArticleRequest>();
            CreateMap<GetArticleResponse, DeleteArticleRequest>();
            CreateMap<GetTagResponse, GetTagFullResponse>();
        }
    }
}
