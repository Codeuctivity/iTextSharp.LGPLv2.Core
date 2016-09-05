using System;
using System.IO;
using System.Text;
using iTextSharp.LGPLv2.Core.System.NetUtils;

namespace iTextSharp.text.pdf
{
    /// <summary>
    /// An implementation of a RandomAccessFile for input only
    /// that accepts a file or a byte array as data source.
    /// @author Paulo Soares (psoares@consiste.pt)
    /// </summary>
    public class RandomAccessFileOrArray
    {

        internal byte[] ArrayIn;
        internal int ArrayInPtr;
        internal byte Back;
        internal string Filename;
        internal bool IsBack;
        internal FileStream Rf;

        /// <summary>
        /// Holds value of property startOffset.
        /// </summary>
        private int _startOffset;

        public RandomAccessFileOrArray(string filename) : this(filename, false)
        {
        }

        public RandomAccessFileOrArray(string filename, bool forceRead)
        {
            var cachedBytes = PdfResourceFileCache.Get(filename);
            if (cachedBytes != null)
            {
                ArrayIn = cachedBytes;
                return;
            }

            if (!File.Exists(filename))
            {
                if (filename.StartsWith("file:/", StringComparison.OrdinalIgnoreCase) ||
                    filename.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                    filename.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    Stream isp = filename.GetResponseStream();
                    try
                    {
                        ArrayIn = InputStreamToArray(isp);
                        return;
                    }
                    finally
                    {
                        try { isp.Dispose(); } catch { }
                    }
                }
                else
                {
                    Stream isp = BaseFont.GetResourceStream(filename);
                    if (isp == null)
                        throw new IOException(filename + " not found as file or resource.");
                    try
                    {
                        ArrayIn = InputStreamToArray(isp);
                        return;
                    }
                    finally
                    {
                        try { isp.Dispose(); } catch { }
                    }
                }
            }
            else if (forceRead)
            {
                Stream s = null;
                try
                {
                    s = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                    ArrayIn = InputStreamToArray(s);
                }
                finally
                {
                    try { if (s != null) s.Dispose(); } catch { }
                }
                return;
            }
            Filename = filename;
            Rf = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public RandomAccessFileOrArray(Uri url)
        {
            Stream isp = url.GetResponseStream();
            try
            {
                ArrayIn = InputStreamToArray(isp);
            }
            finally
            {
                try { isp.Dispose(); } catch { }
            }
        }

        public RandomAccessFileOrArray(Stream isp)
        {
            ArrayIn = InputStreamToArray(isp);
        }

        public RandomAccessFileOrArray(byte[] arrayIn)
        {
            ArrayIn = arrayIn;
        }

        public RandomAccessFileOrArray(RandomAccessFileOrArray file)
        {
            Filename = file.Filename;
            ArrayIn = file.ArrayIn;
            _startOffset = file._startOffset;
        }

        public int FilePointer
        {
            get
            {
                InsureOpen();
                int n = IsBack ? 1 : 0;
                if (ArrayIn == null)
                {
                    return (int)Rf.Position - n - _startOffset;
                }
                else
                    return ArrayInPtr - n - _startOffset;
            }
        }

        public int Length
        {
            get
            {
                if (ArrayIn == null)
                {
                    InsureOpen();
                    return (int)Rf.Length - _startOffset;
                }
                else
                    return ArrayIn.Length - _startOffset;
            }
        }

        public int StartOffset
        {
            get
            {
                return _startOffset;
            }
            set
            {
                _startOffset = value;
            }
        }

        public static byte[] InputStreamToArray(Stream isp)
        {
            byte[] b = new byte[8192];
            MemoryStream outp = new MemoryStream();
            while (true)
            {
                int read = isp.Read(b, 0, b.Length);
                if (read < 1)
                    break;
                outp.Write(b, 0, read);
            }
            return outp.ToArray();
        }
        public void Close()
        {
            IsBack = false;
            if (Rf != null)
            {
                Rf.Dispose();
                Rf = null;
            }
        }

        public bool IsOpen()
        {
            return (Filename == null || Rf != null);
        }

        public void PushBack(byte b)
        {
            Back = b;
            IsBack = true;
        }

        public int Read()
        {
            if (IsBack)
            {
                IsBack = false;
                return Back & 0xff;
            }
            if (ArrayIn == null)
                return Rf.ReadByte();
            else
            {
                if (ArrayInPtr >= ArrayIn.Length)
                    return -1;
                return ArrayIn[ArrayInPtr++] & 0xff;
            }
        }

        public int Read(byte[] b, int off, int len)
        {
            if (len == 0)
                return 0;
            int n = 0;
            if (IsBack)
            {
                IsBack = false;
                if (len == 1)
                {
                    b[off] = Back;
                    return 1;
                }
                else
                {
                    n = 1;
                    b[off++] = Back;
                    --len;
                }
            }
            if (ArrayIn == null)
            {
                return Rf.Read(b, off, len) + n;
            }
            else
            {
                if (ArrayInPtr >= ArrayIn.Length)
                    return -1;
                if (ArrayInPtr + len > ArrayIn.Length)
                    len = ArrayIn.Length - ArrayInPtr;
                Array.Copy(ArrayIn, ArrayInPtr, b, off, len);
                ArrayInPtr += len;
                return len + n;
            }
        }

        public int Read(byte[] b)
        {
            return Read(b, 0, b.Length);
        }

        public bool ReadBoolean()
        {
            int ch = Read();
            if (ch < 0)
                throw new EndOfStreamException();
            return (ch != 0);
        }

        public byte ReadByte()
        {
            int ch = Read();
            if (ch < 0)
                throw new EndOfStreamException();
            return (byte)(ch);
        }

        public char ReadChar()
        {
            int ch1 = Read();
            int ch2 = Read();
            if ((ch1 | ch2) < 0)
                throw new EndOfStreamException();
            return (char)((ch1 << 8) + ch2);
        }

        /// <summary>
        /// Reads a Unicode character from this stream in little-endian order.
        /// This method reads two
        /// bytes from the stream, starting at the current stream pointer.
        /// This method blocks until the two bytes are read, the end of the
        /// stream is detected, or an exception is thrown.
        /// @exception  EOFException  if this stream reaches the end before reading
        /// two bytes.
        /// @exception  IOException   if an I/O error occurs.
        /// </summary>
        /// <returns>the next two bytes of this stream as a Unicode character.</returns>
        public char ReadCharLe()
        {
            int ch1 = Read();
            int ch2 = Read();
            if ((ch1 | ch2) < 0)
                throw new EndOfStreamException();
            return (char)((ch2 << 8) + (ch1 << 0));
        }

        public double ReadDouble()
        {
            long[] a = { ReadLong() };
            double[] b = { 0 };
            Buffer.BlockCopy(a, 0, b, 0, 8);
            return b[0];
        }

        public double ReadDoubleLe()
        {
            long[] a = { ReadLongLe() };
            double[] b = { 0 };
            Buffer.BlockCopy(a, 0, b, 0, 8);
            return b[0];
        }

        public float ReadFloat()
        {
            int[] a = { ReadInt() };
            float[] b = { 0 };
            Buffer.BlockCopy(a, 0, b, 0, 4);
            return b[0];
        }

        public float ReadFloatLe()
        {
            int[] a = { ReadIntLe() };
            float[] b = { 0 };
            Buffer.BlockCopy(a, 0, b, 0, 4);
            return b[0];
        }

        public void ReadFully(byte[] b)
        {
            ReadFully(b, 0, b.Length);
        }

        public void ReadFully(byte[] b, int off, int len)
        {
            if (len == 0)
                return;
            int n = 0;
            do
            {
                int count = Read(b, off + n, len - n);
                if (count <= 0)
                    throw new EndOfStreamException();
                n += count;
            } while (n < len);
        }

        public int ReadInt()
        {
            int ch1 = Read();
            int ch2 = Read();
            int ch3 = Read();
            int ch4 = Read();
            if ((ch1 | ch2 | ch3 | ch4) < 0)
                throw new EndOfStreamException();
            return ((ch1 << 24) + (ch2 << 16) + (ch3 << 8) + ch4);
        }

        /// <summary>
        /// Reads a signed 32-bit integer from this stream in little-endian order.
        /// This method reads 4
        /// bytes from the stream, starting at the current stream pointer.
        /// This method blocks until the four bytes are read, the end of the
        /// stream is detected, or an exception is thrown.
        ///  int .
        /// @exception  EOFException  if this stream reaches the end before reading
        /// four bytes.
        /// @exception  IOException   if an I/O error occurs.
        /// </summary>
        /// <returns>the next four bytes of this stream, interpreted as an</returns>
        public int ReadIntLe()
        {
            int ch1 = Read();
            int ch2 = Read();
            int ch3 = Read();
            int ch4 = Read();
            if ((ch1 | ch2 | ch3 | ch4) < 0)
                throw new EndOfStreamException();
            return ((ch4 << 24) + (ch3 << 16) + (ch2 << 8) + (ch1 << 0));
        }

        public string ReadLine()
        {
            StringBuilder input = new StringBuilder();
            int c = -1;
            bool eol = false;

            while (!eol)
            {
                switch (c = Read())
                {
                    case -1:
                    case '\n':
                        eol = true;
                        break;
                    case '\r':
                        eol = true;
                        int cur = FilePointer;
                        if ((Read()) != '\n')
                        {
                            Seek(cur);
                        }
                        break;
                    default:
                        input.Append((char)c);
                        break;
                }
            }

            if ((c == -1) && (input.Length == 0))
            {
                return null;
            }
            return input.ToString();
        }

        public long ReadLong()
        {
            return ((long)(ReadInt()) << 32) + (ReadInt() & 0xFFFFFFFFL);
        }

        public long ReadLongLe()
        {
            int i1 = ReadIntLe();
            int i2 = ReadIntLe();
            return ((long)i2 << 32) + (i1 & 0xFFFFFFFFL);
        }

        public short ReadShort()
        {
            int ch1 = Read();
            int ch2 = Read();
            if ((ch1 | ch2) < 0)
                throw new EndOfStreamException();
            return (short)((ch1 << 8) + ch2);
        }

        /// <summary>
        /// Reads a signed 16-bit number from this stream in little-endian order.
        /// The method reads two
        /// bytes from this stream, starting at the current stream pointer.
        /// If the two bytes read, in order, are
        ///  b1  and  b2 , where each of the two values is
        /// between  0  and  255 , inclusive, then the
        /// result is equal to:
        ///
        /// (short)((b2 &lt;&lt; 8) | b1)
        ///
        ///
        /// This method blocks until the two bytes are read, the end of the
        /// stream is detected, or an exception is thrown.
        /// 16-bit number.
        /// @exception  EOFException  if this stream reaches the end before reading
        /// two bytes.
        /// @exception  IOException   if an I/O error occurs.
        /// </summary>
        /// <returns>the next two bytes of this stream, interpreted as a signed</returns>
        public short ReadShortLe()
        {
            int ch1 = Read();
            int ch2 = Read();
            if ((ch1 | ch2) < 0)
                throw new EndOfStreamException();
            return (short)((ch2 << 8) + (ch1 << 0));
        }

        public int ReadUnsignedByte()
        {
            int ch = Read();
            if (ch < 0)
                throw new EndOfStreamException();
            return ch;
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer from this stream. This method reads 4
        /// bytes from the stream, starting at the current stream pointer.
        /// This method blocks until the four bytes are read, the end of the
        /// stream is detected, or an exception is thrown.
        ///  long .
        /// @exception  EOFException  if this stream reaches the end before reading
        /// four bytes.
        /// @exception  IOException   if an I/O error occurs.
        /// </summary>
        /// <returns>the next four bytes of this stream, interpreted as a</returns>
        public long ReadUnsignedInt()
        {
            long ch1 = Read();
            long ch2 = Read();
            long ch3 = Read();
            long ch4 = Read();
            if ((ch1 | ch2 | ch3 | ch4) < 0)
                throw new EndOfStreamException();
            return ((ch1 << 24) + (ch2 << 16) + (ch3 << 8) + (ch4 << 0));
        }

        public long ReadUnsignedIntLe()
        {
            long ch1 = Read();
            long ch2 = Read();
            long ch3 = Read();
            long ch4 = Read();
            if ((ch1 | ch2 | ch3 | ch4) < 0)
                throw new EndOfStreamException();
            return ((ch4 << 24) + (ch3 << 16) + (ch2 << 8) + (ch1 << 0));
        }

        public int ReadUnsignedShort()
        {
            int ch1 = Read();
            int ch2 = Read();
            if ((ch1 | ch2) < 0)
                throw new EndOfStreamException();
            return (ch1 << 8) + ch2;
        }

        /// <summary>
        /// Reads an unsigned 16-bit number from this stream in little-endian order.
        /// This method reads
        /// two bytes from the stream, starting at the current stream pointer.
        /// This method blocks until the two bytes are read, the end of the
        /// stream is detected, or an exception is thrown.
        /// unsigned 16-bit integer.
        /// @exception  EOFException  if this stream reaches the end before reading
        /// two bytes.
        /// @exception  IOException   if an I/O error occurs.
        /// </summary>
        /// <returns>the next two bytes of this stream, interpreted as an</returns>
        public int ReadUnsignedShortLe()
        {
            int ch1 = Read();
            int ch2 = Read();
            if ((ch1 | ch2) < 0)
                throw new EndOfStreamException();
            return (ch2 << 8) + (ch1 << 0);
        }

        public void ReOpen()
        {
            if (Filename != null && Rf == null)
                Rf = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            Seek(0);
        }

        public void Seek(int pos)
        {
            pos += _startOffset;
            IsBack = false;
            if (ArrayIn == null)
            {
                InsureOpen();
                Rf.Position = pos;
            }
            else
                ArrayInPtr = pos;
        }

        public void Seek(long pos)
        {
            Seek((int)pos);
        }

        public long Skip(long n)
        {
            return SkipBytes((int)n);
        }

        public int SkipBytes(int n)
        {
            if (n <= 0)
            {
                return 0;
            }
            int adj = 0;
            if (IsBack)
            {
                IsBack = false;
                if (n == 1)
                {
                    return 1;
                }
                else
                {
                    --n;
                    adj = 1;
                }
            }
            int pos;
            int len;
            int newpos;

            pos = FilePointer;
            len = Length;
            newpos = pos + n;
            if (newpos > len)
            {
                newpos = len;
            }
            Seek(newpos);

            /* return the actual number of bytes skipped */
            return newpos - pos + adj;
        }
        protected void InsureOpen()
        {
            if (Filename != null && Rf == null)
            {
                ReOpen();
            }
        }
    }
}
