using Blog.BLL.BusinessModels.Requests.ArticleRequests;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    /// <summary>
    /// Контроллер статей
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IUserService _userService;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(IArticleService articleService, IUserService userService, ILogger<ArticleController> logger)
        {
            _articleService = articleService;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Создание новой статьи
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "title": "Новинки в IT",
        ///         "content": "Microsoft открыл пользователям доступ к Copilot",
        ///         "userId": 2,
        ///         "tags": [
        ///                     {
        ///                         "tagId": 1,
        ///                         "name": "IT-Технологии",
        ///                         "isSelected": true
        ///                     }
        ///                 ]
        ///     }
        /// </remarks>
        /// <param name="addArticleRequest"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Неправильный запрос</response>
        /// <response code="401">Неавторизован</response>
        [Authorize(Roles = "Пользователь")]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] AddArticleRequest addArticleRequest)
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
                return StatusCode(StatusCodes.Status200OK, addArticleRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, addArticleRequest);
            }
        }

        /// <summary>
        /// Обновление статьи
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "articleId": 2,
        ///         "title": "Новинки в IT",
        ///         "content": "Microsoft открыл пользователям доступ к Copilot",
        ///         "userId": 2,
        ///         "tags": [
        ///                     {
        ///                         "tagId": 1,
        ///                         "name": "IT-Технологии",
        ///                         "isSelected": true
        ///                     }
        ///                 ]
        ///     }
        /// </remarks>
        /// <param name="updateArticleRequest"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Неправильный запрос</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Пользователь, Модератор")]
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateArticleRequest updateArticleRequest)
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
                return StatusCode(StatusCodes.Status200OK, updateArticleRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, updateArticleRequest);
            }
        }

        /// <summary>
        /// Получение списка всех статей
        /// </summary>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Пользователь, Модератор")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation($"{User.Identity.Name} :: [HttpGet] GetAll action called");
            var articles = await _articleService.GetAll();
            return StatusCode(StatusCodes.Status200OK, articles);
        }

        /// <summary>
        /// Получение статьи по идентификатору
        /// </summary>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Пользователь, Модератор")]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int articleId)
        {
            _logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Get action called");
            var article = await _articleService.GetById(articleId);
            return StatusCode(StatusCodes.Status200OK, article);
        }

        /// <summary>
        /// Удаление статьи
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     DELETE /Todo
        ///     {
        ///         "articleId": 2,
        ///         "title": "Новинки в IT",
        ///         "content": "Microsoft открыл пользователям доступ к Copilot",
        ///         "tags": [
        ///                     {
        ///                         "tagId": 1,
        ///                         "name": "IT-Технологии",
        ///                         "isSelected": true
        ///                     }
        ///                 ]
        ///     }
        /// </remarks>
        /// <param name="deleteArticleRequest"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Неправильный запрос</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Пользователь, Модератор")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteArticleRequest deleteArticleRequest)
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
                return StatusCode(StatusCodes.Status200OK, deleteArticleRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, deleteArticleRequest);
            }
        }
    }
}
