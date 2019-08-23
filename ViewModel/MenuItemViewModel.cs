using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace YueDroidBox.ViewModel
{
    public class MenuItemViewModel
    {
        public string Name { get; set; }
        public object Content { get; set; }
        public ScrollBarVisibility HorizontalScrollBarVisibilityRequirement { get; set; }
        public ScrollBarVisibility VerticalScrollBarVisibilityRequirement { get; set; }
        public Thickness MarginRequirement { get; set; } = new Thickness(16);

        public event PropertyChangedEventHandler PropertyChanged;

        public MenuItemViewModel(string name, object content)
        {
            Name = name;
            Content = content;
        }

    }
}