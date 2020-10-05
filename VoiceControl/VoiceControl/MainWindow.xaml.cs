using System;
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
        public MainWindow()
        {
            InitializeComponent();
            AppWindow = this;
        }

        public void UpdateModeText(string s)
        {
            ModeText.Text = s;
        }

        public void UpdateLanguageText(string s)
        {
            LanguageText.Text = s;

        }

    }
}
