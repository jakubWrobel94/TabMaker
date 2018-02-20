using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabMaker.Models
{
    /// <summary>
    /// Class which represents spectogram.
    /// </summary>
    
    class Spectrogram
    {
        private List<float[]>   data            = null;
        private float[]         frequencyBins   = null;
        private List<float>     timeVector      = null;

        private int     fftSize;
        private int     sampleRate;
        private float   dt;

        public Spectrogram(int FFT_SIZE, int SAMPLE_RATE)
        {
            fftSize = FFT_SIZE;
            sampleRate = SAMPLE_RATE;
            dt = fftSize / (float)(sampleRate * 2);

            data = new List<float[]>();
            frequencyBins = new float[fftSize / 2];
            timeVector = new List<float>();

            float binSize = sampleRate / (float)fftSize;

            for(int i = 0; i < fftSize/2; ++i)
            {
                frequencyBins[i] = i * binSize;
            }
        }

        public void AddSpectrum(float[] SPECTRUM)
        {
            data.Add(SPECTRUM);

            if(timeVector.Count == 0)
            {
                timeVector.Add(0f);
            }
            else
            {
                timeVector.Add(timeVector.Last() + dt);
            }
        }
    }
}
