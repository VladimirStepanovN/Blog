using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Blog.API
{
    public class GlobalExceptionHandler : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            // Проверяем, что исключение не было обработано ранее
            if (!context.ExceptionHandled)
            {
                // Устанавливаем статус кода ответа на 500 (внутренняя ошибка сервера)
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Логируем исключение
                // Здесь можно добавить логирование в файл, базу данных или другой сервис
                Console.WriteLine($"An error occurred: {context.Exception.Message}");

                // Можно также отправить сообщение об ошибке клиенту
                // Здесь можно добавить отправку JSON с информацией об ошибке
                context.Result = new ContentResult
                {
                    Content = $"An error occurred: {context.Exception.Message}",
                    ContentType = "text/plain"
                };

                // Помечаем исключение как обработанное
                context.ExceptionHandled = true;
            }
        }
    }
}
