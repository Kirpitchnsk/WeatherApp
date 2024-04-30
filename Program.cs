using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherApp.Services;

namespace WeatherApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Добавляем сервис логирования
            services.AddLogging(logging =>
            {
                logging.AddConsole(); // Вывод логов в консоль
                logging.AddDebug(); // Вывод отладочных логов
                                    // Можно добавить другие провайдеры логирования по необходимости
            });

            services.AddControllers().AddXmlSerializerFormatters(); 
            services.AddHttpClient();
            services.AddTransient<WeatherService>();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();         
            app.UseRouting();
            app.UseAuthorization();

            // Добавляем middleware для установки заголовков по умолчанию
            app.Use(async (context, next) =>
            {
                context.Request.Headers.Append("Accept", "application/xml");
                context.Request.Headers.Append("Accept", "application/json");
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
