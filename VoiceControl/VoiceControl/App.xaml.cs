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
        private enum State
        {
            Writing = 0,
            KeyStroke = 1,
            Application = 2
        }


        public static App AppWindow;
        private static SpeechRecognitionEngine Recognizer;
        private static bool HasProcessed = false;

        private string Directory = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        private State CurrentState = State.Writing;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeComponent();
            AppWindow = this;

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
                GrammarBuilder builder = new GrammarBuilder();
                Choices ChoiceList = new Choices("test");


                builder.Append(ChoiceList);


                Recognizer = new SpeechRecognitionEngine(Culture);

                Recognizer.RequestRecognizerUpdate();
                Recognizer.LoadGrammar(new Grammar(builder));

                Recognizer.SpeechRecognized += recognizer_SpeechRecognized;
                //Recognizer.SpeechHypothesized += recognizer_HypothesizedRecognized;
                Recognizer.RecognizeCompleted += recognizer_Competed;

                Recognizer.SetInputToDefaultAudioDevice();
                Recognizer.RecognizeAsync(RecognizeMode.Multiple);

                await Await.WaitForMainwindow();

                VoiceControl.MainWindow.AppWindow.UpdateLanguageText(CultureInfo.CurrentCulture.Name);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.InnerException.Message);
            }
        }

        static async void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Await.Wait(1);
            Console.WriteLine(e.Result.Text);
        }

        static async void recognizer_HypothesizedRecognized(object sender, SpeechHypothesizedEventArgs e)
        {
            if (HasProcessed) return;
            HasProcessed = true;
            Console.WriteLine(e.Result.Text);
        }

        static void recognizer_Competed(object sender, RecognizeCompletedEventArgs e)
        {
            Console.WriteLine(e.Result.Text);
        }
    }
}
