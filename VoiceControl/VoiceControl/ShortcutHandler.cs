using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Speech.Recognition;
using Newtonsoft.Json.Linq;
using System.Windows.Input;
using Newtonsoft.Json;

namespace VoiceControl
{
    public class ShortcutHandler
    {
        public Dictionary<string, List<string>> ChoiceList { get; private set; } = new Dictionary<string, List<string>>();
        public Dictionary<string, Dictionary<string, List<Keyboard.ScanCodeShort>>> Keyevents { get; private set; } = new Dictionary<string, Dictionary<string, List<Keyboard.ScanCodeShort>>>();
        
        private object Match(string _key)
        {
            foreach(Key inputEnum in Enum.GetValues(typeof(Key)))
            {
                if (inputEnum.ToString().ToLower() == _key.ToLower())
                {
                    Console.WriteLine(inputEnum.ToString().ToLower());
                    return inputEnum;
                }
            }

            return null;
        }

        public void LoadChoices()
        {
            if (!Directory.Exists("C:\\Voice Control"))
                Directory.CreateDirectory("C:\\Voice control");

            if (!File.Exists("C:\\Voice Control\\Shortcuts.txt"))
                File.Create("C:\\Voice Control\\Shortcuts.txt");
            else
            {
                string JSON = File.ReadAllText("C:\\Voice Control\\Shortcuts.txt");

                if (!string.IsNullOrEmpty(JSON) || string.IsNullOrWhiteSpace(JSON))
                {
                    Dictionary<object, object> parsed = JsonConvert.DeserializeObject<Dictionary<object, object>>(JSON);

                    Dictionary<string, List<string>> _ChoiceList = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(parsed["ChoiceList"].ToString());
                    Dictionary<string, Dictionary<string, List<Keyboard.ScanCodeShort>>> _KeyEvents = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<Keyboard.ScanCodeShort>>>>(parsed["KeyEvents"].ToString());

                    ChoiceList = (_ChoiceList.Count != 0) ? _ChoiceList : new Dictionary<string, List<string>>();
                    Keyevents = (_KeyEvents.Count != 0) ? _KeyEvents : new Dictionary<string, Dictionary<string, List<Keyboard.ScanCodeShort>>>();

                    foreach (KeyValuePair<string, List<string>> _result in ChoiceList)
                    {
                        MainWindow.AppWindow.Creator.CreateGameTemplate(_result.Key);

                        MainWindow.AppWindow.KeysPressed = new List<Key>();
                        foreach (string choice in _result.Value)
                        {
                            MainWindow.AppWindow.KeysPressed.Clear();
                            foreach (Keyboard.ScanCodeShort key in Keyevents[_result.Key][choice])
                            {
                                object _match = Match(key.ToString());
                                if(_match != null)
                                    MainWindow.AppWindow.KeysPressed.Add((Key)_match);
                            }

                            MainWindow.AppWindow.Creator.CreateShortcutTemplate(_result.Key, choice);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the current stack of choices into the settings file
        /// </summary>
        public void SaveChoices()
        {
            if (!Directory.Exists("C:\\Voice Control"))
                Directory.CreateDirectory("C:\\Voice control");

            if (!File.Exists("C:\\Voice Control\\Shortcuts.txt"))
                File.Create("C:\\Voice Control\\Shortcuts.txt");

            Dictionary<string, object> parseToSinglelist = new Dictionary<string, object>()
            {
                { "ChoiceList", ChoiceList },
                { "KeyEvents", Keyevents },
            };

            JObject JsonObject = JObject.FromObject(parseToSinglelist);

            StreamWriter Writer = new StreamWriter("C:\\Voice Control\\Shortcuts.txt");
            Writer.Write(JsonObject.ToString());
            Writer.Close();
        }

        /// <summary>
        /// Sets value in a nested array
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="Value"></param>
        public void AddChoices(string Game, string Value, List<Keyboard.ScanCodeShort> keycode)
        {
            if (!ChoiceList.ContainsKey(Game))
                ChoiceList.Add(Game, new List<string>());

            ChoiceList[Game].Add(Value);
            if (!Keyevents.ContainsKey(Game))
                Keyevents.Add(Game, new Dictionary<string, List<Keyboard.ScanCodeShort>>());

            Keyevents[Game].Add(Value, keycode);
            VoiceChoices.AppWindow.Reconfigure();
        }

        public void RemoveGame(string game)
        {
            if (ChoiceList.ContainsKey(game))
                ChoiceList.Remove(game);

            if (Keyevents.ContainsKey(game))
                Keyevents.Remove(game);
        }

        public void RemoveChoice(string game, string choice)
        {
            if (ChoiceList.ContainsKey(game))
                if (ChoiceList[game].Contains(choice))
                    ChoiceList[game].Remove(choice);

            if (Keyevents.ContainsKey(game))
                if (Keyevents[game].ContainsKey(choice))
                    Keyevents[game].Remove(choice);
        }
    }
}
