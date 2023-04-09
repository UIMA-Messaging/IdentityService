namespace ContactService.Contracts
{
    public class User
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string? Image { get; set; }
        public DateTime? JoinedAt { get; set; }
    }
}
