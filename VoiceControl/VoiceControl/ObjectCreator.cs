﻿using System;
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

        public static Dictionary<string, Border> GameUIStorage = new Dictionary<string, Border>();
        public static Dictionary<string, Dictionary<string, Border>> ShortcutUIStorage = new Dictionary<string, Dictionary<string, Border>>();

        /// <summary>
        /// wipes the UI data of the given key
        /// </summary>
        /// <param name="Key"></param>
        public void WhipeDataFromKey(string Key)
        {
            GameUIStorage.Remove(Key);
            ShortcutUIStorage.Remove(Key);

            ConvertAndSetGameElements();
            ConvertAndSetShortcutElements(Key);
        }

        /// <summary>
        /// Converts the dictionary to a list in order to implement it into the wpf
        /// </summary>
        private void ConvertAndSetGameElements()
        {
            List<Border> ListedElements = new List<Border>();

            foreach (KeyValuePair<string, Border> Element in GameUIStorage)
            {
                ListedElements.Add(Element.Value);
            }

            MainWindow.AppWindow.SetGameItems(ListedElements);
        }

        /// <summary>
        /// Converts the dictionary to a list in order to implement it into the wpf
        /// </summary>
        /// <param name="Key"></param>
        private void ConvertAndSetShortcutElements(string Key)
        {
            List<Border> ListedElements = new List<Border>();

            foreach (KeyValuePair<string, Dictionary<string, Border>> Element in ShortcutUIStorage)
                if (Element.Key == Key)
                {
                    foreach (KeyValuePair<string, Border> ShortcutElement in Element.Value)
                        ListedElements.Add(ShortcutElement.Value);

                    break;
                }

            MainWindow.AppWindow.SetShortcutItems(ListedElements);
        }

        /// <summary>
        /// Creates the template for a new game that got added
        /// </summary>
        /// <param name="Name"></param>
        public void CreateGameTemplate(string Name)
        {
            if (GameUIStorage.ContainsKey(Name)) return;

            Name = Name.Replace(" ", "");
            Border Border = new Border() { Name = Name, BorderBrush = Brushes.DarkGray, BorderThickness = new Thickness(1,1,1,1), Height=25 };
            Grid Grid = new Grid() {  Height = 24, Width = 170 };

            Button SelectorButton = new Button() {
                Content = Name,
                Name = "Select_" + Name,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(2,2,0,0),
                Height = 19,
                Width = 142
            };

            Button DeleteButton = new Button()
            {
                Content = "🗑️",
                Name = "DeleteButton_" + Name,
                Margin = new Thickness(149, 2, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 19,
                Width = 19
            };

            //Button EditButton = new Button()
            //{
            //    Content = "✎",
            //    Name = "Edit_" + Name,
            //    Margin = new Thickness(2, 2, 0, 0),
            //    VerticalContentAlignment = VerticalAlignment.Center,
            //    HorizontalContentAlignment = HorizontalAlignment.Center,
            //    Height = 19,
            //    Width = 19
            //};

            Grid.Children.Add(SelectorButton);
            Grid.Children.Add(DeleteButton);
            //Grid.Children.Add(EditButton);

            Border.Child = Grid;

            GameUIStorage.Add(Name, Border);

            DeleteButton.Click += DeleteObject;

            ConvertAndSetGameElements();
        }
        
        /// <summary>
        /// Creates a new template for a shortcut
        /// </summary>
        /// <param name="Name"></param>
        public void CreateShortcutTemplate(string Game, string Name, string Keys)
        {
            Border Border = new Border() { BorderBrush = Brushes.DarkGray, BorderThickness = new Thickness(1, 1, 1, 1), Height = 48 };
            Grid Grid = new Grid() { Height = 48, Width = 170 };

            Label RecordedKeys = new Label()
            {
                Content = Name,
                Name = "Record_" + Name,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(2, 24, 0, 0),
                Height = 19,
                Width = 121
            };

            Label PronouncedName = new Label()
            {
                Content = Name,
                Name = "Pronounced_" + Name,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(2, 2, 0, 0),
                Height = 19,
                Width = 166
            };

            Button DeleteButton = new Button()
            {
                Content = "🗑️",
                Name = "DeleteButton_" + Name,
                Margin = new Thickness(149, 24, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Height = 19,
                Width = 19
            };

            Button EditButton = new Button()
            {
                Content = "✎",
                Name = "Edit_" + Name,
                Margin = new Thickness(126, 24, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Height = 19,
                Width = 19
            };


            Grid.Children.Add(RecordedKeys);
            Grid.Children.Add(DeleteButton);
            Grid.Children.Add(EditButton);
            Grid.Children.Add(PronouncedName);

            Border.Child = Grid;

            if (!ShortcutUIStorage.ContainsKey(Game))
                ShortcutUIStorage.Add(Game, new Dictionary<string, Border>());

            ShortcutUIStorage[Game].Add(Name, Border);

            ConvertAndSetShortcutElements(Game);
        }

        private void DeleteObject(object sender, RoutedEventArgs e)
        {
            Button _button = e.Source as Button;
            string _name = _button.Name.Split('_')[1];

            GameUIStorage.Remove(_name);

            ConvertAndSetGameElements();
        }
    }
}
