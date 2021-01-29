using System;
using System.Collections.Generic;
using System.Windows;

using System.Speech.Recognition;
using System.Speech.Synthesis;

using System.Globalization;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace VoiceControl
{

    public partial class App : System.Windows.Application
    {


        public static App AppWindow;
        private static SpeechRecognitionEngine Recognizer;

        //private string Directory = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeComponent();
            AppWindow = this;
        }
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string results = e.Result.Text;
            float confidence = e.Result.Confidence;
        }

        static void recognizer_HypothesizedRecognized(object sender, SpeechHypothesizedEventArgs e)
        {
            string results = e.Result.Text;
            float confidence = e.Result.Confidence;
        }
    }
}
