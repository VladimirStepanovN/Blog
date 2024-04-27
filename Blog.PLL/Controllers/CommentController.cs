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
		private readonly IMapper _mapper;

		public CommentController(ICommentService commentService, IUserService userService, IMapper mapper)
		{
			_commentService = commentService;
			_userService = userService;
			_mapper = mapper;
		}

		//для формы добавления комментария
		[Authorize(Roles = "Пользователь, Модератор")]
		[HttpGet]
		[Route("Comment/Create")]
		public async Task<IActionResult> Create(int articleId)
		{
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
			var user = await _userService.GetByLogin(User.Identity.Name);
			addCommentRequest.UserId = user.UserId;
			var result = await _commentService.Create(addCommentRequest);
			if (result.Errors.FirstOrDefault() != null)
			{
				return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
			}
			return RedirectToAction("Get", "Article", new { articleId = addCommentRequest.ArticleId });
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
			var result = await _commentService.Update(updateCommentRequest, User.Identity.Name);
			if (result.Errors.FirstOrDefault() != null)
			{
				return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
			}
			return RedirectToAction("Get", "Article", new {articleId = updateCommentRequest.ArticleId});
		}

		/// <summary>
		/// Получение списка всех комментариев
		/// </summary>
		[Authorize(Roles = "Модератор")]
		[HttpGet]
		[Route("Comment/GetAll")]
		public async Task<IActionResult> GetAll()
		{
			var comments = await _commentService.GetAll();
			return View(comments);
		}

		/// <summary>
		/// Получение комментария по его идентификатору
		/// </summary>
		/// <param name="commentId"></param>
		/// <returns></returns>
		[Authorize(Roles = "Модератор")]
		[HttpGet]
		[Route("GetComment")]
		public async Task<IActionResult> Get(int commentId)
		{
			var comment = await _commentService.Get(commentId);
			return StatusCode(StatusCodes.Status200OK, comment);
			//return View();
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
			var result = await _commentService.Delete(deleteCommentRequest.CommentId, User.Identity.Name);
			if (result.Errors.FirstOrDefault() != null)
			{
				return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
			}
			return RedirectToAction("Get", "Article", new { articleId = deleteCommentRequest.ArticleId });
		}
	}
}
