using Orleans;

namespace StreamQuest.Shared.Interfaces
{
    public interface ICharacterGrain : IGrainWithGuidKey
    {
        Task<string?> Name();
        Task SetName(string? name);
    }
}
