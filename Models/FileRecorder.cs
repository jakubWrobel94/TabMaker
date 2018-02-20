using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace TabMaker.Models
{
    /// <summary>
    /// Class to record and write audio into WAV file.
    /// </summary>
    /// 
    class FileRecorder : Recorder
    {
        String          filePath = null;
        WaveFileWriter  waveFile = null;

        public FileRecorder(String FILE_PATH, WaveFormat WAVE_FORMAT) : base(WAVE_FORMAT)
        {
            filePath = FILE_PATH;
        }

        public override void StartRecording()
        {
            if(base.audioInput == null)
            {
                base.audioInput = new WaveIn();
                audioInput.DataAvailable += new EventHandler<WaveInEventArgs>(OnDataAvailable);
                base.audioInput.WaveFormat = WaveFormat;
                base.audioInput.DeviceNumber = DeviceNum;
                audioInput.StartRecording();
            }
            
            if(waveFile == null)
            {
                waveFile = new WaveFileWriter(filePath, waveFormat);
            } 
        }


        public override void StopRecording()
        {
            audioInput.StopRecording();
            if (audioInput != null)
            {
                audioInput.Dispose();

                audioInput = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if(waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }
    }
}
