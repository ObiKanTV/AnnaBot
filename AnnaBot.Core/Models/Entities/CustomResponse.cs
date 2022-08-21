using AnnaBot.Domain.Models.Entities.Shared;

namespace AnnaBot.Domain.Models.Entities;

public class CustomResponse : EntityBase
{
    public string Key { get; set; }
    public string Value { get; set; }
    public ulong GuildId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

