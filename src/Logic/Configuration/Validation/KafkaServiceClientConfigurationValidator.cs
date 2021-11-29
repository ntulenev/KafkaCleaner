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

            return ValidateOptionsResult.Success;
        }
    }
}
