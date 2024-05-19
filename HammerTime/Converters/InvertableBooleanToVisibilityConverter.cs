﻿using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace HammerTime.Converters
{
    internal class InvertableBooleanToVisibilityConverter : IValueConverter
    {
        private enum VisibilityParameter
        {
            Normal,
            Inverted
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;
            var direction = (VisibilityParameter)Enum.Parse(typeof(VisibilityParameter), (string)parameter);

            if (direction == VisibilityParameter.Inverted)
                return !boolValue ? Visibility.Visible : Visibility.Collapsed;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}
