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
            string trimmedName = Name.Replace(" ", "1a44d2");

            if (GameUIStorage.ContainsKey(trimmedName)) return;

            Border Border = new Border() { Name = trimmedName, BorderBrush = Brushes.DarkGray, BorderThickness = new Thickness(1,1,1,1), Height=25 };
            Grid Grid = new Grid() {  Height = 24, Width = 170 };

            Button SelectorButton = new Button() {
                Content = Name,
                Name = "Select_" + trimmedName,
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
                Content = "🗑",
                Name = "DeleteButton_" + trimmedName,
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

            GameUIStorage.Add(trimmedName, Border);

            SelectorButton.Click += SelectGame;
            DeleteButton.Click += DeleteObject;

            ConvertAndSetGameElements();
        }
        
        /// <summary>
        /// Creates a new template for a shortcut
        /// </summary>
        /// <param name="Name"></param>
        public void CreateShortcutTemplate(string Game, string Name)
        {

            Name = Name.Trim();
            string TrimmedName = Name.Replace(" ", "1a44d2");

            if (ShortcutUIStorage.ContainsKey(Game) && ShortcutUIStorage[Game].ContainsKey(TrimmedName)) return;

            Border Border = new Border() { Name = TrimmedName, BorderBrush = Brushes.DarkGray, BorderThickness = new Thickness(1, 1, 1, 1), Height = 48, Width = 173 };
            Grid Grid = new Grid() { Height = 48, Width = 170, Margin = new Thickness(0, 0, 0, -2) };

            //string KeyString = "";

            //foreach(System.Windows.Input.Key key in MainWindow.AppWindow.KeysPressed)
            //{
            //    if (KeyString == "")
            //        KeyString = key.ToString();
            //    else
            //        KeyString += " + " + key.ToString();
            //}

            Button RecordedKeys = new Button()
            {
                Content = string.Join(" + ", MainWindow.AppWindow.KeysPressed),
                Name = "Recorded_" + Game + "_" + TrimmedName,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(2, 24, 0, 0),
                Height = 19,
                Width = 142
            };

            Button PronouncedName = new Button()
            {
                Content = Name,
                Name = "Pronounced_" + Game + "_" + TrimmedName,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(2, 2, 0, 0),
                Height = 19,
                Width = 166
            };

            Button DeleteButton = new Button()
            {
                Content = "🗑",
                Name = "DeleteButton_" + Game + "_" + TrimmedName,
                Margin = new Thickness(149, 24, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 19,
                Width = 19
            };


            Grid.Children.Add(RecordedKeys);
            Grid.Children.Add(DeleteButton);
            Grid.Children.Add(PronouncedName);

            Border.Child = Grid;

            DeleteButton.Click += DeleteShortcut;

            if (!ShortcutUIStorage.ContainsKey(Game))
                ShortcutUIStorage.Add(Game, new Dictionary<string, Border>());

            ShortcutUIStorage[Game].Add(TrimmedName, Border);

            ConvertAndSetShortcutElements(Game);
        }

        private void SelectGame(object sender, RoutedEventArgs e)
        {
            Button _button = e.Source as Button;
            string _name = _button.Name.Split('_')[1];

            MainWindow.AppWindow.UpdateSelectedGame(_name);

            ConvertAndSetShortcutElements(_name);
        }

        private void DeleteShortcut(object sender, RoutedEventArgs e)
        {
            Button _button = e.Source as Button;
            string _game = _button.Name.Split('_')[1];
            string _name = _button.Name.Split('_')[2];

            Console.WriteLine(_name);

            GameUIStorage.Remove(_name);

            ShortcutUIStorage[_game].Remove(_name);

            ConvertAndSetShortcutElements(_game);
        }

        private void DeleteObject(object sender, RoutedEventArgs e)
        {
            Button _button = e.Source as Button;
            string _name = _button.Name.Split('_')[1];
            _name = _name.Trim();

            GameUIStorage.Remove(_name);
            ShortcutUIStorage[_name].Clear();

            ConvertAndSetGameElements();

            if (MainWindow.AppWindow.SelectedGame == _name)
                MainWindow.AppWindow.UpdateSelectedGame(null);

            ConvertAndSetShortcutElements(_name);
        }
    }
}
