using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceControl
{
    class Await
    {

        public static async Task WaitForMainwindow()
        {
            if (MainWindow.AppWindow == null)
            {
                while (MainWindow.AppWindow == null)
                {
                    await Task.Yield();
                }
            }
        }
    }
}
