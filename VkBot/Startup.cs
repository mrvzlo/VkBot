using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using VkBot.Entities;
using VkBot.Repositories;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            bool.TryParse(Configuration["Config:IsLocal"], out var isLocal);

            services.AddSingleton<IVkApi>(sp =>
            {
                var api = new VkApi();
                if (!isLocal)
                    api.Authorize(new ApiAuthParams { AccessToken = Configuration["Config:AccessToken"] });
                return api;
            });

            services.AddDbContextPool<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Default")).EnableSensitiveDataLogging());

            services.AddScoped<IMemoryService, MemoryService>();
            services.AddScoped<IReplyService, ReplyService>();
            services.AddScoped<ISizeRepository, SizeRepository>();
            services.AddScoped<IPairRepository, PairRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
