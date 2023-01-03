using System.Security.Cryptography;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Mvc;

namespace E2
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages(o =>
            {
                // this is to make demos easier
                // don't do this in production
                o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            }).AddRazorRuntimeCompilation();
            // dependencies for server sent events
            // Lib.AspNetCore.ServerSentEvents
            builder.Services.AddServerSentEvents();
            builder.Services.AddHostedService<ServerEventsWorker>();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }

    public class ServerEventsWorker : BackgroundService
    {
        private readonly IServerSentEventsService client;

        public ServerEventsWorker(IServerSentEventsService client)
        {
            this.client = client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var clients = client.GetClients();
                    if (clients.Any())
                    {
                        Number.Value = RandomNumberGenerator.GetInt32(1, 100);
                        await client.SendEventAsync(
                            new ServerSentEvent
                            {
                                Id = "number",
                                Type = "number",
                                Data = new List<string>
                                {
                                    Number.Value.ToString()
                                }
                            },
                            stoppingToken
                        );
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
                // this exception is expected
            }
        }
    }
    
    public static class Number
    {
        public static int Value { get; set; } = 1;
    }
}