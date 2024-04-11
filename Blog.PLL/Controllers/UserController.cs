using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Blog.PLL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //для формы регистрации пользователя
        [HttpGet]
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
        [Route("RegisterUser")]
        public async Task<IActionResult> Register([FromBody] AddUserRequest addUserRequest)
        {
            var result = await _userService.Register(addUserRequest);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return StatusCode(StatusCodes.Status200OK, addUserRequest);
            //return View();
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// /// <param name="updateUserRequest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> Update(int userId, [FromBody] UpdateUserRequest updateUserRequest)
        {
            var result = await _userService.Update(userId, updateUserRequest);
            return StatusCode(StatusCodes.Status200OK, updateUserRequest);
            //return View();
        }

        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return StatusCode(StatusCodes.Status200OK, users);
            //return View();
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// /// <param name="deleteUserRequest"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> Delete(int userId)
        {
            var result = await _userService.Delete(userId);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return StatusCode(StatusCodes.Status200OK, $"Пользователь {userId} удалён");
            //return View();
        }

        /*public IActionResult Index()
        {
            return View();
        }*/
    }
}
