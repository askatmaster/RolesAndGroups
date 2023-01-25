using System.ComponentModel.DataAnnotations;
namespace Models.Domain;

/// <summary>
/// Доменная модель группы
/// </summary>
public class Group : DomainDomainEntityBase
{
    [MaxLength(150)]
    public string Name { get; set; }

    /// <summary>
    /// Роли
    /// </summary>
    public ICollection<Role> Roles { get; set; }
}