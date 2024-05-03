using Blog.BLL.BlogConfiguration;
using Blog.BLL.Services.IServices;
using Blog.BLL.Services;
using AutoMapper;
using NLog.Web;
using NLog.Extensions.Logging;

namespace Blog.PLL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //блок проброса настроек
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
			//----------------------


			//добавляем возможность маппинга моделей
			var mapperConfig = new MapperConfiguration((v) =>
            {
                v.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
