using AutoMapper;
using Blog.BLL.BusinessModels.Requests.ArticleRequests;
using Blog.BLL.BusinessModels.Requests.TagRequests;
using Blog.BLL.BusinessModels.Responses.ArticleResponses;
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
        private readonly IMapper _mapper;

        public ArticleController(IArticleService articleService, IUserService userService, ITagService tagService, IMapper mapper)
        {
            _articleService = articleService;
            _userService = userService;
            _tagService = tagService;
            _mapper = mapper;
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
            var user = await _userService.GetByLogin(User.Identity.Name);
            var tags = addArticleRequest.Tags.Where(t => t.IsSelected == true).ToArray();
            addArticleRequest.Tags = tags;
            addArticleRequest.UserId = user.UserId;
            var result = await _articleService.Create(addArticleRequest);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return RedirectToAction("GetAll", "Article");
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
            var getArticleResponse = await _articleService.GetById(articleId);
            var tags = await _tagService.GetAll();
            var tagRequest = _mapper.Map<GetTagResponse[], TagRequest[]>(tags);
			if (getArticleResponse.Tags != null && getArticleResponse.Tags.Length > 0)
            {
                foreach(var tag in getArticleResponse.Tags)
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
            var tags = updateArticleRequest.Tags.Where(t => t.IsSelected == true).ToArray();
            updateArticleRequest.Tags = tags;
            var result = await _articleService.Update(updateArticleRequest, User.Identity.Name);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return RedirectToAction("Get", "Article", new {articleId = updateArticleRequest.ArticleId});
        }

        /// <summary>
        /// Получение списка всех статей
        /// </summary>
        [Authorize(Roles = "Пользователь, Модератор")]
        [HttpGet]
        [Route("Article/GetAll")]
        public async Task<IActionResult> GetAll()
        {
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
            var article = await _articleService.GetById(articleId);
            return View(article);
        }

        /// <summary>
        /// Получение списка всех статей определенного автора по его Идентификатору
        /// </summary>
        [Authorize(Roles = "Пользователь, Модератор")]
        [HttpGet]
        [Route("Article/GetArticlesByAuthor")]
        public async Task<IActionResult> GetAllByAuthor(int userId)
        {
            var articles = await _articleService.GetAllByUserId(userId);
            return StatusCode(StatusCodes.Status200OK, articles);
            //return View();
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
            var result = await _articleService.Delete(deleteArticleRequest.ArticleId, User.Identity.Name);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return RedirectToAction("GetAll", "Article");
        }
    }
}
