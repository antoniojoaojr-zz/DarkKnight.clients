using System;
using System.Net;
using System.Text;

#region License Information
/* ************************************************************
 * 
 *    @author AntonioJr <antonio@emplehstudios.com.br>
 *    @copyright 2015 Empleh Studios, Inc
 * 
 * 	  Project Folder: https://github.com/antoniojoaojr/DarkKnight
 * 
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *    
 *        http://www.apache.org/licenses/LICENSE-2.0
 *    
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *    
 * ************************************************************/
#endregion

namespace DarkKnight.client
{
    /// <summary>
    /// This class handles the connection to a server and gives us the ability to send and receive data to the same
    /// </summary>
    /// <exception cref="System.ArgumentNullException">address is null</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">The port number is not valid.</exception>
    /// <exception cref="System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information.</exception>
    /// <exception cref="System.ObjectDisposedException">The System.Net.Sockets.Socket has been closed.</exception>
    /// <exception cref="System.NotSupportedException">This method is valid for sockets in the System.Net.Sockets.AddressFamily.InterNetwork or System.Net.Sockets.AddressFamily.InterNetworkV6 families</exception>
    /// <exception cref="System.ArgumentException">The length of address is zero</exception>
    /// <exception cref="System.InvalidOperationException">The System.Net.Sockets.Socket is System.Net.Sockets.Socket.Listen(System.Int32)ing.</exception>
    /// <exception cref="System.Exception">Not completed handshake with the server</exception>
    public class Client : AsyncReceiver
    {
        private DataTransport transportLayer;


        private IPAddress getIpAddress(string dns)
        {
            try
            {
                return IPAddress.Parse(dns);
            }
            catch
            {
                return Dns.GetHostAddresses(dns)[0];
            }
        }

        public Client(string dns, int port)
        {
            _client.Connect(getIpAddress(dns), port);
            transportLayer = new DataTransport(this, _client);

            transportLayer.Send(new byte[] { 32, 32 });

            byte[] auth = new byte[2];
            _client.Receive(auth);

            if (auth[0] == 32 && auth[1] == 32)
                transportLayer.StartPing();
            else
                throw new Exception("Invalid handshake with server");
        }

        /// <summary>
        /// We try to get a server package
        /// </summary>
        /// <returns>DarkKnight.client.Packet obj received</returns>
        /// <exception cref="System.ArgumentNullException">address is null</exception>
        /// <exception cref="System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information.</exception>
        /// <exception cref="System.ObjectDisposedException">The System.Net.Sockets.Socket has been closed.</exception>
        /// <exception cref="System.Security.SecurityException">A caller in the call stack does not have the required permissions.</exception>
        /// <exception cref="System.Exception">If no data received generated a error.</exception>
        public new Packet Receive()
        {
            if (isReceiving)
            {
                receiveWork.WaitOne();
            }

            return base.Receive();
        }

        /// <summary>
        /// We receive a server package asyncronous
        /// </summary>
        /// <param name="objCallback">The method callback when data is received is ready to read</param>
        /// <exception cref="System.Exception">When a ReceiveAsync is in progress</exception>
        public void ReceiveAsync(Action<AsyncReceiver> objCallback)
        {
            base.ReceiveAsync(objCallback, this);
        }

        /// <summary>
        /// We close a connection with server
        /// </summary>
        public void Close()
        {
            _client.Close();
        }

        /// <summary>
        /// We send a byte to server
        /// </summary>
        /// <param name="toSend">byte to send</param>
        public void Send(byte toSend)
        {
            Send(new byte[] { toSend });
        }

        /// <summary>
        /// Send a array of bytes 8-bits to the server
        /// </summary>
        /// <param name="toSend">The array of bytes to send</param>
        public void Send(byte[] toSend)
        {
            SendEncodingPacket(new PacketCreator(toSend));
        }

        /// <summary>
        /// Send a UTF8 String to the server
        /// </summary>
        /// <param name="toSend">The string to send</param>
        public void SendString(string toSend)
        {
            Send(Encoding.UTF8.GetBytes(toSend));
        }

        /// <summary>
        /// Send a mapped with format to the server
        /// </summary>
        /// <param name="format">DarkKnight.client.PacketFormat object</param>
        public void SendFormated(PacketFormat format)
        {
            SendEncodingPacket(new PacketCreator(format, new byte[] { }));
        }

        /// <summary>
        /// Send a array of bytes 8-bits mapped with format to the server
        /// </summary>
        /// <param name="format">DarkKnight.client.PacketFormat object</param>
        /// <param name="toSend">the int to send</param>
        public void SendFormated(PacketFormat format, byte[] toSend)
        {
            SendEncodingPacket(new PacketCreator(format, toSend));
        }

        /// <summary>
        ///  Send a UTF8 String mapped with format to the server
        /// </summary>
        /// <param name="format">DarkKnight.client.PacketFormat object</param>
        /// <param name="toSend">the string to send</param>
        public void SendFormatedString(PacketFormat format, string toSend)
        {
            SendFormated(format, Encoding.UTF8.GetBytes(toSend));
        }

        private void SendEncodingPacket(PacketCreator packet)
        {
            transportLayer.Send(packet.data);
        }


    }
}
