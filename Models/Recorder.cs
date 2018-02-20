using NAudio.Wave;

namespace TabMaker.Models
{
    abstract class Recorder
    {
        
        protected WaveFormat    waveFormat  = null;
        protected WaveIn        audioInput  = null;
        protected int           deviceNum   = 0;

        public Recorder(WaveFormat WAVE_FORMAT)
        {
            this.waveFormat = WAVE_FORMAT;
            
        }
        
        public int DeviceNum { get => deviceNum; set => deviceNum = value; }
        public WaveFormat WaveFormat { get => waveFormat; set => waveFormat = value; }
        public int GetSampleRate() => waveFormat.SampleRate;

        public abstract void StartRecording();
        public abstract void StopRecording();
    }
}
