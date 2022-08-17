using Orleans;
using Orleans.Hosting;
using Spectre.Console;
using StreamQuest.Client;
using StreamQuest.Shared;
using StreamQuest.Shared.Enums;
using StreamQuest.Shared.Interfaces;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .ConfigureApplicationParts(
        parts => parts.AddApplicationPart(typeof(ICharacterGrain).Assembly).WithReferences())
    .AddSimpleMessageStreamProvider("chat")
    .Build();


ClientContext context = new(client);

await StartAsync(context);

var player = context.Client.GetGrain<ICharacterGrain>(Guid.NewGuid());
var name = AnsiConsole.Ask<string>("What is your [yellow]name[/]?");
await player.SetName(name);

context = context with
{
    Character = player
};

await ProcessLoopAsync(context);

static Task StartAsync(ClientContext context) =>
    AnsiConsole.Status().StartAsync("Connecting to server", async ctx =>
    {
        ctx.Spinner(Spinner.Known.Dots);
        ctx.Status = "Connecting...";

        await context.Client.Connect(async error =>
        {
            AnsiConsole.MarkupLine("[bold red]Error:[/] error connecting to server!");
            AnsiConsole.WriteException(error);
            ctx.Status = "Waiting to retry...";
            await Task.Delay(TimeSpan.FromSeconds(2));
            ctx.Status = "Retrying connection...";
            return true;
        });

        ctx.Status = "Connected!";
    }
);

static async Task ProcessLoopAsync(ClientContext context)
{
    string? input = null;
    do
    {
        input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            continue;
        }

        if (input.StartsWith("/exit") &&
            AnsiConsole.Confirm("Do you really want to exit?"))
        {
            break;
        }
        var firstTwoCharacters = input[..2];
        if (firstTwoCharacters switch
        {
            "/n" => EnterRoom(context, input.Replace("/n", "").Trim()),
            _ => null
        } is Task<ClientContext> cxtTask)
        {
            context = await cxtTask;
            continue;
        }

        await SendMessage(context, input);
    } while (input is not "/exit");
}

static async Task<ClientContext> EnterRoom(ClientContext context, string roomName)
{
    context = context with { CurrentRoom = roomName };
    await AnsiConsole.Status().StartAsync("Entering room...", async ctx =>
    {
        var room = context.Client.GetGrain<IRoomGrain>(context.CurrentRoom);
        var streamId = await room.Enter(context.Character!, Direction.North);
        var stream =
            context.Client
                .GetStreamProvider("chat")
                .GetStream<ActionMessage>(streamId, "default");

        //subscribe to the stream to receive further messages sent to the room
        await stream.SubscribeAsync(new StreamObserver(roomName));
    });
    return context;
}

static async Task SendMessage(
    ClientContext context,
    string messageText)
{
    var room = context.Client.GetGrain<IRoomGrain>(context.CurrentRoom);
    await room.Message(new ActionMessage("Larry", messageText));
}