﻿using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.DAL.Entities;
using AutoMapper;
using Blog.BLL.BusinessModels.Responses.UserResponses;
using Blog.BLL.BusinessModels.Requests.ArticleRequests;
using Blog.BLL.BusinessModels.Requests.CommentRequests;
using Blog.BLL.BusinessModels.Responses.CommentResponses;
using Blog.BLL.BusinessModels.Requests.TagRequests;
using Blog.BLL.BusinessModels.Responses.TagResponses;
using Blog.BLL.BusinessModels.Responses.ArticleResponses;

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
            CreateMap<AddTagRequest, Tag>();
            CreateMap<UpdateTagRequest, Tag>();
            CreateMap<Tag, GetTagResponse>();
            CreateMap<User, AuthenticateResponse>();   
            CreateMap<TagRequest, Tag>();
			CreateMap<Tag, TagRequest>();
            CreateMap<Tag, GetTagFullResponse>();
            CreateMap<User, GetUserRequest>();
            CreateMap<Comment, GetCommentRequest>();
		}
    }
}
