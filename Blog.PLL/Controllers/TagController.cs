using AutoMapper;
using Blog.BLL.BusinessModels.Requests.TagRequests;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PLL.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
		private readonly ILogger<TagController> _logger;
		private readonly IMapper _mapper;

        public TagController(ITagService tagService, IMapper mapper, ILogger<TagController> logger)
        {
            _tagService = tagService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Создание нового тега
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [Authorize(Roles = "Модератор")]
        [HttpGet]
        [Route("Tag/Create")]
        public async Task<IActionResult> Create()
        {
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Create action called");
			return View();
        }


        /// <summary>
        /// Создание нового тега
        /// </summary>
        /// <param name="addTagRequest"></param>
        /// <returns></returns>
        [Authorize(Roles = "Модератор")]
        [HttpPost]
        [Route("Tag/Create")]
        public async Task<IActionResult> Create(AddTagRequest addTagRequest)
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
                return RedirectToAction("GetAll", "Tag");
            }
            else
            {
                return View(addTagRequest);
            }
        }

        /// <summary>
        /// Редактирование тега
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns name="UpdateTagRequest"></returns>
        [Authorize(Roles = "Модератор")]
        [HttpGet]
        [Route("Tag/Update")]
        public async Task<IActionResult> Update(int tagId)
        {
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Update action called");
			var tag = await _tagService.Get(tagId);
            var view = _mapper.Map<UpdateTagRequest>(tag);
            return View(view);
        }

        /// <summary>
        /// Редактирование тега
        /// </summary>
        /// <param name="updateTagRequest"></param>
        /// <returns></returns>
        [Authorize(Roles = "Модератор")]
        [HttpPost]
        [Route("Tag/Update")]
        public async Task<IActionResult> Update(UpdateTagRequest updateTagRequest)
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
                return RedirectToAction("Get", "Tag", new { tagId = updateTagRequest.TagId });
            }
            else
            {
                return View(updateTagRequest);
            }
		}

        /// <summary>
        /// Получение списка всех тегов
        /// </summary>
        [Authorize(Roles = "Пользователь, Модератор")]
        [HttpGet]
        [Route("Tag/GetAll")]
        public async Task<IActionResult> GetAll()
        {
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] GetAll action called");
			var tags = await _tagService.GetAll();
            return View(tags);
        }

        /// <summary>
        /// Получение тега по его идентификатору
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Пользователь, Модератор")]
        [HttpGet]
        [Route("Tag/Get")]
        public async Task<IActionResult> Get(int tagId)
        {
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Get action called");
			var getTagFullResponse = await _tagService.GetAllInfo(tagId);
            return View(getTagFullResponse);
        }

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// /// <param name="tagId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Модератор")]
        [HttpGet]
        [Route("Tag/Delete")]
        public async Task<IActionResult> Delete(int tagId)
        {
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Delete action called");
			var tag = await _tagService.Get(tagId);
            var view = _mapper.Map<DeleteTagRequest>(tag);
            return View(view);
        }

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// /// <param name="deleteTagRequest"></param>
        /// <returns></returns>
        [Authorize(Roles = "Модератор")]
        [HttpPost]
        [Route("Tag/Delete")]
        public async Task<IActionResult> Delete(DeleteTagRequest deleteTagRequest)
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
                return RedirectToAction("GetAll", "Tag");
            }
            else
            {
                return View(deleteTagRequest);
            }
        }
    }
}
