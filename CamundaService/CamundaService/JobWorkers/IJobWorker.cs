using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace CamundaApp.JobWorkers
{
    public interface IJobWorker
    {
        public string JobType { get; }

        public abstract void Worker(IJobClient jobClient, IJob job);

        public int MaxJobsActive { get; }
        public TimeSpan PollInterval { get; }
        public TimeSpan TimeOut { get; }
    }
}
