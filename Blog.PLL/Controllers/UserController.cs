using AutoMapper;
using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.PLL.Controllers
{
	public class UserController : Controller
	{
		private readonly IUserService _userService;
		private readonly ILogger<UserController> _logger;
		private readonly IMapper _mapper;

		public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
		{
			_userService = userService;
			_mapper = mapper;
			_logger = logger;
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
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Register action called");
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
			_logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Register action called");
			if (ModelState.IsValid)
			{
				var result = await _userService.Register(addUserRequest);
				if (result.Errors.FirstOrDefault() != null)
				{
					_logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
					return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
				}
				return View("Authenticate");
			}
			else
			{
				return View(addUserRequest);
			}
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
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Update action called");
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
			_logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Update action called");
			if (ModelState.IsValid)
			{
				var result = await _userService.Update(updateUserRequest, User.Identity.Name);
				if (result.Errors.FirstOrDefault() != null)
				{
					_logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
					return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
				}
				return RedirectToAction("GetAll", "User");
			}
			else
			{
				return View(updateUserRequest);
			}
		}

		/// <summary>
		/// Получение списка всех пользователей
		/// </summary>
		[Authorize(Roles = "Пользователь, Модератор, Администратор")]
		[HttpGet]
		[Route("User/GetAll")]
		public async Task<IActionResult> GetAll()
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] GetAll action called");
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
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Delete action called");
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
			_logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Delete action called");
			if (ModelState.IsValid)
			{
				var result = await _userService.Delete(deleteUserRequest.UserId, User.Identity.Name);
				if (result.Errors.FirstOrDefault() != null)
				{
					_logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
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
			else
			{
				return View(deleteUserRequest);
			}
		}

		[HttpGet]
		[Route("User/Authenticate")]
		public async Task<IActionResult> Authenticate()
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Authenticate action called");
			return View();
		}

		[HttpPost]
		[Route("User/Authenticate")]
		public async Task<IActionResult> Authenticate(LoginUserRequest loginUserRequest)
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

				return RedirectToAction("GetAll", "Article");
			}
			else
			{
				return View(loginUserRequest);
			}
		}

		[HttpGet]
		[Route("User/Logout")]
		public async Task<IActionResult> LogoutAndRemoveCookie()
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] LogoutAndRemoveCookie action called");
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Authenticate", "User");
		}
	}
}
