using CamundaApp.Configurations;
using CamundaApp.Interfaces;
using CamundaApp.JobWorkers;
using CamundaApp.Meta;
using Google.Apis.Auth.OAuth2;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Impl.Builder;

namespace CamundaApp.Services
{

    public class CamundaService : ICamundaService
    {
        private static IZeebeClient zeebeClient;
        public CamundaService(IConfiguration configuration)
        {
            ZeebeConfiguration zeebeConfiguration = new ZeebeConfiguration();
            configuration.GetSection(ZeebeConfiguration.Name).Bind(zeebeConfiguration);

            // If ClientSecret and ClientId are set, assume that we are using SaaS Camunda, and 
            // thus use the cloud client builder.  Else assume a locally running config
            if (zeebeConfiguration.ClientSecret != null && zeebeConfiguration.ClientId != null)
            {
                zeebeClient = CamundaCloudClientBuilder
                      .Builder()
                      .UseClientId(zeebeConfiguration.ClientId)
                      .UseClientSecret(zeebeConfiguration.ClientSecret)
                      .UseContactPoint(zeebeConfiguration.ZeebeUrl)
                      .Build();
            }
            else
            {

                zeebeClient = ZeebeClient.Builder()
                .UseGatewayAddress(zeebeConfiguration.ZeebeUrl)
                .UsePlainText()
                .Build();
            }
        }

        public async Task<string> DeployProcess(string bpmnFile)
        {
            var deployRespone = await zeebeClient.NewDeployCommand()
                .AddResourceFile(bpmnFile)
                .Send();

            Console.WriteLine("Process Definition has been deployed!");

            var bpmnProcessId = deployRespone.Processes[0].BpmnProcessId;
            return bpmnProcessId;
        }

        public async Task<long> Start(string variables)
        {
            IProcessInstanceResponse response = await zeebeClient.NewCreateProcessInstanceCommand()
                .BpmnProcessId(BpmnProcess.Id)
                .LatestVersion()
                .Variables(variables)
                .Send();

            return response.ProcessInstanceKey;
        }

        public void StartWorker(IJobWorker jobWorker)
        {
            using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
            {
                zeebeClient.NewWorker()
                           .JobType(jobWorker.JobType)
                           .Handler(jobWorker.Worker)
                           .MaxJobsActive(5)
                           .Name(Environment.MachineName)
                           .AutoCompletion()
                           .PollInterval(TimeSpan.FromSeconds(1))
                           .Timeout(TimeSpan.FromSeconds(10))
                           .Open();

                signal.WaitOne();
            }
        }
    }
}
