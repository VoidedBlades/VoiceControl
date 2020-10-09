using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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

        public MainWindow()
        {
            InitializeComponent();
            AppWindow = this;

            ShortcutList_Internal = ShortcutList;
            GameList_Internal = GameList;
        }

        public void UpdateModeText(string s)
        {
            ModeText.Text = s;
        }

        public void UpdateLanguageText(string s)
        {
            LanguageText.Text = s;
        }
        
        public static void SetGameItems(List<Border> _List)
        {
            GameList_Internal.ItemsSource = null;
            GameList_Internal.ItemsSource = _List;
        }

        public static void SetShortcutItems(List<Border> _List)
        {
            ShortcutList_Internal.ItemsSource = null;
            ShortcutList_Internal.ItemsSource = _List;
        }
    }
}
