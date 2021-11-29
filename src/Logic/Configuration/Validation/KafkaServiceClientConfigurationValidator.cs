using System.Diagnostics;

using Microsoft.Extensions.Options;

namespace Logic.Configuration.Validation
{
    /// <summary>
    /// Validator for <see cref="KafkaServiceClientConfiguration"/>.
    /// </summary>
    public class KafkaServiceClientConfigurationValidator : IValidateOptions<KafkaServiceClientConfiguration>
    {
        /// <summary>
        /// Validates <see cref="KafkaServiceClientConfiguration"/>.
        /// </summary>
        public ValidateOptionsResult Validate(string name, KafkaServiceClientConfiguration options)
        {
            Debug.Assert(name is not null);
            Debug.Assert(options is not null);

            if (options.BootstrapServers is null)
            {
                return ValidateOptionsResult.Fail("BootstrapServers section is not set.");
            }

            if (!options.BootstrapServers.Any())
            {
                return ValidateOptionsResult.Fail("BootstrapServers section is empty.");
            }

            if (options.BootstrapServers.Any(x => String.IsNullOrEmpty(x)))
            {
                return ValidateOptionsResult.Fail("BootstrapServers section contains empty string.");
            }

            if (options.BootstrapServers.Any(x => String.IsNullOrWhiteSpace(x)))
            {
                return ValidateOptionsResult.Fail("BootstrapServers section contains empty string of whitespaces.");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
