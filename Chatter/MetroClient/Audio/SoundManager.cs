using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Chatter.MetroClient.UDP;
using LumiSoft.Media.Wave;

namespace Chatter.MetroClient.Audio
{
   public class SoundManager
    {

       public SoundManager(AudioUtil au)
       {
           this.au = au;
         
       }

        private WaveIn _waveIn;
        private WaveOut _waveOut;

  
     
        private AudioUtil au;

        public void StartRecordAndSend()
        {
            if (_waveIn != null || _waveOut != null)
            {
                throw new Exception("Call is allready started");
            }

            // int waveInDevice = (Int32)Application.UserAppDataRegistry.GetValue("WaveIn", 0);
            //  int waveOutDevice = (Int32)Application.UserAppDataRegistry.GetValue("WaveOut", 0);

            int waveOutDevice = 0;

            _waveIn = new WaveIn(WaveIn.Devices[0], 8000, 16, 1, 400);

            _waveIn.BufferFull += new BufferFullHandler(_waveIn_BufferFull);
            _waveIn.Start();

            _waveOut = new WaveOut(WaveOut.Devices[waveOutDevice], 8000, 16, 1);


           

        }


        

        

        void _waveIn_BufferFull(byte[] buffer)
        {

          byte[] header= Encoding.UTF8.GetBytes("D");

            byte[] data=new byte[buffer.Length+header.Length];
            Array.Copy(header,data,header.Length);
            Array.Copy(buffer,0,data,header.Length,buffer.Length);

            au.Send(data);

           
          
        }

        public void Stop()
        {
            if (_waveIn != null)
            {
                _waveIn.Dispose();
            }

            if (_waveOut != null)
            {
                _waveOut.Dispose();
            }

           

        }

        internal void Play(byte[] buffer)
        {
            _waveOut.Play(buffer, 0, buffer.Length);
        }
    }
}
