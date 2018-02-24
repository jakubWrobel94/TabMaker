using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TabMaker.Models
{
    /// <summary>
    /// Class which represents spectogram.
    /// </summary>

    class Spectrogram
    {
        private List<float[]>   data = null;
        private float[]         frequencyBins = null;
        private List<float>     timeVector = null;

        private int     fftSize;
        private int     sampleRate;
        private int overlapFactor;
        private float   dt;
        

        public Spectrogram(int FFT_SIZE, int SAMPLE_RATE, int OVERLAP_FACTOR)
        {
            fftSize = FFT_SIZE;
            sampleRate = SAMPLE_RATE;
            overlapFactor = OVERLAP_FACTOR;
            dt = fftSize / (float)(sampleRate * OVERLAP_FACTOR);
            

            data = new List<float[]>();
            frequencyBins = new float[fftSize / 2];
            timeVector = new List<float>();

            float binSize = sampleRate / (float)fftSize;

            for (int i = 0; i < fftSize / 2; ++i)
            {
                frequencyBins[i] = i * binSize;
            }
        }

        public float MaxValue
            {
            get
            {
                List<float> maxVaues = new List<float>();
                foreach(var arr in data)
                {
                    maxVaues.Add(arr.Max());
                }
                return maxVaues.Max();
            }

            }
        public void AddSpectrum(float[] SPECTRUM)
        {
            data.Add(SPECTRUM);

            if(timeVector.Count == 0)
            {
                timeVector.Add(dt);
            }
            else
            {
                timeVector.Add(timeVector.Last() + dt);
            }
        }

        public BitmapSource DrawSpectogram()
        {

            byte[] buffer = new byte[data.Count * frequencyBins.Length]; 
            float scaleFactor = 1/MaxValue;
            for (int j = 0; j < frequencyBins.Length; ++j)
            {
                var currentRow = j;
                for (var i = 0; i < timeVector.Count; ++i)
                {
                    var buffPos = currentRow* timeVector.Count + i;
                    buffer[buffPos] = (byte)(scaleFactor*255*data[i][j]);
                }
                
            }
            var width = timeVector.Count; 
            var height = 128; 
            var dpiX = 96d;
            var dpiY = 96d;
            var pixelFormat = PixelFormats.Gray8; // grayscale bitmap
            var bytesPerPixel = (pixelFormat.BitsPerPixel + 7) / 8; // 
            var stride = bytesPerPixel * width; // == width in this example

            var bitmap = BitmapSource.Create(width, height, dpiX, dpiY,
                                             pixelFormat, null, buffer, stride);
            return bitmap;
        }

        public void SaveToTxtFile(string FILE_PATH)
        {

            if (System.IO.File.Exists(FILE_PATH)) System.IO.File.Delete(FILE_PATH);
            var fileWriter = System.IO.File.AppendText(FILE_PATH);

            foreach (var spectrum in data)
            {
                
                var stringSpectrum = new StringBuilder();

                foreach(var floatVal in spectrum)
                {
                    stringSpectrum.Append(floatVal);
                }

                fileWriter.WriteLine(stringSpectrum);

            }
        }
    }
}
