using CamundaApp.JobWorkers;

namespace CamundaApp.Interfaces
{
    public interface ICamundaService
    {
        Task<string> DeployProcess(string bpmnFile);
        Task<long> Start(string variables);
        void StartWorker(IJobWorker jobWorker);
    }
}
