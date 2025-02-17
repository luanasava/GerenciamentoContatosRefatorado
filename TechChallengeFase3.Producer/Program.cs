using MassTransit;
using TechChallengeFase3.Producer.Services;
using TechChallengeFase3.Producer.Services.Interfaces;
//using k8s;

//string apiServerUrl = "http://127.0.0.1:8001";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConsumerService, ConsumerService>();
builder.WebHost.UseUrls("http://0.0.0.0:3000");

var configuration = builder.Configuration;
var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;

//var config = new KubernetesClientConfiguration { Host = apiServerUrl };
//var client = new Kubernetes(config);

string ns = "default";

//var services = client.ListNamespacedService(ns).Items;

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(servidor, "/", h =>
        {
            h.Username(usuario);
            h.Password(senha);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
