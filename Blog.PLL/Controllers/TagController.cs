using AutoMapper;
using Blog.BLL.BusinessModels.Requests.TagRequests;
using Blog.BLL.BusinessModels.Responses.TagResponses;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PLL.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public TagController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
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
            var result = await _tagService.Create(addTagRequest);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return RedirectToAction("GetAll", "Tag");
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
            var tag = await _tagService.Get(tagId);
            var view = _mapper.Map<UpdateTagRequest>(tag);
            return View(view);
        }

        /// <summary>
        /// Редактирование тега
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="updateTagRequest"></param>
        /// <returns></returns>
        [Authorize(Roles = "Модератор")]
        [HttpPost]
        [Route("Tag/Update")]
        public async Task<IActionResult> Update(UpdateTagRequest updateTagRequest)
        {
            var result = await _tagService.Update(updateTagRequest);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return StatusCode(StatusCodes.Status200OK, updateTagRequest);
            //return View();
        }

        /// <summary>
        /// Получение списка всех тегов
        /// </summary>
        [Authorize(Roles = "Пользователь, Модератор")]
        [HttpGet]
        [Route("Tag/GetAll")]
        public async Task<IActionResult> GetAll()
        {
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
            var tag = await _tagService.Get(tagId);
            var view = _mapper.Map<DeleteTagRequest>(tag);
            return View(view);
        }

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// /// <param name="tagId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Модератор")]
        [HttpPost]
        [Route("Tag/Delete")]
        public async Task<IActionResult> Delete(DeleteTagRequest deleteTagRequest)
        {
            var result = await _tagService.Delete(deleteTagRequest.TagId);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            //return StatusCode(StatusCodes.Status200OK, $"$Тег {deleteTagRequest.TagId} удален");
            return RedirectToAction("GetAll", "Tag");
        }
    }
}
