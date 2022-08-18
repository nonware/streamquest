using Orleans.Streams;
using Spectre.Console;
using StreamQuest.Shared;

namespace StreamQuest.Client
{
    public class StreamObserver : IAsyncObserver<ActionMessage>
    {
        private readonly string _roomName;

        public StreamObserver(string roomName) => _roomName = roomName;

        public Task OnCompletedAsync() => Task.CompletedTask;

        public Task OnErrorAsync(Exception ex)
        {
            AnsiConsole.WriteException(ex);

            return Task.CompletedTask;
        }

        public Task OnNextAsync(ActionMessage item, StreamSequenceToken? token = null)
        {
            AnsiConsole.MarkupLine(
                "[[[dim]{0}[/]]][[{1}]] [bold yellow]{2}:[/] {3}",
                item.Created.LocalDateTime, _roomName, item.Text, item.Text);

            return Task.CompletedTask;
        }
    }
}
