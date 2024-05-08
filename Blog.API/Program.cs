using Blog.BLL.BlogConfiguration;
using Blog.BLL.Services.IServices;
using Blog.BLL.Services;
using NLog.Web;
using NLog.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionSettings = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionSettings>();
            builder.Services.AddSingleton(connectionSettings);
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IArticleService, ArticleService>();
            builder.Services.AddTransient<ICommentService, CommentService>();
            builder.Services.AddTransient<ITagService, TagService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication(options => options.DefaultScheme = "Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = redirectContext =>
                        {
                            redirectContext.HttpContext.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddConsole();
                loggingBuilder.AddNLog();
            });

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Blog API",
                    Description = "Web API для учебного проекта Blog",
                    Contact = new OpenApiContact
                    {
                        Name = "Автор: Владимир Степанов",
                        Url = new Uri("mailto:Vladimir_stepanov00@mail.ru")
                    }
                });
            });

            builder.Services.AddSwaggerGen(options =>
            {
                var basePath = AppContext.BaseDirectory;

                var xmlPath = Path.Combine(basePath, "Blog.API.xml");
                options.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddTransient<IExceptionFilter, GlobalExceptionHandler>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
