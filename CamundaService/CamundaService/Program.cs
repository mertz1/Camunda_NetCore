
using CamundaApp.Interfaces;
using CamundaApp.JobWorkers;
using CamundaApp.Meta;
using CamundaApp.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddTransient<ICamundaService, CamundaService>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        // Publish Camunda Process
        ICamundaService camundaService = new CamundaService(app.Configuration);
        camundaService.DeployProcess(BpmnProcess.File);

        // Start Job Worker
        camundaService.StartWorker(new ImageJobWorker());

        app.Run();
    }
}