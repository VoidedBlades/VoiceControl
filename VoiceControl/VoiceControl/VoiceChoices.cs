using System.Collections.Generic;
using System.Speech.Recognition;
using System.Windows;

using System;

namespace VoiceControl
{
    public class VoiceChoices : Application
    {
        private enum State
        {
            KeyStrokes = 0,
            Typing = 1
        }

        private static State CurrentState;
        private static Choices Defaults = new Choices("Enable");

        private static List<string> InputSections;
        private static Dictionary<string, Choices> ReferenceDictionary;
        private static bool IsInput;

        public static Grammar DefaultChoices { get { return new Grammar(new GrammarBuilder(Defaults)); } }

        public static void _Startup()
        {
            ReferenceDictionary = new Dictionary<string, Choices>();
            InputSections = new List<string>();

            ReferenceDictionary.Add("Enable", new Choices("Gaming Mode", "Exit"));
            ReferenceDictionary.Add("Gaming Mode", new Choices("Final Fantasy", "Exit"));
            ReferenceDictionary.Add("Final Fantasy", new Choices(
                "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=",
                "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "A0", "A-", "A=",
                "S1", "S2", "S3", "S4", "A5", "S6", "S7", "S8", "S9", "S0", "S-", "S=",
                "Exit"
                ));

            InputSections.Add("Final Fantasy");
        }

        public static Grammar RetrieveChoices(string Target)
        {
            Console.WriteLine(Target);
            if (Target == "Exit")
            {
                IsInput = false;
                VoiceControl.MainWindow.AppWindow.UpdateModeText("Default");
                return new Grammar(new GrammarBuilder(Defaults));
            }

            if (IsInput)
            {
                TextHandler.ConvertToInputKeystrokes(Target);
                return null;
            }
            else
            {
                if (ReferenceDictionary.ContainsKey(Target))
                {
                    if (Target == "Gaming Mode")
                        CurrentState = State.KeyStrokes;

                    VoiceControl.MainWindow.AppWindow.UpdateModeText(Target);
                    if (InputSections.Exists(x => x == Target))
                        IsInput = true;

                    Console.WriteLine(IsInput);
                    return new Grammar(new GrammarBuilder(ReferenceDictionary[Target]));
                }
                else
                    return null;
            }
        }
    }
}
