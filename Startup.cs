using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoUygulaması.Data;
using ToDoUygulaması.Repositories;
using ToDoUygulaması.Services;

namespace ToDoUygulaması
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // DbContext'i InMemory olarak yapılandırma
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TodoDb"));

            // Repository'leri kaydetme
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Cache servisini kaydetme
            services.AddMemoryCache();
            services.AddScoped<ICacheService, CacheService>();

            // Uygulama servislerini kaydetme
            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Todo}/{action=Index}/{id?}");
            });

            // Veritabanını başlangıçta oluştur
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}