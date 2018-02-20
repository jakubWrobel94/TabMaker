using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabMaker.Models
{
    class Buffer
    {
        private int     size;
        private float[] data        = null;
        private int     position    = 0;

        public int Size { get => size; }
        public float[] Data { get => data; }

        public Buffer(int SIZE)
        {
            size = SIZE;
            data = new float[size];

            for(int i = 0; i < size; ++i)
            {
                Data[i] = -1f;
            }
        }

        public event EventHandler<BufferFilledEventArgs> BufferFilled;
        
        protected virtual void OnBufferFilled(BufferFilledEventArgs e)
        {

            BufferFilled?.Invoke(this, e);
            
        }

        public void AddSample(float SAMPLE_VALUE)
        {
            Data[position] = SAMPLE_VALUE;
            position++;

            if( (position == size/2) && ( data.Last() != -1f ))
            {
                var args = new BufferFilledEventArgs
                {
                    FilledIndex = size / 2
                };

                OnBufferFilled(args);
            }

            if(position == size)
            {
                position = 0;
                var args = new BufferFilledEventArgs
                {
                    FilledIndex = 0
                };

                OnBufferFilled(args);
            }
        }

        
    }

    public class BufferFilledEventArgs : EventArgs
    {
        private int filledIndex;

        public int FilledIndex { get => filledIndex; set => filledIndex = value; }
    }
}
