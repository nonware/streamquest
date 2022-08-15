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
        private readonly List<Character> _characters = new();
        private IAsyncStream<ActionMessage> _stream = null!;

        public override Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider("app");

            _stream = streamProvider.GetStream<ActionMessage>(Guid.NewGuid(), "default");

            return base.OnActivateAsync();
        }

        public async Task<Guid> Enter(Character character, Direction direction)
        {
            _characters.Add(character);

            await _stream.OnNextAsync(
                new ActionMessage(
                    "Game",
                    $"{character.Name} enters from the {direction}..."));

            return _stream.Guid;
        }

        public async Task<Guid> Exit(Character character, Direction direction)
        {
            _characters.Remove(character);

            await _stream.OnNextAsync(
                new ActionMessage(
                    "Game",
                    $"{character.Name} leaves to the {direction}..."));

            return _stream.Guid;
        }
    }
}
