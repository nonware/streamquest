using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;

await Host.CreateDefaultBuilder(args)
    .UseOrleans(sb =>
    {
        sb
        .UseLocalhostClustering()
        .AddMemoryGrainStorage("PubSubStore")
        .AddSimpleMessageStreamProvider("room", options => { options.FireAndForgetDelivery = true; });
    })
    .RunConsoleAsync();
