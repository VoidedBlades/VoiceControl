using System.Collections.Generic;
using System.Speech.Recognition;
using System.Windows;
using System;
using System.Threading;

namespace VoiceControl
{
    public partial class VoiceChoices
    {
        public static VoiceChoices AppWindow;

        private SpeechRecognitionEngine recognizer;
        private ShortcutHandler shortcutHandler;

        public void OnStartup()
        {
            AppWindow = this;
            recognizer = new SpeechRecognitionEngine();
            shortcutHandler = MainWindow.AppWindow.shortcutHandler;

            recognizer.SpeechRecognized += speechRecognizer_SpeechRecognized;

            recognizer.SetInputToDefaultAudioDevice();
        }


        private void speechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine(e.Result.Confidence + ":::" + e.Result.Text);
            if(e.Result.Confidence >= .5f && MainWindow.AppWindow.SelectedGame != null)
                if(shortcutHandler.Keyevents[MainWindow.AppWindow.SelectedGame].ContainsKey(e.Result.Text))
                    TextHandler.AppWindow.ConvertToInputKeystrokes(e.Result.Text);
        }

        public void Reconfigure()
        {
            recognizer.RecognizeAsyncStop();
            recognizer.UnloadAllGrammars();

            if (MainWindow.AppWindow.SelectedGame == null || 
                !MainWindow.AppWindow.shortcutHandler.ChoiceList.ContainsKey(MainWindow.AppWindow.SelectedGame) || 
                MainWindow.AppWindow.shortcutHandler.ChoiceList[MainWindow.AppWindow.SelectedGame].Count == 0) return;
            
            Choices _choices = new Choices();
            GrammarBuilder _builder = new GrammarBuilder();

            foreach(string voiceChoice in shortcutHandler.ChoiceList[MainWindow.AppWindow.SelectedGame])
                _choices.Add(voiceChoice);

            _builder.Append(_choices);

            recognizer.LoadGrammar(new Grammar(_builder));
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void Shutdown()
        {
            recognizer.RecognizeAsyncStop();
            recognizer.UnloadAllGrammars();
        }
    }
}
