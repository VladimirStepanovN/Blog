using Blog.BLL.BusinessModels.Requests.CommentRequests;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    /// <summary>
    /// Контроллер комментариев
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentService commentService, IUserService userService, ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Создание нового комментария
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///        "content": "Прекрасная статья",
        ///        "userId": 5,
        ///        "articleId": 123
        ///     }
        /// </remarks>
        /// <param name="addCommentRequest"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Неправильный запрос</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Пользователь, Модератор")]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] AddCommentRequest addCommentRequest)
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
                return StatusCode(StatusCodes.Status200OK, addCommentRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, addCommentRequest);
            }
        }

        /// <summary>
        /// Редактирование комментария
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///        "commentId": 1,
        ///        "content": "Прекрасная статья",
        ///        "articleId": 123,
        ///        "userId": 5
        ///     }
        /// </remarks>
        /// <param name="updateCommentRequest"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Неправильный запрос</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Пользователь, Модератор")]
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateCommentRequest updateCommentRequest)
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
                return StatusCode(StatusCodes.Status200OK, updateCommentRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, updateCommentRequest);
            }
        }

        /// <summary>
        /// Получение списка всех комментариев
        /// </summary>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Модератор")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation($"{User.Identity.Name} :: [HttpGet] GetAll action called");
            var comments = await _commentService.GetAll();
            return StatusCode(StatusCodes.Status200OK, comments);
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///        "commentId": 1,
        ///        "content": "Прекрасная статья",
        ///        "articleId": 123,
        ///        "userId": 5
        ///     }
        /// </remarks>
        /// <param name="deleteCommentRequest"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Неправильный запрос</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Пользователь, Модератор")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteCommentRequest deleteCommentRequest)
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
                return StatusCode(StatusCodes.Status200OK, deleteCommentRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, deleteCommentRequest);
            }
        }
    }
}
