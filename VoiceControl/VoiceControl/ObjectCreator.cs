using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

using System.Speech.Recognition;

namespace VoiceControl
{
    class ObjectCreator
    {

        public static Dictionary<string, Dictionary<string, Border>> UIStorage;
        public static Dictionary<string, Dictionary<string, List<Choices>>> ChoiceStorage;

        public void CreateGameTemplate(string Name, string Key)
        {
            Border Border = new Border() { BorderBrush = Brushes.DarkGray, BorderThickness = new Thickness(1,1,1,1), Height=25 };
            Grid Grid = new Grid() {  Height = 24, Width = 170 };

            Button SelectorButton = new Button() {
                Content = Name,
                Name = Name,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(2,2,0,0),
                Height = 19,
                Width = 121
            };

            Button DeleteButton = new Button()
            {
                Content = "🗑️",
                Name = "DeleteButton_" + Name,
                Margin = new Thickness(149, 2, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Height = 19,
                Width = 19
            };

            Button EditButton = new Button()
            {
                Content = "✎",
                Name = "Edit_" + Name,
                Margin = new Thickness(2, 2, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Height = 19,
                Width = 19
            };


            Grid.Children.Add(SelectorButton);
            Grid.Children.Add(DeleteButton);
            Grid.Children.Add(SelectorButton);

            Border.Child = Grid;
        }
    }
}
