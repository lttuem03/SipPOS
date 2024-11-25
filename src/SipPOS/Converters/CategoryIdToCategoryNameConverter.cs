using Microsoft.UI.Xaml.Data;

using SipPOS.DataTransfer.Entity;
using SipPOS.Services.Entity.Interfaces;

namespace SipPOS.Converters;

/// <summary>
/// Converts a category ID to a category name using the ICategoryService.
/// </summary>
public class CategoryIdToCategoryNameConverter : IValueConverter
{
    private readonly ICategoryService _categoryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryIdToCategoryNameConverter"/> class.
    /// </summary>
    public CategoryIdToCategoryNameConverter()
    {
        _categoryService = App.GetService<ICategoryService>();
    }

    /// <summary>
    /// Converts a category ID to a category name.
    /// </summary>
    /// <param name="value">The category ID.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="language">The language.</param>
    /// <returns>The category name if found; otherwise, the original value.</returns>
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is long id)
        {
            CategoryDto? categoryDto = _categoryService.GetById(id);
            return categoryDto?.Name;
        }
        return value;
    }

    /// <summary>
    /// Converts a category name back to a category ID. This method is not implemented.
    /// </summary>
    /// <param name="value">The category name.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="language">The language.</param>
    /// <returns>Throws a <see cref="NotImplementedException"/>.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
