using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VoiceControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindow;

        private static ListBox ShortcutList_Internal;
        private static ListBox GameList_Internal;
        public ShortcutHandler shortcutHandler;
        public ObjectCreator Creator;

        public string SelectedGame;
        public bool Recording;
        public KeyConverter Key_Converter;
        public Keyboard KeyboardInput;

        public List<Key> KeysPressed;

        public MainWindow()
        {
            InitializeComponent();
            AppWindow = this;

            ShortcutList_Internal = ShortcutList;
            GameList_Internal = GameList;

            shortcutHandler = new ShortcutHandler();
            Creator = new ObjectCreator();

            KeysPressed = new List<Key>();
            Key_Converter = new KeyConverter();

            shortcutHandler.LoadChoices();

            GameName.GotFocus += TextBoxOnFocus;
            KeybindPronounce.GotFocus += TextBoxOnFocus;
        }

        public void UpdateLanguageText(string s)
        {
            LanguageText.Text = s;
        }
        
        public void UpdateSelectedGame(string s)
        {
            if (s == null)
            {
                SelectedGame = null;
                SelectedGameText.Content = "No game selected";
            }
            else
            {
                SelectedGame = s;
                SelectedGameText.Content = s.Replace("1a44d2", " ");
            }
        }

        public void SetGameItems(List<Border> _List)
        {
            GameList_Internal.ItemsSource = null;
            GameList_Internal.ItemsSource = _List;
        }

        public void SetShortcutItems(List<Border> _List)
        {
            ShortcutList_Internal.ItemsSource = null;
            ShortcutList_Internal.ItemsSource = _List;
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            Creator.CreateGameTemplate(GameName.Text);
            GameName.Text = "Game Name";
        }

        private void TextBoxOnFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = e.Source as TextBox;
            textBox.Text = "";
        }

        private void AddKeybind_Click(object sender, RoutedEventArgs e)
        {
            Creator.CreateShortcutTemplate(SelectedGame, KeybindPronounce.Text);

            List<Keyboard.ScanCodeShort> _list = new List<Keyboard.ScanCodeShort>();

            foreach (Key _key in KeysPressed)
                _list.Add((Keyboard.ScanCodeShort)Enum.Parse(typeof(Keyboard.ScanCodeShort), _key.ToString().ToUpper()));

            shortcutHandler.AddChoices(SelectedGame, KeybindPronounce.Text, _list);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Recording && SelectedGame != null && KeysPressed.Count < 4)
            {
                KeyComboDisplay.Content += (string)KeyComboDisplay.Content == "" ? Key_Converter.ConvertToString(e.Key) : " + " + Key_Converter.ConvertToString(e.Key);
                KeysPressed.Add(e.Key);
            }
        }

        private void RecordKey_Click(object sender, RoutedEventArgs e)
        {
            Recording = !Recording;

            if (Recording)
            {
                KeyComboDisplay.Content = "";
                KeysPressed.Clear();
                RecordKey.Content = "Finish Recording";
            }
            else
                RecordKey.Content = "Record Keycombo";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            shortcutHandler.SaveChoices();
        }
    }
}
