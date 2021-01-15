using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Speech.Recognition;
using Newtonsoft.Json.Linq;

namespace VoiceControl
{
    public class ShortcutHandler
    {
        public Dictionary<string, List<string>> ChoiceList { get; private set; }
        public Dictionary<string, Dictionary<string, List<Keyboard.ScanCodeShort>>> Keyevents { get; private set; }


        public void LoadChoices()
        {
            StreamReader Reader = new StreamReader("C:\\Program Files\\VoiceControl\\Shortcuts.txt");
            JObject Json = JObject.Parse(Reader.ReadToEnd());

            Dictionary<object, object> parsed = Json.ToObject<Dictionary<object, object>>();

            ChoiceList = (parsed["ChoiceList"] != null) ? (Dictionary<string, List<string>>)parsed["ChoiceList"] : new Dictionary<string, List<string>>();
            Keyevents = (parsed["KeyEvents"] != null) ? (Dictionary<string, Dictionary<string, List<Keyboard.ScanCodeShort>>>)parsed["KeyEvents"] : new Dictionary<string, Dictionary<string, List<Keyboard.ScanCodeShort>>>();
            
            foreach(KeyValuePair<string, List<string>> _result in ChoiceList)
            {
                MainWindow.AppWindow.Creator.CreateGameTemplate(_result.Key);

                // this is where I left off
                //foreach(string choice in _result.Value)


                //MainWindow.AppWindow.Creator.CreateShortcutTemplate();
            }
        
        }

        /// <summary>
        /// Sets the current stack of choices into the settings file
        /// </summary>
        public void SaveChoices()
        {
            if (!Directory.Exists("C:\\Program Files\\Voice Control"))
                Directory.CreateDirectory("C:\\Program Files\\Voice control");

            StreamWriter Writer = new StreamWriter("C:\\Program Files\\VoiceControl\\Shortcuts.txt");

            Dictionary<string, object> parseToSinglelist = new Dictionary<string, object>()
            {
                { "ChoiceList", ChoiceList },
                { "KeyEvents", Keyevents },
            };
            

            JObject JsonObject = JObject.FromObject(parseToSinglelist);

            string Json = JsonObject.ToString();
            Writer.Write(Json);
        }

        /// <summary>
        /// returns the list of choices going from the given key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public Choices GetChoicesFromKey(string Game)
        {
            Choices _choices = new Choices();

            foreach (string choice in ChoiceList[Game])
                _choices.Add(choice);

            return _choices;
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
        }
    }
}
