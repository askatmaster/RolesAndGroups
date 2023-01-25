namespace Models.Dto;

/// <summary>
/// Данные роли
/// </summary>
public class RoleDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime? CreatedDateTime { get; set; }

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime? LastUpdateDateTime { get; set; }

    /// <summary>
    /// Дата удаления
    /// </summary>
    public DateTime? DeletedDateTime { get; set; }

    /// <summary>
    /// Группы
    /// </summary>
    public GroupDto[] Groups { get; set; }

    /// <summary>
    /// Роль выбрана
    /// </summary>
    public bool IsSelected { get; set; }
}