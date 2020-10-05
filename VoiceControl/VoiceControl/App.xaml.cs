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

    public partial class App : Application
    {


        public static App AppWindow;
        private static SpeechRecognitionEngine Recognizer;

        private string Directory = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeComponent();
            AppWindow = this;

            VoiceChoices._Startup();

            SetupVoiceControl();
        }

        private List<string> Applications()
        {
            List<string> _Applications = new List<string>();
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(Directory))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        _Applications.Add((string)subkey.GetValue("DisplayName"));
                    }
                }
            }
            return _Applications;
        }

        private async void SetupVoiceControl()
        {
            try
            {
                CultureInfo Culture = new CultureInfo(CultureInfo.CurrentCulture.Name);
                await Await.WaitForMainwindow();

                Recognizer = new SpeechRecognitionEngine(Culture);

                Recognizer.RequestRecognizerUpdate();
                Recognizer.LoadGrammar(VoiceChoices.DefaultChoices);

                //Recognizer.SpeechRecognized += recognizer_SpeechRecognized;
                Recognizer.SpeechHypothesized += recognizer_HypothesizedRecognized;

                Recognizer.SetInputToDefaultAudioDevice();
                Recognizer.RecognizeAsync(RecognizeMode.Multiple);

                VoiceControl.MainWindow.AppWindow.UpdateLanguageText(CultureInfo.CurrentCulture.Name);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.InnerException.Message);
            }
        }

        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //Recognizer.RecognizeAsyncStop();
            Console.WriteLine(e.Result.Text);
            Grammar Results = VoiceChoices.RetrieveChoices(e.Result.Text);
            Recognizer.RequestRecognizerUpdate();
            if (Results != null)
            {
                Recognizer.UnloadAllGrammars();
                Recognizer.LoadGrammar(Results);
            }
            //Recognizer.RecognizeAsync(RecognizeMode.Multiple);

        }

        static void recognizer_HypothesizedRecognized(object sender, SpeechHypothesizedEventArgs e)
        {
            Console.WriteLine(e.Result.Text);
            if (e.Result.Confidence > .6f)
            {
                Grammar Results = VoiceChoices.RetrieveChoices(e.Result.Text);
                Recognizer.RequestRecognizerUpdate();
                if (Results != null)
                {
                    Recognizer.UnloadAllGrammars();
                    Recognizer.LoadGrammar(Results);
                }
            }
        }
    }
}
