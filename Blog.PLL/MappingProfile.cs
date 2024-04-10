using AutoMapper;
using Blog.BLL.BusinessModels.Requests.UsersRequests;
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
        }
    }
}
