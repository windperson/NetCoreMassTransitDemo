using System;
using MassTransit;
using MassTransit.AspNetCoreIntegration;
using MassTransit.Azure.ServiceBus.Core;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus.Primitives;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedContract;

namespace WebDemo
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMassTransit(CreateAzureServiceBus, ConfigServiceBus);
        }

        private static IBusControl CreateAzureServiceBus(IServiceProvider provider)
        {
            return Bus.Factory.CreateUsingAzureServiceBus(config =>
            {
                config.UseSerilog();
                var host = config.Host(new Uri(ConstantForAzureServiceBus.ServiceBusUrl), hostCfg =>
                {
                    hostCfg.TokenProvider =
                        TokenProvider.CreateSharedAccessSignatureTokenProvider(
                            ConstantForAzureServiceBus.KeyName,
                            @"Nk8YFq5WKvVFM0sp+DAh9spopa/gUxBYQps0VF4rJi4=");

                    //hostCfg.OperationTimeout = TimeSpan.FromSeconds(10);
                });
            });
        }

        private static void ConfigServiceBus(IServiceCollectionConfigurator config)
        {
            var serviceAddress = new Uri($"{ConstantForAzureServiceBus.ServiceBusUrl}/{Constant.DemoQueueName}");
            config.Collection.AddScoped<IRequestClient<ISubmitOrder, IOrderAccepted>>(x =>
                new MessageRequestClient<ISubmitOrder, IOrderAccepted>(x.GetRequiredService<IBus>(), serviceAddress, TimeSpan.FromSeconds(10))
            );
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvcWithDefaultRoute();
        }
    }
}
