using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using TechChallengeFase1.Data;
using TechChallengeFase1.Data.Mapping;
using TechChallengeFase1.Services;
using TechChallengeFase1.Services.Interfaces;
using Prometheus;
using MassTransit;
using TechChallengeFase3.Consumer.Eventos;
using TechChallengeFase3.Consumer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IContatoService, ContatoService>();

// Adiciona os serviços ao container.
builder.Services.AddControllers();

var filaCreate = builder.Configuration["MassTransit:FilaCreate"];
var filaUpdate = builder.Configuration["MassTransit:FilaUpdate"];
var filaDelete = builder.Configuration["MassTransit:FilaDelete"];

var servidor = builder.Configuration["MassTransit:Servidor"];
var usuario = builder.Configuration["MassTransit:Usuario"];
var senha = builder.Configuration["MassTransit:Senha"];

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ContatoCreateConsumer>();
    x.AddConsumer<ContatoUpdateConsumer>();
    x.AddConsumer<ContatoDeleteConsumer>();

    x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(servidor, "/", h =>
                {
                    h.Username(usuario);
                    h.Password(senha);
                });
                cfg.ReceiveEndpoint(filaCreate, e =>
                {
                    e.Consumer<ContatoCreateConsumer>(context);
                });
                cfg.ReceiveEndpoint(filaUpdate, e =>
                {
                    e.Consumer<ContatoUpdateConsumer>(context);
                });
                cfg.ReceiveEndpoint(filaDelete, e =>
                {
                    e.Consumer<ContatoDeleteConsumer>(context);
                });
            });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ContatoContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        opt => opt.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds));
});

builder.Services.AddSingleton<IBrasilApiService, BrasilApiService>();
builder.Services.AddSingleton<IDDDService, DDDService>();


builder.Services.AddAutoMapper(typeof(DDDMapping));

var app = builder.Build();



// Métricas - Prometheus

var memoryUsageGauge = Metrics.CreateGauge("dotnet_memory_usage_bytes", "Uso da memória em bytes.");
var cpuUsageGauge = Metrics.CreateGauge("dotnet_cpu_usage_percent", "Uso do CPU em porcentagem.");


var timer = new System.Timers.Timer(5000); 
timer.Elapsed += (sender, e) =>
{
    var process = Process.GetCurrentProcess();
    memoryUsageGauge.Set(process.WorkingSet64); 
    cpuUsageGauge.Set(GetCpuUsage(process)); 
};
timer.Start();


static double GetCpuUsage(Process process)
{
    var cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
    return cpuCounter.NextValue();
}


// Configure o pipeline de requisições HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Prometheus 
app.UseMetricServer();
app.UseHttpMetrics();

app.MapControllers();
app.Run();
