using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSA.Shared;
using LMSA.Tasks.DAL.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LMSA.Tasks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services, RabbitMQManager rabbitMQ)
        {
            services.AddControllers();
            services.AddDbContext<TasksDbContext>(
                options =>
                {
                    options.UseSqlServer(Configuration["ConnectionString"]);
                }
            );


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();

            var channel = rabbitMQ.GetChannel;

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                // copy or deserialise the payload
                // and process the message
                // ...
                channel.BasicAck(ea.DeliveryTag, false);
            };
            // this consumer tag identifies the subscription
            // when it has to be cancelled
            String consumerTag = channel.BasicConsume("lmsa-projects", false, consumer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LMSA.Tasks API");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
