using Orleans;
using StreamQuest.Shared;
using StreamQuest.Shared.Interfaces;

internal readonly record struct ClientContext(
    IClusterClient Client,
    ICharacterGrain? Character = null,
    string? CurrentRoom = null);