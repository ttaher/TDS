using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TDS.Domain.Services.Abstract;
using TDS.Infrastructure.Data.Context;
using TDS.Infrastructure.Repositories;
using TDS.Infrastructure.Services;

namespace TDS.API
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

            //services.AddHostedService<DiscoveringPaymentHostedService>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<IAccountInsertHandler, AccountInsertHandler>();
            services.AddScoped<IAccountQueryHandler, AccountQueryHandler>();
            services.AddScoped<IAccountDiscoveringService, AccountDiscoveringService>();
            services.AddAutoMapper(typeof(API.Mapper.AccountProfile));
            services.AddAutoMapper(typeof(Infrastructure.Mapper.AccountProfile));
            services.AddDbContextPool<TDSDbContext>(o => o.UseNpgsql(Configuration.GetConnectionString("PostgresConnection")));


            services.AddHealthChecks();
            services.AddControllers();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TDSDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TDS API V1");
            });
            app.UseRouting();
            app.UseHealthChecks("/health-check");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            if (!context.Database.EnsureCreated())
            {
                context.Database.Migrate();
            }
        }
    }
}
