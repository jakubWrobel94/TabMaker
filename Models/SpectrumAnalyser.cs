using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using NAudio.Dsp;
namespace TabMaker.Models
{
    /// <summary>
    /// This class calculates spectrum using FFT. 
    /// </summary>
    class SpectrumAnalyser
    {
        private Complex[]   fftInput    = null;
        private Complex[]   fftOutput   = null;

        private Buffer      buffer      = null;
        private Spectrogram spectrogram = null;
        private String      filePath    = null;
        private WaveFormat  waveFormat  = null;
        private int         fftSize;

        public SpectrumAnalyser(int M_POWER)
        {
            fftSize = (int)Math.Pow(2,M_POWER);
            buffer = new Buffer(fftSize);

            fftInput = new Complex[fftSize];
            fftOutput = new Complex[fftSize];
            for(int i = 0; i < fftSize; ++i)
            {
                fftInput[i].X = 0f;
                fftInput[i].Y = 0f;
            }

            buffer.BufferFilled += new EventHandler<BufferFilledEventArgs>(OnBufferFilled);
            
        }

        private void OnBufferFilled(object sender, BufferFilledEventArgs e)
        {
            
            CalculateFFT(e.FilledIndex);
            spectrogram.AddSpectrum(CalculateSpectrum());

        }

        public int FftSize { get => fftSize; }
        public string FilePath { get => filePath; }
        internal Spectrogram Spectrogram { get => spectrogram; }

        public void OpenFile(String FILE_PATH)
        {
            using (AudioFileReader fileReader = new AudioFileReader(FILE_PATH))
            {
                filePath = FILE_PATH;
                waveFormat = fileReader.WaveFormat;
            }

            spectrogram = new Spectrogram(fftSize, waveFormat.SampleRate);
        }

        public void AnalyseFile()
        {
            using (var fileReader = new AudioFileReader(filePath))
            {
                long bytesRecorded = fileReader.Length;
                byte[] fileReaderBuffer = new byte[fftSize*4];
                int bytesReaded;
                
                float sample32;

                do
                {
                    bytesReaded = fileReader.Read(fileReaderBuffer, 0, fileReaderBuffer.Length);
                    for (int i = 0; i < bytesReaded; i += 4)
                    {
                        sample32 = BitConverter.ToSingle(fileReaderBuffer, i);
                        buffer.AddSample(sample32);
                    }
                } while (bytesReaded != 0);
            }
        }

        /// <summary>
        /// Calculates Fourier Transform from data stored in buffor from given index. 
        /// </summary>
        /// <param name="START_IDX"></param>
        
        void CalculateFFT(int START_IDX)
        {
            // Get data from buffer.data to fftInput and apply Hamming Window.
            int bufferPosition = START_IDX;
            for(int i = 0; i < fftSize; ++i)
            {
                fftInput[i].X = buffer.Data[bufferPosition] * (float)FastFourierTransform.HammingWindow(i,fftSize);
                bufferPosition++;
                if (bufferPosition == fftSize) bufferPosition = 0;
            }

            //Copy fftInput to fftOutput and calculate in place FFT.
            fftInput.CopyTo(fftOutput, 0);
            int m = (int)Math.Log(fftSize, 2);
            FastFourierTransform.FFT(true, m, fftOutput);
        }

        /// <summary>
        /// Calculates and returns spectrum from complex data stored in fftOutput.
        /// </summary>
        /// <returns></returns>
        float[] CalculateSpectrum()
        {
            float[] spectrum = new float[fftSize];
            for(int i = 0; i < fftSize; ++i)
            {
                spectrum[i] = (float)Math.Sqrt(fftOutput[i].X * fftOutput[i].X + fftOutput[i].Y * fftOutput[i].Y);
            }
            return spectrum;
        }
    }
}
