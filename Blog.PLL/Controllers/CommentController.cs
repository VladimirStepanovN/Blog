using Blog.BLL.BusinessModels.Requests.CommentRequests;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PLL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        //для формы добавления комментария
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        /// <summary>
        /// Создание нового комментария
        /// </summary>
        /// <param name="addCommentRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddComment")]
        public async Task<IActionResult> Create([FromBody] AddCommentRequest addCommentRequest)
        {
            var result = await _commentService.Create(addCommentRequest);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return StatusCode(StatusCodes.Status200OK, addCommentRequest);
            //return View();
        }

        /// <summary>
        /// Редактирование комментария
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="updateCommentRequest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateComment")]
        public async Task<IActionResult> Update(int commentId, [FromBody] UpdateCommentRequest updateCommentRequest)
        {
            var result = await _commentService.Update(commentId, updateCommentRequest);
            return StatusCode(StatusCodes.Status200OK, updateCommentRequest);
            //return View();
        }

        /// <summary>
        /// Получение списка всех комментариев
        /// </summary>
        [HttpGet]
        [Route("GetComments")]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentService.GetAll();
            return StatusCode(StatusCodes.Status200OK, comments);
            //return View();
        }

        /// <summary>
        /// Получение комментария по его идентификатору
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
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
        [HttpDelete]
        [Route("DeleteComment")]
        public async Task<IActionResult> Delete(int commentId)
        {
            var result = await _commentService.Delete(commentId);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return StatusCode(StatusCodes.Status200OK, $"$Комментарий {commentId} удален");
            //return View();
        }

        /*public IActionResult Index()
        {
            return View();
        }*/
    }
}
