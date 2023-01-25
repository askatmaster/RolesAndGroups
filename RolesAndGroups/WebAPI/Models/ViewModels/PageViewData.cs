namespace Models.ViewModels;

/// <summary>
/// Представление пагинации
/// </summary>
/// <typeparam name="T">Тип содержимых данных</typeparam>
public class PageViewData<T>
{
    /// <summary>
    /// номер текущей страницы
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// кол-во объектов на странице
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// всего объектов
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// всего страниц
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);

    /// <summary>
    /// Данные
    /// </summary>
    public IEnumerable<T> Items { get; set; }
}