using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Helpers
{
    public class Helper : Button
    {
        public static readonly DependencyProperty SomePropProperty =
            DependencyProperty.RegisterAttached(
                "SomeProp",
                typeof(string),
                typeof(Helper),
                new FrameworkPropertyMetadata(null, ChangeCallback));


        public static void SetSomeProp(UIElement element, string value)
        {
            element.SetValue(SomePropProperty, value);
        }
        public static string GetSomeProp(UIElement element)
        {
            return (string)element.GetValue(SomePropProperty);
        }


        public static void ChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as UIElement).MouseLeave += new System.Windows.Input.MouseEventHandler(SomeFunc);
            (d as UIElement).MouseEnter += new System.Windows.Input.MouseEventHandler(SomeFuncEnter);

        }

        private static void SomeFuncEnter(object sender, MouseEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Red;
        }

        private static void SomeFunc(object sender, MouseEventArgs e)
        {
            (sender as Button).Background = Brushes.Green;
            (sender as Button).Foreground = Brushes.Black;
        }

    }
}