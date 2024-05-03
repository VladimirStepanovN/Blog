using AutoMapper;
using Blog.BLL.BusinessModels.Requests.ArticleRequests;
using Blog.BLL.BusinessModels.Requests.TagRequests;
using Blog.BLL.BusinessModels.Responses.TagResponses;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PLL.Controllers
{
	public class ArticleController : Controller
	{
		private readonly IArticleService _articleService;
		private readonly IUserService _userService;
		private readonly ITagService _tagService;
		private readonly ILogger<ArticleController> _logger;
		private readonly IMapper _mapper;

		public ArticleController(IArticleService articleService, IUserService userService, ITagService tagService, IMapper mapper, ILogger<ArticleController> logger)
		{
			_articleService = articleService;
			_userService = userService;
			_tagService = tagService;
			_mapper = mapper;
			_logger = logger;
		}

		/// <summary>
		/// Создание новой статьи
		/// </summary>
		/// <param></param>
		/// <returns></returns>
		[Authorize(Roles = "Пользователь")]
		[HttpGet]
		[Route("Article/Create")]
		public async Task<IActionResult> Create()
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Create action called");
			var tags = await _tagService.GetAll();
			var tagRequests = _mapper.Map<GetTagResponse[], TagRequest[]>(tags);
			var addArticleRequest = new AddArticleRequest { Tags = tagRequests };
			return View(addArticleRequest);
		}


		/// <summary>
		/// Создание новой статьи
		/// </summary>
		/// <param name="addArticleRequest"></param>
		/// <returns></returns>
		[Authorize(Roles = "Пользователь")]
		[HttpPost]
		[Route("Article/Create")]
		public async Task<IActionResult> Create(AddArticleRequest addArticleRequest)
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Create action called");
			if (ModelState.IsValid)
			{
                var user = await _userService.GetByLogin(User.Identity.Name);
                var tags = addArticleRequest.Tags.Where(t => t.IsSelected == true).ToArray();
                addArticleRequest.Tags = tags;
                addArticleRequest.UserId = user.UserId;
                var result = await _articleService.Create(addArticleRequest);
                if (result.Errors.FirstOrDefault() != null)
                {
					_logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
					return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
                }
                return RedirectToAction("GetAll", "Article");
            }
			else
			{
				return View(addArticleRequest);
			}
		}

		/// <summary>
		/// Обновление статьи
		/// </summary>
		/// <param name="articleId"></param>
		/// <returns></returns>
		[Authorize(Roles = "Пользователь, Модератор")]
		[HttpGet]
		[Route("Article/Update")]
		public async Task<IActionResult> Update(int articleId)
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Update action called");
			var getArticleResponse = await _articleService.GetById(articleId);
			var tags = await _tagService.GetAll();
			var tagRequest = _mapper.Map<GetTagResponse[], TagRequest[]>(tags);
			if (getArticleResponse.Tags != null && getArticleResponse.Tags.Length > 0)
			{
				foreach (var tag in getArticleResponse.Tags)
				{
					tagRequest.Where(t => t.TagId == tag.TagId).FirstOrDefault().IsSelected = true;
				}
			}
			getArticleResponse.Tags = tagRequest;
			var updateArticleRequest = _mapper.Map<UpdateArticleRequest>(getArticleResponse);
			return View(updateArticleRequest);
		}

		/// <summary>
		/// Обновление статьи
		/// </summary>
		/// <param name="updateArticleRequest"></param>
		/// <returns></returns>
		[Authorize(Roles = "Пользователь, Модератор")]
		[HttpPost]
		[Route("Article/Update")]
		public async Task<IActionResult> Update(UpdateArticleRequest updateArticleRequest)
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Update action called");
			if (ModelState.IsValid)
			{
                var tags = updateArticleRequest.Tags.Where(t => t.IsSelected == true).ToArray();
                updateArticleRequest.Tags = tags;
                var result = await _articleService.Update(updateArticleRequest, User.Identity.Name);
                if (result.Errors.FirstOrDefault() != null)
                {
					_logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
					return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
                }
                return RedirectToAction("Get", "Article", new { articleId = updateArticleRequest.ArticleId });
            }
			else
			{
				return View(updateArticleRequest);
			}
		}

		/// <summary>
		/// Получение списка всех статей
		/// </summary>
		[Authorize(Roles = "Пользователь, Модератор")]
		[HttpGet]
		[Route("Article/GetAll")]
		public async Task<IActionResult> GetAll()
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] GetAll action called");
			var articles = await _articleService.GetAll();
			return View(articles);
		}

		/// <summary>
		/// Получение статьи по идентификатору
		/// </summary>
		[Authorize(Roles = "Пользователь, Модератор")]
		[HttpGet]
		[Route("Article/Get")]
		public async Task<IActionResult> Get(int articleId)
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Get action called");
			var article = await _articleService.GetById(articleId);
			return View(article);
		}

		/// <summary>
		/// Удаление статьи
		/// </summary>
		/// /// <param name="articleId"></param>
		/// <returns></returns>
		[Authorize(Roles = "Пользователь, Модератор")]
		[HttpGet]
		[Route("Article/Delete")]
		public async Task<IActionResult> Delete(int articleId)
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Delete action called");
			var getArticleResponse = await _articleService.GetById(articleId);
			var deleteArticleRequest = _mapper.Map<DeleteArticleRequest>(getArticleResponse);
			return View(deleteArticleRequest);
		}

		/// <summary>
		/// Удаление статьи
		/// </summary>
		/// /// <param name="deleteArticleRequest"></param>
		/// <returns></returns>
		[Authorize(Roles = "Пользователь, Модератор")]
		[HttpPost]
		[Route("Article/Delete")]
		public async Task<IActionResult> Delete(DeleteArticleRequest deleteArticleRequest)
		{
			_logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Delete action called");
			if (ModelState.IsValid)
			{
                var result = await _articleService.Delete(deleteArticleRequest.ArticleId, User.Identity.Name);
                if (result.Errors.FirstOrDefault() != null)
                {
					_logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
					return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
                }
                return RedirectToAction("GetAll", "Article");
            }
			else
			{
				return View(deleteArticleRequest);
			}
		}
	}
}
