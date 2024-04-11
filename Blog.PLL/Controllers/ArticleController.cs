using Blog.BLL.BusinessModels.Requests.ArticleRequests;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PLL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        //для формы добавления статьи
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        /// <summary>
        /// Создание новой статьи
        /// </summary>
        /// <param name="addArticleRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddArticle")]
        public async Task<IActionResult> Create([FromBody] AddArticleRequest addArticleRequest)
        {
            var result = await _articleService.Create(addArticleRequest);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return StatusCode(StatusCodes.Status200OK, addArticleRequest);
            //return View();
        }

        /// <summary>
        /// Обновление статьи
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="updateArticleRequest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateArticle")]
        public async Task<IActionResult> Update(int articleId, [FromBody] UpdateArticleRequest updateArticleRequest)
        {
            var result = await _articleService.Update(articleId, updateArticleRequest);
            return StatusCode(StatusCodes.Status200OK, updateArticleRequest);
            //return View();
        }

        /// <summary>
        /// Получение списка всех статей
        /// </summary>
        [HttpGet]
        [Route("GetArticles")]
        public async Task<IActionResult> GetAll()
        {
            var articles = await _articleService.GetAll();
            return StatusCode(StatusCodes.Status200OK, articles);
            //return View();
        }

        /// <summary>
        /// Получение списка всех статей определенного автора по его Идентификатору
        /// </summary>
        [HttpGet]
        [Route("GetArticlesByAuthor")]
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
        [HttpDelete]
        [Route("DeleteArticle")]
        public async Task<IActionResult> Delete(int articleId)
        {
            var result = await _articleService.Delete(articleId);
            if (result.Errors.FirstOrDefault() != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault().Description);
            }
            return StatusCode(StatusCodes.Status200OK, $"$Статья {articleId} удалена");
            //return View();
        }

        /*public IActionResult Index()
        {
            return View();
        }*/
    }
}
