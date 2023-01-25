using System.ComponentModel.DataAnnotations;
namespace Models.Domain;

/// <summary>
/// Доменная модель роли
/// </summary>
public class Role : DomainDomainEntityBase
{
    [MaxLength(150)]
    public string Name { get; set; }

    /// <summary>
    /// Роли
    /// </summary>
    public ICollection<Group> Groups { get; set; }
}