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

        //[DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        //public static extern IntPtr FindWindow(string lpClassName,
        //    string lpWindowName);

        //[DllImport("user32.dll")]
        //static extern IntPtr GetForegroundWindow();

        //[DllImport("user32.dll")]
        //public static extern int SetForegroundWindow(IntPtr hWnd);

        //[DllImport("user32.dll", SetLastError = true)]
        //public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [STAThread]
        public static void ConvertToInputKeystrokes(string Text) 
        {
            char[] Characters = Text.ToCharArray();
            string NewString = Text;

            NewString = NewString.Replace('A', '%');
            NewString = NewString.Replace('S', '+');
            NewString = NewString.Replace('C', '^');

            Console.WriteLine(NewString);
            
            SendKeys.SendWait(NewString);
        }

        public static void Write(string Text)
        {
            SendKeys.SendWait(Text);
        }

    }
}
