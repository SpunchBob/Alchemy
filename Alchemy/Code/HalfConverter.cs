using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Alchemy.Code
{
    public class HalfConverter : IValueConverter
    {
        // Преобразует значение (например, высоту) в половину
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double size)
                return size / 2;

            return 0;
        }

        // Обратное преобразование (не требуется)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
