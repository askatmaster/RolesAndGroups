namespace Infrastructure.Helpers;

/// <summary>
/// Хелпер для маппинга сущностей
/// </summary>
public static class EntityMapHelper
{
    /// <summary>
    /// Маппинг сущностей
    /// </summary>
    public static void MapSourceToEntity<TSource, TEntity>(TSource source, ref TEntity destination)
        where TSource : class
        where TEntity : class
    {
        var typeOfSource = source.GetType();
        var typeOfDestination = destination.GetType();

        foreach (var propertyOfSource in typeOfSource.GetProperties())
        {
            var propertyOfDestination = typeOfDestination.GetProperty(propertyOfSource.Name);

            if (propertyOfDestination != null && propertyOfDestination.CanWrite && propertyOfDestination.PropertyType.IsAssignableFrom(propertyOfSource.PropertyType))
                try
                {
                    propertyOfDestination.SetValue(destination, propertyOfSource.GetValue(source));
                }
                catch (ArgumentException) { }
        }
    }
}