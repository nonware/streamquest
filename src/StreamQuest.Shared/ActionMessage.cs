namespace StreamQuest.Shared
{
    [Serializable]
    public record class ActionMessage(string? Issuer, string Text)
    {
        public string Issuer { get; init; } = Issuer ?? "System";
        public string Text { get; init; } = Text ?? "Hello world";
        public DateTimeOffset Created { get; init; } = DateTimeOffset.Now;
    }
}