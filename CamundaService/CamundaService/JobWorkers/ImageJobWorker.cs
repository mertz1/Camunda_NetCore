using Newtonsoft.Json;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace CamundaApp.JobWorkers
{
    public class ImageJobWorker : IJobWorker
    {
        public ImageJobWorker() { }

        public string JobType
        {
            get { return "getImage"; }
        }

        public void Worker(IJobClient jobClient, IJob job)
        {
            jobClient.NewCompleteJobCommand(job.Key)
             .Send()
             .GetAwaiter()
             .GetResult();

            Console.WriteLine("Completed the fetched Task");
        }

        public int MaxJobsActive
        {
            get { return 5; }
        }

        public TimeSpan PollInterval
        {
            get { return TimeSpan.FromSeconds(1); }
        }

        public TimeSpan TimeOut
        {
            get { return TimeSpan.FromSeconds(10); }
        }

    }
}
