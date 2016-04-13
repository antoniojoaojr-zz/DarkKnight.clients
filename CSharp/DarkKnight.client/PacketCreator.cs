﻿using System;
using System.Collections.Generic;

namespace DarkKnight.client
{
    class PacketCreator
    {
        /// <summary>
        /// The data formated in default package DarkKnight
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// Gets the data formated
        /// </summary>
        public byte[] data
        {
            get { return _data; }
        }

        /// <summary>
        /// Creator a data formated with not format
        /// </summary>
        /// <param name="data">array of bytes data</param>
        public PacketCreator(byte[] data)
        {
            Creator(new PacketFormat("???"), data);
        }

        /// <summary>
        /// Create data formated with a format
        /// </summary>
        /// <param name="format">DarkKnight.Data.PacketFormat object</param>
        /// <param name="data">array of bytes data</param>
        public PacketCreator(PacketFormat format, byte[] data)
        {
            Creator(format, data);
        }

        private void Creator(PacketFormat format, byte[] data)
        {
            // create format
            byte[] packetFormat = formatingPacket(format.getByteArrayFormat);

            // create data
            byte[] packetData = formatingPacket(data);

            // setting length of packet
            _data = new byte[packetFormat.Length + packetData.Length];

            // copy format to packet
            Array.Copy(packetFormat, _data, packetFormat.Length);

            // copy data to packet
            Array.Copy(packetData, 0, _data, packetFormat.Length, packetData.Length);
        }

        /// <summary>
        /// Get the data formated with a format of DarkKnight package
        /// </summary>
        /// <param name="data">the byte of array for format</param>
        /// <returns>the data formated</returns>
        private byte[] formatingPacket(byte[] data)
        {
            // we get a dynamic array stored a length information in packet
            byte[] lengthData = lengthList(data.Length);

            // we create a dataFormated with length:
            // length of length data + length of length data information + 1 (for position final length information) 
            byte[] packetData = new byte[data.Length + lengthData.Length + 1];

            // convert the dynamic list to the dataFormated
            for (int i = 0; i < lengthData.Length; i++)
                packetData[i] = lengthData[i];

            // add the final length information
            packetData[lengthData.Length] = 0;

            // if length is one, return
            if (packetData.Length < 2)
                return packetData;

            // copy the data to data formated
            Array.Copy(data, 0, packetData, lengthData.Length + 1, data.Length);

            // return the data formated
            return packetData;
        }

        private byte[] lengthList(int _length)
        {
            int length = _length;
            byte[] lengthData = new byte[(int)Math.Ceiling(_length / 127.0f)];

            for (int i = 0; length > 0; i++)
            {
                lengthData[i] = (length > 127) ? (byte)127 : (byte)length;
                length -= lengthData[i];
            }

            return lengthData;
        }
    }
}
