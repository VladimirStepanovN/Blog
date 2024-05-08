using Blog.BLL.BusinessModels.Requests.TagRequests;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    /// <summary>
    /// Контроллер тегов
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagController> _logger;

        public TagController(ITagService tagService, ILogger<TagController> logger)
        {
            _tagService = tagService;
            _logger = logger;
        }

        /// <summary>
        /// Создание нового тега
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///        "name": "IT-технологии"
        ///     }
        /// </remarks>
        /// <param name="addTagRequest"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Неправильный запрос</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Модератор")]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] AddTagRequest addTagRequest)
        {
            _logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Create action called");
            if (ModelState.IsValid)
            {
                var result = await _tagService.Create(addTagRequest);
                if (result.Errors.FirstOrDefault() != null)
                {
                    _logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
                    return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
                }
                return StatusCode(StatusCodes.Status200OK, addTagRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, addTagRequest);
            }
        }

        /// <summary>
        /// Редактирование тега
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///        "tagId": 1,
        ///        "name": "IT-технологии"
        ///     }
        /// </remarks>
        /// <param name="updateTagRequest"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Неправильный запрос</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Модератор")]
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateTagRequest updateTagRequest)
        {
            _logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Update action called");
            if (ModelState.IsValid)
            {
                var result = await _tagService.Update(updateTagRequest);
                if (result.Errors.FirstOrDefault() != null)
                {
                    _logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
                    return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
                }
                return StatusCode(StatusCodes.Status200OK, updateTagRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, updateTagRequest);
            }
        }

        /// <summary>
        /// Получение списка всех тегов
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
            var tags = await _tagService.GetAll();
            return StatusCode(StatusCodes.Status200OK, tags);
        }

        /// <summary>
        /// Получение тега по его идентификатору
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Пользователь, Модератор")]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int tagId)
        {
            _logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Get action called");
            var getTagResponse = await _tagService.Get(tagId);
            return StatusCode(StatusCodes.Status200OK, getTagResponse);
        }

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     DELETE /Todo
        ///     {
        ///        "tagId": 1,
        ///        "name": "IT-технологии"
        ///     }
        /// </remarks>
        /// <param name="deleteTagRequest"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Неправильный запрос</response>
        /// <response code="401">Неавторизован</response>
        /// <response code="403">Неуполномочен</response>
        [Authorize(Roles = "Модератор")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteTagRequest deleteTagRequest)
        {
            _logger.LogInformation($"{User.Identity.Name} :: [HttpPost] Delete action called");
            if (ModelState.IsValid)
            {
                var result = await _tagService.Delete(deleteTagRequest.TagId);
                if (result.Errors.FirstOrDefault() != null)
                {
                    _logger.LogError($"{User.Identity.Name} :: {result.Errors.FirstOrDefault().Description}");
                    return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
                }
                return StatusCode(StatusCodes.Status200OK, deleteTagRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, deleteTagRequest);
            }
        }
    }
}
