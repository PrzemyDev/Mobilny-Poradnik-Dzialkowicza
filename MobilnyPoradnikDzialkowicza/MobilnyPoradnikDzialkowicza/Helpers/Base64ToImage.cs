using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace MobilnyPoradnikDzialkowicza.Helpers
{
    public class Base64ToImage : IValueConverter
    {
        ImageSource image;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] bytes = System.Convert.FromBase64String(value.ToString());
            image = ImageSource.FromStream(() => new MemoryStream(bytes));
            return image;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
