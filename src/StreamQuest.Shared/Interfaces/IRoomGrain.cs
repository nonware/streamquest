using Orleans;
using StreamQuest.Shared.Enums;

namespace StreamQuest.Shared.Interfaces
{
    public interface IRoomGrain : IGrainWithStringKey
    {
        Task<Guid> Enter(Character character, Direction from);
        Task<Guid> Exit(Character character, Direction to);
    }
}
