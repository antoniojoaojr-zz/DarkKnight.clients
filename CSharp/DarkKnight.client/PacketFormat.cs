using System;
using System.Text;

namespace DarkKnight.client
{
    public class PacketFormat
    {
        private string format = null;

        /// <summary>
        /// Sets the format of a packet passing as parameter string
        /// </summary>
        /// <param name="name">The name of format</param>
        public PacketFormat(string name)
        {
            if (name.Length < 3)
                throw new Exception("The min length of name is 3");

            format = name;
        }

        /// <summary>
        /// Gets the packet format in a string
        /// </summary>
        public string getStringFormat
        {
            get
            {
                if (format == null)
                    throw new Exception("The format is not setted");

                return format;
            }
        }

        /// <summary>
        /// Gets the packet format in a array of char
        /// </summary>
        public char[] getCharArrayFormat
        {
            get
            {
                if (format == null)
                    throw new Exception("the format is not setted");

                return format.ToCharArray();
            }
        }

        /// <summary>
        /// Gets the packet format in a array of byte
        /// </summary>
        public byte[] getByteArrayFormat
        {
            get
            {
                if (format == null)
                    throw new Exception("the format is not setted");

                return Encoding.UTF8.GetBytes(format);
            }
        }
    }
}
