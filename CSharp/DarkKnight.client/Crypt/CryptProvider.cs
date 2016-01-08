﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DarkKnight.client.Crypt
{
    class CryptProvider
    {/// <summary>
     /// The crypt class registered
     /// </summary>
        private Object _crypt;

        /// <summary>
        /// Indication crypt class is registed if is true
        /// </summary>
        private bool _registed = false;

        /// <summary>
        /// Try decode a package in a registered crypt class
        /// </summary>
        /// <param name="packet">array of byte to decode</param>
        /// <returns>if no error, the array of byte decoded, if generate a error return the original array of byte</returns>
        public byte[] decode(byte[] packet)
        {
            // if no have registed crypt class, return the packet without decoding
            if (!_registed)
                return packet;

            try
            {
                // Try decode and return the packet decoded
                return (byte[])_crypt.GetType().
                    GetMethod("decode").
                    Invoke(_crypt, new object[] { packet });
            }
            catch (TargetInvocationException ex)
            {
                //Log.Write(_crypt.GetType().Name + " responsable for decrypt - " + ex.InnerException.Message + "\n" + ex.InnerException.StackTrace, LogLevel.ERROR);
                // if the packet not be complet the decode, return the original packet from param
                return packet;
            }
        }

        /// <summary>
        /// Trye to encode a packet in a registered crypt class
        /// </summary>
        /// <param name="packet">array of byte to encode</param>
        /// <returns>if no error, the array of byte encoded, if a error return the original array of byte</returns>
        public byte[] encode(byte[] packet)
        {
            // if no have registed crypt class, return the packet without encoding
            if (!_registed)
                return packet;

            try
            {
                // Try encode and return the packet encoded
                return (byte[])_crypt.GetType().
                    GetMethod("encode").
                    Invoke(_crypt, new object[] { packet }); ;
            }
            catch (TargetInvocationException ex)
            {
                //Log.Write(_crypt.GetType().Name + " responsable for encrypt - " + ex.InnerException.Message + "\n" + ex.InnerException.StackTrace, LogLevel.ERROR);
                // if the packet not be complet the encoded, return the original packet from param
                return packet;
            }
        }

        /// <summary>
        /// Return true if crypt class is registed
        /// otherwise false
        /// </summary>
        public bool cryptRegisted
        {
            get { return _registed; }
        }

        /// <summary>
        /// Register a crypt class in the memory
        /// </summary>
        /// <param name="crypt">The AbstractCrypt extended class</param>
        public void registerCrypt<T>(T crypt)
        {
            _crypt = crypt;
            _registed = true;
        }
    }
}
