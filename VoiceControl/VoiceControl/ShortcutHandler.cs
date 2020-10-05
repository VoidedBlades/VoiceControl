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
        private List<Dictionary<string, List<Choices>>> GetChoices()
        {
            if (!Directory.Exists("C:\\Program Files\\Voice Control"))
                Directory.CreateDirectory("C:\\Program Files\\Voice control");

            StreamReader Reader = new StreamReader("C:\\Program Files\\VoiceControl\\Shortcuts.txt");
            JObject Json = JObject.Parse(Reader.ReadToEnd());

            return Json.ToObject<List<Dictionary<string, List<Choices>>>>();
        }

        private void SetChoices()
        {
            if (!Directory.Exists("C:\\Program Files\\Voice Control"))
                Directory.CreateDirectory("C:\\Program Files\\Voice control");

            StreamWriter Writer = new StreamWriter("C:\\Program Files\\VoiceControl\\Shortcuts.txt");
            JObject JsonObject = JObject.FromObject(ChoiceList);

            string Json = JsonObject.ToString();
            Writer.Write(Json);
        }

        public List<Choices> GetChoicesFromKey(string Key)
        {
            if (ChoiceList == null)
                ChoiceList = GetChoices();

            foreach(Dictionary<string, List<Choices>> ChoiceSum in ChoiceList)
                if (ChoiceSum.ContainsKey(Key))
                    return ChoiceSum[Key];

            return null;
        }

        public void SetFromPath(string[] TargetKey)
        {
            
        }

        private object[] RemoveFirstIndexCopy(object[] Array)
        {
            if (Array.Length <= 1) return null;
            object[] TempArray = new object[Array.Length-1];

            for(int i = 1; i < Array.Length-1; i++)
            {
                TempArray[i] = Array[i];
            }

            return TempArray;
        }

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
