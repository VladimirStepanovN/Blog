using Microsoft.AspNetCore.Mvc;
using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Blog.API.Controllers
{
    /// <summary>
    /// Контроллер пользователей
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///        "firstName": "Пётр",
        ///        "lastName": "Петров",
        ///        "login": "Petr",
        ///        "email": "petr@mail.ru",
        ///        "password": "12345678",
        ///        "passwordApproved": "12345678"
        ///     }
        /// </remarks>
        /// <param name="addUserRequest"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Неправильный запрос</response>
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] AddUserRequest addUserRequest)
        {
            _logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Register action called");
            if (ModelState.IsValid)
            {
                var result = await _userService.Register(addUserRequest);
                if (result.Errors.FirstOrDefault() != null)
                {
                    _logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
                    return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
                }
                return StatusCode(StatusCodes.Status200OK, addUserRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, addUserRequest);
            }
        }

        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///        "userId": 4,
        ///        "firstName": "Пётр",
        ///        "lastName": "Петров",
        ///        "email": "petr@mail.ru",
        ///        "password": "12345678"
        ///     }
        /// </remarks>
        /// <param name="updateUserRequest"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Неправильный запрос</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Пользователь, Администратор")]
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest updateUserRequest)
        {
            _logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Update action called");
            if (ModelState.IsValid)
            {
                var result = await _userService.Update(updateUserRequest, User.Identity.Name);
                if (result.Errors.FirstOrDefault() != null)
                {
                    _logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
                    return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
                }
                return StatusCode(StatusCodes.Status200OK, updateUserRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, updateUserRequest);
            }
        }

        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="401">Неавторизован</response>
        [Authorize(Roles = "Пользователь, Модератор, Администратор")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation($"{User.Identity.Name} :: [HttpGet] GetAll action called");
            var users = await _userService.GetAll();
            return StatusCode(StatusCodes.Status200OK, users);
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     DELETE /Todo
        ///     {
        ///        "userId": 4,
        ///        "firstName": "Пётр",
        ///        "lastName": "Петров",
        ///        "email": "petr@mail.ru"
        ///     }
        /// </remarks>
        /// <param name="deleteUserRequest"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Неправильный запрос</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Пользователь, Администратор")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteUserRequest deleteUserRequest)
        {
            _logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Delete action called");
            if (ModelState.IsValid)
            {
                var result = await _userService.Delete(deleteUserRequest.UserId, User.Identity.Name);
                if (result.Errors.FirstOrDefault() != null)
                {
                    _logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
                    return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
                }
                return StatusCode(StatusCodes.Status200OK, deleteUserRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, deleteUserRequest);
            }
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///        "login": "Ivan",
        ///        "password": "12345678"
        ///     }
        /// </remarks>
        /// <param name="loginUserRequest"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Неправильный запрос</response>
        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginUserRequest loginUserRequest)
        {
            _logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Authenticate action called");
            if (ModelState.IsValid)
            {
                var authenticateResponse = await _userService.GetByLogin(loginUserRequest.Login);

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

                return StatusCode(StatusCodes.Status200OK, loginUserRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, loginUserRequest);
            }
        }

        /// <summary>
        /// Очистка куки от данных авторизации
        /// </summary>
        /// /// <param></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> LogoutAndRemoveCookie()
        {
            _logger.LogInformation($"{User.Identity.Name} :: [HttpGet] LogoutAndRemoveCookie action called");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
