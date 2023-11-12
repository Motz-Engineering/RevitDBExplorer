﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RevitDBExplorer.WPF.Converters
{
    internal class StringTruncateConverter : DependencyObject, IValueConverter
    {
        public int MaxChars
        {
            get => (int)GetValue(MaxCharsProperty);
            set => SetValue(MaxCharsProperty, value);
        }
        public static readonly DependencyProperty MaxCharsProperty = DependencyProperty.Register("MaxChars", typeof(int), typeof(StringTruncateConverter), new PropertyMetadata(0));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            return (value as string).Truncate(MaxChars);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}