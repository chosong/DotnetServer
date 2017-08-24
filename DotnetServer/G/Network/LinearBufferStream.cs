using System;
using System.IO;

namespace G.Network
{
	public class LinearBufferStream : Stream
	{
		private byte[] buffer;
		
		private int capacity;
		public int Capacity { get { return capacity; } }
		
		private int readable;
		public int Readable { get { return readable; } }
		
		public int Writable { get { return capacity - offsetW; } }
		
		private int offsetR;
		private int offsetW;
		
		public LinearBufferStream(int capacity)
		{
			this.capacity = capacity;
			buffer = new byte[capacity];
			readable = 0;
			offsetR = 0;
			offsetW = 0;
		}
		
		public int Peek(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
				throw new ArgumentNullException();
			
			if (offset < 0 || count < 0)
				throw new ArgumentOutOfRangeException();
			
			if (offset + count > buffer.Length)
				throw new ArgumentException();
			
			if (count > readable)
				count = readable;
			
			Array.Copy(this.buffer, offsetR, buffer, offset, count);
			
			return count;
		}
		
		public override int Read(byte[] buffer, int offset, int count)
		{
			count = Peek(buffer, offset, count);
			
			offsetR += count;
			readable -= count;
			
			return count;
		}
		
		public override void Write(byte[] buffer, int offset, int count)
		{
            if (buffer == null)
                throw new ArgumentNullException();

            if (offset < 0 || count < 0)
                throw new ArgumentOutOfRangeException();

            if (offset + count > buffer.Length)
                throw new ArgumentException();

            if (count > Writable)
                throw new ArgumentException();
			
			Array.Copy(buffer, offset, this.buffer, offsetW, count);
			
			offsetW += count;
			readable += count;
		}
		
        public override bool CanRead { get { return true; } }

        public override bool CanWrite { get { return true; } }

        public override bool CanSeek { get { return false; } }

        public override void Flush()
        {
			Reset();
        }

		public override long Length
        {
            get { throw new NotSupportedException(); }
        }
		
        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
		
		public ArraySegment<byte> ReadableArraySegment
		{
			get
			{
				return new ArraySegment<byte>(buffer, offsetR, readable);
			}
		}
		
		public bool OnRead(int count)
		{
			if (count > readable) return false;
			
			readable -= count;
			if (readable == 0)
			{
				offsetR = 0;
				offsetW = 0;
			}
			else
			{
				offsetR += count;
			}
			
			return true;
		}
		
		public ArraySegment<byte> WritableArraySegment
		{
			get
			{
				int writable = this.Writable;
				
				if (writable <= 0)
				{
					return default(ArraySegment<byte>);
				}
				
				return new ArraySegment<byte>(buffer, offsetW, writable);
			}
		}
		
		public bool OnWrite(int count)
		{
			if (count > Writable) return false;
			offsetW += count;
			readable += count;
			return true;
		}
		
		public void Reset()
		{
			readable = 0;
			offsetR = 0;
			offsetW = 0;
		}
		
		public void Optimize()
		{
			if (readable > 0 && offsetR > 0)
			{
				Array.Copy(buffer, offsetR, buffer, 0, readable);
				offsetR = 0;
				offsetW = readable;
			}
		}
		
        #region State
        public int backupReadable = -1;
        public int backupOffsetR = -1;
        public int backupOffsetW = -1;

        public void Save()
        {
            backupReadable = readable;
            backupOffsetR = offsetR;
            backupOffsetW = offsetW;
        }

        public void Restore()
        {
            readable = backupReadable;
            offsetR = backupOffsetR;
            offsetW = backupOffsetW;
        }
        #endregion
	}
}
