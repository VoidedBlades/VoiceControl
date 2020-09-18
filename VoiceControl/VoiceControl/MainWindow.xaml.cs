using System;
using System.Windows;

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
            
            Console.WriteLine("applied");
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
