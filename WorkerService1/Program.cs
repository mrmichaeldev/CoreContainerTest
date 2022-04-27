using WorkerService1;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        //services.AddTransient<RulesProcessor>();
    })
    .Build();

await host.RunAsync();
