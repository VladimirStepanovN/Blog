using Blog.BLL.BusinessModels.Requests.CommentRequests;
using Blog.BLL.BusinessModels.Requests.TagRequests;
using Blog.BLL.Services;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PLL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        //для формы добавления тега
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        /// <summary>
        /// Создание нового тега
        /// </summary>
        /// <param name="addTagRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddTag")]
        public async Task<IActionResult> Create([FromBody] AddTagRequest addTagRequest)
        {
            var result = await _tagService.Create(addTagRequest);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return StatusCode(StatusCodes.Status200OK, addTagRequest);
            //return View();
        }

        /// <summary>
        /// Редактирование тега
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="updateTagRequest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateTag")]
        public async Task<IActionResult> Update(int tagId, [FromBody] UpdateTagRequest updateTagRequest)
        {
            var result = await _tagService.Update(tagId, updateTagRequest);
            return StatusCode(StatusCodes.Status200OK, updateTagRequest);
            //return View();
        }

        /// <summary>
        /// Получение списка всех тегов
        /// </summary>
        [HttpGet]
        [Route("GetTags")]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagService.GetAll();
            return StatusCode(StatusCodes.Status200OK, tags);
            //return View();
        }

        /// <summary>
        /// Получение тега по его идентификатору
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTag")]
        public async Task<IActionResult> Get(int tagId)
        {
            var tag = await _tagService.Get(tagId);
            return StatusCode(StatusCodes.Status200OK, tag);
            //return View();
        }

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteTag")]
        public async Task<IActionResult> Delete(int tagId)
        {
            var result = await _tagService.Delete(tagId);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return StatusCode(StatusCodes.Status200OK, $"$Тег {tagId} удален");
            //return View();
        }


        /*public IActionResult Index()
        {
            return View();
        }*/
    }
}
