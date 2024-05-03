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
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

		public UserService(ConnectionSettings settings)
        {
            _userRepository = new UserRepository(settings.DefaultConnection);
            _roleRepository = new RoleRepository(settings.DefaultConnection);
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
            var userRole = await _roleRepository.GetRoleUser();
            user.RoleId = userRole.RoleId;
            await _userRepository.Add(user);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Логика сервиса обновления пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="updateUserRequest"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Update(UpdateUserRequest updateUserRequest, string login)
        {
            var initiator = await _userRepository.GetByLogin(login);
            var role = await _roleRepository.GetRoleById(initiator.RoleId);
            var entity = await _userRepository.Get(updateUserRequest.UserId);
            
            if (entity != null)
            {
                if (role.RoleName == "Администратор")
                {
					var user = _mapper.Map<User>(updateUserRequest);
                    await _userRepository.Update(updateUserRequest.UserId, user);
                    return IdentityResult.Success;
                }

                if (role.RoleName == "Пользователь" && initiator.UserId == updateUserRequest.UserId)
                {
					var user = _mapper.Map<User>(updateUserRequest);
                    await _userRepository.Update(updateUserRequest.UserId, user);
                    return IdentityResult.Success;
                }
            }

			return IdentityResult.Failed(new IdentityError
            {
                Description = $"Пользователь {updateUserRequest.UserId} не существует"
            });
        }

        /// <summary>
        /// Логика сервиса получения всех пользователей
        /// </summary>
        public async Task<GetUserResponse[]> GetAll()
        {
            var users = await _userRepository.GetUsers();
            for (int i = 0; i < users.Length; i++)
            {
                var role = await _roleRepository.GetRoleById(users[i].RoleId);
                users[i].Role = role;
            }
            var getUsersResponse = _mapper.Map<User[], GetUserResponse[]>(users);
            return getUsersResponse;
        }

        /// <summary>
        /// Логика сервиса получения пользователя по Идентификатору
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<GetUserResponse> Get(int userId)
        {
            var user = await _userRepository.Get(userId);
            if (user != null)
            {
                var role = await _roleRepository.GetRoleById(user.RoleId);
                user.Role = role;
            }
            var getUserResponse = _mapper.Map<GetUserResponse>(user);
            return getUserResponse;
        }

        /// <summary>
        /// Логика сервиса получения пользователя по Логину
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<AuthenticateResponse> GetByLogin(string login)
        {
            var user = await _userRepository.GetByLogin(login);
            if (user != null)
            {
                var role = await _roleRepository.GetRoleById(user.RoleId);
                user.Role = role;
            }
            var authenticateResponse = _mapper.Map<AuthenticateResponse>(user);
            return authenticateResponse;
        }

        /// <summary>
        /// Логика сервиса получения пользователя по Логину
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<GetUserResponse> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEMail(email);
            if (user != null)
            {
                var role = await _roleRepository.GetRoleById(user.RoleId);
                user.Role = role;
            }
            var getUserResponse = _mapper.Map<GetUserResponse>(user);
            return getUserResponse;
        }

        /// <summary>
        /// Логика сервиса удаления пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Delete(int userId, string login)
        {
            var initiator = await _userRepository.GetByLogin(login);
            var role = await _roleRepository.GetRoleById(initiator.RoleId);
            var entity = await _userRepository.Get(userId);

            if (entity != null)
            {
                if (role.RoleName == "Администратор")
                {
                    await _userRepository.Delete(userId);
                    return IdentityResult.Success;
                }

                if (role.RoleName == "Пользователь" && initiator.UserId == userId)
                {
                    await _userRepository.Delete(userId);
                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed(new IdentityError
            {
                Description = $"Пользователь {userId} не существует"
            });
        }
    }
}
