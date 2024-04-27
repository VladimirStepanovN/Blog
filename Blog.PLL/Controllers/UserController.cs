using AutoMapper;
using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;

namespace Blog.PLL.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

		public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        [Route("User/Register")]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="addUserRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/Register")]
        public async Task<IActionResult> Register(AddUserRequest addUserRequest)
        {
            var result = await _userService.Register(addUserRequest);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return View("Authenticate");
        }

        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [Authorize(Roles = "Пользователь, Администратор")]
        [HttpGet]
        [Route("User/Update")]
        public async Task<IActionResult> Update(int userId)
        {
            var user = await _userService.Get(userId);
            var view = _mapper.Map<UpdateUserRequest>(user);
            return View(view);
        }

        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        /// /// <param name="updateUserRequest"></param>
        /// <returns></returns>
        [Authorize(Roles = "Пользователь, Администратор")]
        [HttpPost]
        [Route("User/Update")]
        public async Task<IActionResult> Update(UpdateUserRequest updateUserRequest)
        {
			var result = await _userService.Update(updateUserRequest, User.Identity.Name);
            if (result.Errors.FirstOrDefault() != null)
            {
				return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return StatusCode(StatusCodes.Status200OK, "Обновлен");
			//return View();
		}

        /// <summary>
        /// Получение пользователя по Идентификатору
        /// </summary>
        [Authorize(Roles = "Пользователь, Модератор, Администратор")]
        [HttpGet]
        [Route("User/GetById")]
        public async Task<IActionResult> Get(int userId)
        {
            var user = await _userService.Get(userId);
            return StatusCode(StatusCodes.Status200OK, user);
            //return View();
        }

        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        [Authorize(Roles = "Пользователь, Модератор, Администратор")]
        [HttpGet]
        [Route("User/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return View(users);
        }

		/// <summary>
		/// Удаление пользователя
		/// </summary>
		/// /// <param name="userId"></param>
		/// <returns></returns>
		[Authorize(Roles = "Пользователь, Администратор")]
		[HttpGet]
		[Route("User/Delete")]
		public async Task<IActionResult> Delete(int userId)
		{
			var user = await _userService.Get(userId);
			var view = _mapper.Map<DeleteUserRequest>(user);
			return View(view);
		}

		/// <summary>
		/// Удаление пользователя
		/// </summary>
		/// /// <param name="userId"></param>
		/// <returns></returns>
		[Authorize(Roles = "Пользователь, Администратор")]
        [HttpPost]
        [Route("User/Delete")]
        public async Task<IActionResult> Delete(DeleteUserRequest deleteUserRequest)
        {
			var result = await _userService.Delete(deleteUserRequest.UserId, User.Identity.Name);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            if (User.IsInRole("Администратор"))
            {
				return RedirectToAction("GetAll", "User");
            }
            else
            {
				return RedirectToAction("Authenticate", "User");
			}
        }

        [HttpGet]
        [Route("User/Authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            return View();
        }

        [HttpPost]
        [Route("User/Authenticate")]
        public async Task<IActionResult> Authenticate(string login, string password)
        {

            if (string.IsNullOrEmpty(login) ||
              string.IsNullOrEmpty(password))
                throw new ArgumentNullException("Запрос не корректен");

            var authenticateResponse = await _userService.GetByLogin(login);

            if (authenticateResponse is null)
                throw new AuthenticationException("Пользователь на найден");

            if (authenticateResponse.Password != password)
                throw new AuthenticationException("Введенный пароль не корректен");

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, authenticateResponse.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, authenticateResponse.Role.RoleName)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims,
                "BlogCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            var user = await _userService.Get(authenticateResponse.UserId);

            return RedirectToAction("GetAll", "Article");
        }
	}
}
