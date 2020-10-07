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

        private List<Dictionary<string, List<Choices>>> ChoiceList;

        /// <summary>
        /// Retrieve shortcuts from the saved settings file
        /// </summary>
        /// <returns></returns>
        private List<Dictionary<string, List<Choices>>> GetChoices()
        {
            if (!Directory.Exists("C:\\Program Files\\Voice Control"))
                Directory.CreateDirectory("C:\\Program Files\\Voice control");

            StreamReader Reader = new StreamReader("C:\\Program Files\\VoiceControl\\Shortcuts.txt");
            JObject Json = JObject.Parse(Reader.ReadToEnd());

            return Json.ToObject<List<Dictionary<string, List<Choices>>>>();
        }

        /// <summary>
        /// Sets the current stack of choices into the settings file
        /// </summary>
        private void SetChoices()
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
        public List<Choices> GetChoicesFromKey(string Key)
        {
            if (ChoiceList == null)
                ChoiceList = GetChoices();

            foreach(Dictionary<string, List<Choices>> ChoiceSum in ChoiceList)
                if (ChoiceSum.ContainsKey(Key))
                    return ChoiceSum[Key];

            return null;
        }

        /// <summary>
        /// Sets value in a nested array
        /// </summary>
        /// <param name="Dict"></param>
        /// <param name="Value"></param>
        /// <param name="Targets"></param>
        /// <param name="T"></param>
        private void SetInNested(object Dict, Choices Value, string Targets, Type T)
        {
            if (T == typeof(Dictionary<string, List<Choices>>))
            {
                Dictionary<string, List<Choices>> TempDict = (Dictionary<string, List<Choices>>)Dict;
                if (TempDict.ContainsKey(Targets))
                {
                    if(Targets != null)
                        SetInNested(TempDict[Targets], Value, Targets, TempDict[Targets].GetType());
                }
            }
            else if (T == typeof(List<Choices>))
            {

            }
        }
    }
}
