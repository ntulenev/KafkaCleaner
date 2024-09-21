using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Serilog;

using Confluent.Kafka;

using Abstractions;
using KafkaCleaner.UI;
using Logic;
using Logic.Configuration;
using Logic.Configuration.Validation;

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
      services.AddSingleton(s =>
      {
          var section = hostContext.Configuration.GetSection(nameof(BootstrapConfiguration));
          var config = section.Get<BootstrapConfiguration>();

          Debug.Assert(config is not null);

          var validator = s.GetRequiredService<IValidateOptions<BootstrapConfiguration>>();

          // Crutch to use IValidateOptions in manual generation logic.
          var validationResult = validator.Validate(string.Empty, config);
          if (validationResult.Failed)
          {
              throw new OptionsValidationException
                  (string.Empty, typeof(BootstrapConfiguration), [validationResult.FailureMessage]);
          }

          var adminConfig = new AdminClientConfig
          {
              BootstrapServers = string.Join(",", config.BootstrapServers),
              SecurityProtocol = config.SecurityProtocol,
              SaslMechanism = config.SASLMechanism,
              SaslUsername = config.Username,
              SaslPassword = config.Password
          };

          return new AdminClientBuilder(adminConfig).Build();

      });
      services.Configure<KafkaServiceClientConfiguration>(
          hostContext.Configuration.GetSection(nameof(KafkaServiceClientConfiguration)));
      services.Configure<BootstrapConfiguration>(hostContext.Configuration.GetSection(nameof(BootstrapConfiguration)));
      services.AddSingleton<IValidateOptions<BootstrapConfiguration>, BootstrapConfigurationValidator>();

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

    try
    {
        var form = services.GetRequiredService<KafkaListWindow>();
        Application.Run(form);
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message, "Application error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
