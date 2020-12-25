using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace VoiceControl
{

    class TextHandler
    {

        [STAThread]
        public static void ConvertToInputKeystrokes(string Text) 
        {
            string[] keys_String = Text.Split(' ');
            List<Keyboard.ScanCodeShort> Keys = new List<Keyboard.ScanCodeShort>();

            for(int i = 0; i < keys_String.Length-1; i++)
            {
                Keyboard.ScanCodeShort key = (Keyboard.ScanCodeShort)System.Enum.Parse(typeof(Keyboard.ScanCodeShort), Text);
                Keys.Add(key);
            }

            MainWindow.AppWindow.KeyboardInput.Send(Keys);

        }
        public static void Write(string Text)
        {
            SendKeys.SendWait(Text);
        }

    }
}
