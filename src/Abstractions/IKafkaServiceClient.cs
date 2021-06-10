using System.Collections.Generic;
using System.Threading.Tasks;

using Models;

namespace Abstractions
{
    public interface IKafkaServiceClient
    {
        public IEnumerable<Topic> RequestTopicsList();

        public Task DeleteTopicAsync(Topic topic);
    }
}
