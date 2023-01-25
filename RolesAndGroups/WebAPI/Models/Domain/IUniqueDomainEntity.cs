namespace Models.Domain;

/// <summary>
/// Интерфейс доменных сущностей требующих уникальности
/// </summary>
public interface IUniqueDomainEntity
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Дата создания (Устанавливается только при создании обьекта)
    /// </summary>
    public DateTime CreatedDateTime { get; set; }

    /// <summary>
    /// Дата создания (Устанавливается только через репозиторий)
    /// </summary>
    public DateTime LastUpdateDateTime { get; set; }

    /// <summary>
    /// Дата удаления (Устанавливается только через репозиторий)
    /// </summary>
    public DateTime? DeletedDateTime { get; set; }
}