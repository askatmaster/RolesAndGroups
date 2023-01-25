namespace Models.Dto;

/// <summary>
/// Данные группы
/// </summary>
public class GroupDto
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
    /// Роли
    /// </summary>
    public RoleDto[] Roles { get; set; }

    /// <summary>
    /// Группа выбрана
    /// </summary>
    public bool IsSelected { get; set; }
}