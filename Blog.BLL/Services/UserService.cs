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
            var user = await _userRepository.GetByLogin(addUserRequest.Login);
            if (user != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Пользователь с таким Login:{addUserRequest.Login} уже существует"
                });
            }

            user = await _userRepository.GetByEMail(addUserRequest.Email);
            if (user != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Пользователь с таким Email:{addUserRequest.Email} уже существует"
                });
            }

            user = _mapper.Map<User>(addUserRequest);
            await _userRepository.Add(user);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Логика сервиса обновления пользователя
        /// </summary>
        /// <param name="updateUserRequest"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Update(int userId, UpdateUserRequest updateUserRequest)
        {
            var user = _mapper.Map<User>(updateUserRequest);
            await _userRepository.Update(userId, user);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Логика сервиса получения всех пользователей
        /// </summary>
        public async Task<GetUserResponse[]> GetAll()
        {
            var users = await _userRepository.GetUsers();
            var getUsersResponse = _mapper.Map<User[], GetUserResponse[]>(users);
            return getUsersResponse;
        }

        /// <summary>
        /// Логика сервиса получения пользователя по Идентификатору
        /// </summary>
        public async Task<GetUserResponse> Get(int userId)
        {
            var user = await _userRepository.Get(userId);
            var getUserResponse = _mapper.Map<GetUserResponse>(user);
            return getUserResponse;
        }

        /// <summary>
        /// Логика сервиса удаления пользователя
        /// </summary>
        /// <param name="deleteUserRequest"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Delete(int userId)
        {
            var entity = await _userRepository.Get(userId);
            if (entity == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Пользователь {userId} не существует"
                });
            }

            await _userRepository.Delete(userId);
            return IdentityResult.Success;
        }
    }
}
