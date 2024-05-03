using AutoMapper;
using Blog.BLL.BusinessModels.Requests.CommentRequests;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PLL.Controllers
{
	public class CommentController : Controller
	{
		private readonly ICommentService _commentService;
		private readonly IUserService _userService;
		private readonly ILogger<CommentController> _logger;
		private readonly IMapper _mapper;

		public CommentController(ICommentService commentService, IUserService userService, IMapper mapper, ILogger<CommentController> logger)
		{
			_commentService = commentService;
			_userService = userService;
			_mapper = mapper;
			_logger = logger;
		}

		[Authorize(Roles = "Пользователь, Модератор")]
		[HttpGet]
		[Route("Comment/Create")]
		public async Task<IActionResult> Create(int articleId)
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Create action called");
			return View(new AddCommentRequest { ArticleId = articleId });
		}


		/// <summary>
		/// Создание нового комментария
		/// </summary>
		/// <param name="addCommentRequest"></param>
		/// <returns></returns>
		[Authorize(Roles = "Пользователь, Модератор")]
		[HttpPost]
		[Route("Comment/Create")]
		public async Task<IActionResult> Create(AddCommentRequest addCommentRequest)
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Create action called");
			if (ModelState.IsValid)
			{
                var user = await _userService.GetByLogin(User.Identity.Name);
                addCommentRequest.UserId = user.UserId;
                var result = await _commentService.Create(addCommentRequest);
                if (result.Errors.FirstOrDefault() != null)
                {
					_logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
					return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
                }
                return RedirectToAction("Get", "Article", new { articleId = addCommentRequest.ArticleId });
            }
			else
			{
				return View(addCommentRequest);
			}
		}

		/// <summary>
		/// Редактирование комментария
		/// </summary>
		/// <param name="commentId"></param>
		/// <returns></returns>
		[Authorize(Roles = "Пользователь, Модератор")]
		[HttpGet]
		[Route("Comment/Update")]
		public async Task<IActionResult> Update(int commentId)
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Update action called");
			var getCommentResponse = await _commentService.Get(commentId);
			var updateCommentRequest = _mapper.Map<UpdateCommentRequest>(getCommentResponse);
			return View(updateCommentRequest);
		}

		/// <summary>
		/// Редактирование комментария
		/// </summary>
		/// <param name="updateCommentRequest"></param>
		/// <returns></returns>
		[Authorize(Roles = "Пользователь, Модератор")]
		[HttpPost]
		[Route("Comment/Update")]
		public async Task<IActionResult> Update(UpdateCommentRequest updateCommentRequest)
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Update action called");
			if (ModelState.IsValid)
			{
                var result = await _commentService.Update(updateCommentRequest, User.Identity.Name);
                if (result.Errors.FirstOrDefault() != null)
                {
					_logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
					return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
                }
                return RedirectToAction("Get", "Article", new { articleId = updateCommentRequest.ArticleId });
            }
			else
			{
				return View(updateCommentRequest);
			}
		}

		/// <summary>
		/// Получение списка всех комментариев
		/// </summary>
		[Authorize(Roles = "Модератор")]
		[HttpGet]
		[Route("Comment/GetAll")]
		public async Task<IActionResult> GetAll()
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] GetAll action called");
			var comments = await _commentService.GetAll();
			return View(comments);
		}

		/// <summary>
		/// Удаление комментария
		/// </summary>
		/// /// <param name="commentId"></param>
		/// <returns></returns>
		[Authorize(Roles = "Пользователь, Модератор")]
		[HttpGet]
		[Route("Comment/Delete")]
		public async Task<IActionResult> Delete(int commentId)
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Delete action called");
			var getCommentResponse = await _commentService.Get(commentId);
			var deleteCommentRequest = _mapper.Map<DeleteCommentRequest>(getCommentResponse);
			return View(deleteCommentRequest);
		}

		/// <summary>
		/// Удаление комментария
		/// </summary>
		/// /// <param name="deleteCommentRequest"></param>
		/// <returns></returns>
		[Authorize(Roles = "Пользователь, Модератор")]
		[HttpPost]
		[Route("Comment/Delete")]
		public async Task<IActionResult> Delete(DeleteCommentRequest deleteCommentRequest)
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Delete action called");
			if (ModelState.IsValid)
			{
                var result = await _commentService.Delete(deleteCommentRequest.CommentId, User.Identity.Name);
                if (result.Errors.FirstOrDefault() != null)
                {
					_logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
					return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
                }
                return RedirectToAction("Get", "Article", new { articleId = deleteCommentRequest.ArticleId });
            }
			else
			{
				return View(deleteCommentRequest);
			}
		}
	}
}
