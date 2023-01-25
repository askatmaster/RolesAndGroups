namespace Models.Domain;

/// <summary>
/// Базовая доменная сущность
/// </summary>
public class DomainDomainEntityBase : IUniqueDomainEntity
{
    /// <inheritdoc />
    public int Id { get; set; }

    /// <inheritdoc />
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

    /// <inheritdoc />
    public DateTime LastUpdateDateTime { get; set; } = DateTime.UtcNow;

    /// <inheritdoc />
    public DateTime? DeletedDateTime { get; set; }

}