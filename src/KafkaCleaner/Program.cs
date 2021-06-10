using System;
using System.Windows.Forms;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Abstractions;
using KafkaCleaner.UI;
using Logic;

namespace KafkaCleanerApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var builder = new HostBuilder()
              .ConfigureAppConfiguration((hostingContext, config) =>
              {
                  config.AddJsonFile("appsettings.json", optional: true);
              })
              .ConfigureServices((hostContext, services) =>
              {
                  services.AddScoped<KafkaListWindow>();
                  services.AddSingleton<IKafkaServiceClient, KafkaServiceClient>();
                  services.Configure<KafkaServiceClientConfiguration>(hostContext.Configuration.GetSection(nameof(KafkaServiceClientConfiguration)));
              });

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var form = services.GetRequiredService<KafkaListWindow>();
                Application.Run(form);
            }
        }
    }
}
