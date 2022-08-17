using Orleans;
using Orleans.Concurrency;
using StreamQuest.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamQuest.Shared
{
    [Immutable]
    public record class PlayerInfo(Guid Key, string? Name);

    public class PlayerCharacter : Grain, ICharacterGrain
    {
        private PlayerInfo _myInfo = null!;

        public override Task OnActivateAsync()
        {
            _myInfo = new(this.GetPrimaryKey(), "larry");
            return base.OnActivateAsync();
        }

        public Task<string?> Name() => Task.FromResult(_myInfo?.Name);

        public Task SetName(string? name)
        {
            _myInfo = _myInfo with { Name = name };
            return Task.CompletedTask;
        }
    }
}
