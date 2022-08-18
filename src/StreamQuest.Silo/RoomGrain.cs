using Orleans;
using Orleans.Streams;
using StreamQuest.Shared;
using StreamQuest.Shared.Enums;
using StreamQuest.Shared.Interfaces;
using System.IO;

namespace StreamQuest.Silo
{
    public class RoomGrain : Grain, IRoomGrain
    {
        private readonly List<ActionMessage> _messages = new(100);
        private readonly List<ICharacterGrain> _characters = new(10);
        private IAsyncStream<ActionMessage> _stream = null!;
        public override Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider("room");

            _stream = streamProvider.GetStream<ActionMessage>(Guid.NewGuid(), "default");

            return base.OnActivateAsync();
        }

        public async Task<Guid> Enter(ICharacterGrain character, Direction direction)
        {
            _characters.Add(character);

            await _stream.OnNextAsync(
                new ActionMessage(
                    "Game",
                    $"{character.Name} enters from the {direction}..."));

            return _stream.Guid;
        }

        public async Task<Guid> Exit(ICharacterGrain character, Direction direction)
        {
            _characters.Remove(character);

            await _stream.OnNextAsync(
                new ActionMessage(
                    "Game",
                    $"{character.Name} leaves to the {direction}..."));

            return _stream.Guid;
        }

        public async Task<bool> Message(ActionMessage msg)
        {
            _messages.Add(msg);

            await _stream.OnNextAsync(msg);

            return true;
        }

        public Task<ActionMessage[]> ReadHistory(int numberOfMessages)
        {
            var response = _messages
                .OrderByDescending(x => x.Created)
                .Take(numberOfMessages)
                .OrderBy(x => x.Created)
                .ToArray();

            return Task.FromResult(response);
        }
    }
}
