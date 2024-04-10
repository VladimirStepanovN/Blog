using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.DAL.Entities;
using AutoMapper;
using Blog.BLL.BusinessModels.Responses.UserResponses;

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
            CreateMap<DeleteUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, GetUserResponse>();
        }
    }
}
