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
        public static void ConvertToInputKeystrokes(List<Keyboard.ScanCodeShort> Keys)
        {
            MainWindow.AppWindow.KeyboardInput.Send(Keys);
        }
    }
}
