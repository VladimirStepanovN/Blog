using AutoMapper;
using Blog.BLL.BlogConfiguration;
using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.BLL.BusinessModels.Responses.UserResponses;
using Blog.BLL.Services.IServices;
using Blog.DAL.Entities;
using Blog.DAL.Repositories;
using Blog.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(ConnectionSettings settings)
        {
            _userRepository = new UserRepository(settings.DefaultConnection);
            var mapperConfig = new MapperConfiguration((v) =>
            {
                v.AddProfile(new BusinessMappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
        }


        /// <summary>
        /// Логика сервиса регистрации нового пользователя
        /// </summary>
        /// <param name="addUserRequest"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Register(AddUserRequest addUserRequest)
        {
            var entity = await _userRepository.GetByLogin(addUserRequest.Login);
            if (entity != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Пользователь с таким Login:{addUserRequest.Login} уже существует"
                });
            }

            entity = await _userRepository.GetByEMail(addUserRequest.Email);
            if (entity != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Пользователь с таким Email:{addUserRequest.Email} уже существует"
                });
            }

            var user = _mapper.Map<User>(addUserRequest);
            await _userRepository.Add(user);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Логика сервиса обновления пользователя
        /// </summary>
        /// <param name="updateUserRequest"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Update(UpdateUserRequest updateUserRequest)
        {
            var user = _mapper.Map<User>(updateUserRequest);
            await _userRepository.Update(user);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Логика сервиса получения всех пользователей
        /// </summary>
        public async Task<GetUserResponse[]> GetAll()
        {
            var users = await _userRepository.GetUsers();
            var usersResponse = _mapper.Map<User[], GetUserResponse[]>(users);
            return usersResponse;
        }

        /// <summary>
        /// Логика сервиса удаления пользователя
        /// </summary>
        /// <param name="deleteUserRequest"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Delete(DeleteUserRequest deleteUserRequest)
        {
            var entity = await _userRepository.GetByLogin(deleteUserRequest.Login);
            if (entity == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Пользователь с таким Login:{deleteUserRequest.Login} не существует"
                });
            }

            await _userRepository.Delete(entity);
            return IdentityResult.Success;
        }
    }
}
