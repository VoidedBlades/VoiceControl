using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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

        public static void Wait(float TimeLimit)
        {
            Thread.Sleep((int)Math.Round(TimeLimit) * 1000);
        }
    }
}
