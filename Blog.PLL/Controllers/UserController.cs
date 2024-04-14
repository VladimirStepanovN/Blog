﻿using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;

namespace Blog.PLL.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
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
        [Authorize(Roles = "Пользователь, Модератор, Администратор")]
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> Update(int userId, [FromBody] UpdateUserRequest updateUserRequest)
        {
            var result = await _userService.Update(userId, updateUserRequest);
            return StatusCode(StatusCodes.Status200OK, updateUserRequest);
            //return View();
        }

        /// <summary>
        /// Получение пользователя по Идентификатору
        /// </summary>
        [Authorize(Roles = "Пользователь, Модератор, Администратор")]
        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> Get(int userId)
        {
            var user = await _userService.Get(userId);
            return StatusCode(StatusCodes.Status200OK, user);
            //return View();
        }

        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        [Authorize(Roles = "Администратор")]
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
        /// /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Пользователь, Администратор")]
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

        [HttpPost]
        [Route("Authenticate")]
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

            var user = await _userService.Get(authenticateResponse.UsertId);
            return StatusCode(StatusCodes.Status200OK, user);
            //return View();
        }

        /*public IActionResult Index()
        {
            return View();
        }*/
    }
}
