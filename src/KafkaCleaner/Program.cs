using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

using Abstractions;
using KafkaCleaner.UI;
using Logic;
using Logic.Configuration;

// It is unfortunate but we have to set it to Unknown first.
Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

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

      var logger = new LoggerConfiguration()
                       .ReadFrom.Configuration(hostContext.Configuration)
                       .CreateLogger();

      services.AddLogging(x =>
      {
          x.SetMinimumLevel(LogLevel.Information);
          x.AddSerilog(logger: logger, dispose: true);
      });
  });

var host = builder.Build();

using (var serviceScope = host.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var form = services.GetRequiredService<KafkaListWindow>();
    Application.Run(form);
}
