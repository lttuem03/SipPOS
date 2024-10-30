using Microsoft.UI.Xaml.Data;
using SipPOS.DataTransfer;
using SipPOS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Converters
{
    public class CategoryIdToCategoryNameConverter : IValueConverter
    {
        private readonly ICategoryService _categoryService;

        public CategoryIdToCategoryNameConverter()
        {
            _categoryService = App.GetService<ICategoryService>();
        }

        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is long id)
            {
                CategoryDto categoryDto = _categoryService.GetById(id);
                return categoryDto?.Name;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
