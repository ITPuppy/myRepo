using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Chatter.Log;

namespace Chatter.MetroClient.Sound
{
    public class SoundPlayer
    {

        static string fileName = "./sound/water.wav";
       

        [DllImport("winmm.dll")]
        private static extern bool PlaySound(string szSound, IntPtr hMod, PlaySoundFlags flags);

        



        public static void Play()
        {

            try
            {

               
              PlaySound(fileName, IntPtr.Zero, PlaySoundFlags.SND_ASYNC | PlaySoundFlags.SND_FILENAME | PlaySoundFlags.SND_NODEFAULT);
            
              
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("播放声音出现错误",ex);
            }
        }

        private enum PlaySoundFlags
        {
            SND_ALIAS = 0x10000,
            SND_ALIAS_ID = 0x110000,
            SND_ASYNC = 1,
            SND_FILENAME = 0x20000,
            SND_LOOP = 8,
            SND_MEMORY = 4,
            SND_NODEFAULT = 2,
            SND_NOSTOP = 0x10,
            SND_NOWAIT = 0x2000,
            SND_RESOURCE = 0x40004,
            SND_SYNC = 0
        }

    }
}
