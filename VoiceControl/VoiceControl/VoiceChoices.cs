using System.Collections.Generic;
using System.Speech.Recognition;
using System.Windows;

using System;

namespace VoiceControl
{
    public class VoiceChoices : System.Windows.Application
    {
        private enum State
        {
            KeyStrokes = 0,
            Typing = 1
        }

        public static void _Startup()
        {
           
        }
    }
}
