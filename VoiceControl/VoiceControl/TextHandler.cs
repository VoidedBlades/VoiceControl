using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace VoiceControl
{

    public partial class TextHandler
    {
        public static TextHandler AppWindow;

        private ShortcutHandler shortcutHandler;
        public void OnStartup()
        {
            AppWindow = this;
            shortcutHandler = MainWindow.AppWindow.shortcutHandler;
        }

        public void ConvertToInputKeystrokes(string Word)
        {
            List<Keyboard.ScanCodeShort> Keys = shortcutHandler.Keyevents[MainWindow.AppWindow.SelectedGame][Word];
            MainWindow.AppWindow.KeyboardInput.Send(Keys);
        }
    }
}
