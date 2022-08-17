using Orleans;
using StreamQuest.Shared.Enums;

namespace StreamQuest.Shared.Interfaces
{
    public interface IRoomGrain : IGrainWithStringKey
    {
        Task<Guid> Enter(ICharacterGrain character, Direction direction);
        Task<Guid> Exit(ICharacterGrain character, Direction direction);
        Task<bool> Message(ActionMessage msg);
    }
}
