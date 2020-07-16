using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using ODataBatchResponseSample.Data;
using ODataBatchResponseSample.Entities;

namespace ODataBatchResponseSample
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
            services.AddDbContext<SampleDbContext>(opt => opt.UseInMemoryDatabase("sample"));
            services.AddOData();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SampleDbContext db)
        {
            SampleDbInitializer.Initialize(db);

            app.UseODataBatching();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapODataRoute("sample", null, GetEdmModel(), new DefaultODataBatchHandler());
                endpoints.MapControllers();
            });
        }

        public IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            var datumType = typeof(Datum);
            var datumEntityType = builder.AddEntityType(datumType);

            datumEntityType.HasKey(datumType.GetProperty("Id"));

            builder.AddEntitySet("Data", datumEntityType);

            return builder.GetEdmModel();
        }
    }
}
