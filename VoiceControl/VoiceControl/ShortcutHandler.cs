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
    class ShortcutHandler
    {

        private static List<string> Games;
        private Dictionary<string, Choices> GameShortcuts;

        private List<Dictionary<string, Choices>> ChoiceList;
        private List<Dictionary<Choices, Keyboard.ScanCodeShort>> Keyevents;
        /// <summary>
        /// Retrieve shortcuts from the saved settings file
        /// </summary>
        /// <returns></returns>
        private List<Dictionary<string, Choices>> GetChoices()
        {
            if (!Directory.Exists("C:\\Program Files\\Voice Control"))
                Directory.CreateDirectory("C:\\Program Files\\Voice control");

            StreamReader Reader = new StreamReader("C:\\Program Files\\VoiceControl\\Shortcuts.txt");
            JObject Json = JObject.Parse(Reader.ReadToEnd());

            return Json.ToObject<List<Dictionary<string, Choices>>>();
        }

        /// <summary>
        /// Sets the current stack of choices into the settings file
        /// </summary>
        private void SaveChoices()
        {
            if (!Directory.Exists("C:\\Program Files\\Voice Control"))
                Directory.CreateDirectory("C:\\Program Files\\Voice control");

            StreamWriter Writer = new StreamWriter("C:\\Program Files\\VoiceControl\\Shortcuts.txt");
            JObject JsonObject = JObject.FromObject(ChoiceList);

            string Json = JsonObject.ToString();
            Writer.Write(Json);
        }

        /// <summary>
        /// returns the list of choices going from the given key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public Choices GetChoicesFromKey(string Key)
        {
            if (ChoiceList == null)
                ChoiceList = GetChoices();

            foreach(Dictionary<string, Choices> ChoiceSum in ChoiceList)
                if (ChoiceSum.ContainsKey(Key))
                    return ChoiceSum[Key];

            return null;
        }

        /// <summary>
        /// Sets value in a nested array
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="Value"></param>
        private void AddChoices(string Game, string Value)
        {
            for(int index = 0; index < ChoiceList.Count-1; index++)
            {
                if (ChoiceList[index].ContainsKey(Game))
                {
                    ChoiceList[index][Game].Add(Value);
                    break;
                }
            }
        }
    }
}
