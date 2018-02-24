using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabMaker.Models
{
    /// <summary>
    /// Buffer class that holds data samples from recorded audio for FFT.
    /// </summary>
    class Buffer
    {
        private int     size;
        private float[] data           = null;
        private int     position       = 0;
        private int     overlapFactor  = 8; // buffer overlaps 
        private int[]   overlapPositions = null;

        public int Size { get => size; }
        public float[] Data { get => data; }
        public int OverlapFactor { get => overlapFactor; }

        /// <summary>
        /// Create Buffer on specific size.
        /// </summary>
        /// <param name="SIZE"></param>
        public Buffer(int SIZE)
        {
            size = SIZE;
            data = new float[size];

            for(int i = 0; i < size; ++i)
            {
                Data[i] = -1f;
            }

            overlapPositions = new int[overlapFactor - 1];

            for(int i = 0; i < overlapPositions.Length; ++i)
            {
                overlapPositions[i] = size / overlapFactor * (i + 1);
            }
        }

        /// <summary>
        /// Event raised when buffer is filled or windows overlaps.
        /// </summary>
        public event EventHandler<BufferFilledEventArgs> BufferFilled;
        
        protected virtual void OnBufferFilled(BufferFilledEventArgs e)
        {

            BufferFilled?.Invoke(this, e);
            
        }

        /// <summary>
        /// Add sample to buffer.
        /// </summary>
        /// <param name="SAMPLE_VALUE"></param>
       
        public void AddSample(float SAMPLE_VALUE)
        {
            Data[position] = SAMPLE_VALUE;
            position++;

            if(BufferOverlaps())
            {
                var args = new BufferFilledEventArgs
                {
                    Position = position
                };

                OnBufferFilled(args);
            }

            if(position == size)
            {
                position = 0;
                var args = new BufferFilledEventArgs
                {
                    Position = 0
                };

                OnBufferFilled(args);
            }
        }

        private bool BufferOverlaps()
        {
            if (data.Last() != -1f)
            {
                foreach (var idx in overlapPositions)
                {
                    if (idx == position)
                        return true;
                }
            }
            return false;
        }
    }

    public class BufferFilledEventArgs : EventArgs
    {
        private int position;

        public int Position { get => position; set => position = value; }
    }
}
