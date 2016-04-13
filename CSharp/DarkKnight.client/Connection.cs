using DarkKnight.client.Crypt;
using System.Net;
using System.Text;

namespace DarkKnight.client
{
    public class Connection
    {
        private SocketCore socketCore = new SocketCore();

        public Connection(string dns, int port, ICloud receiver)
        {
            socketCore.socket.Connect(getIpAddress(dns), port);
            socketCore.Connection(this);
            new ReceiverController(socketCore, receiver);
        }

        /// <summary>
        /// When is true you receive packets asynchronous
        /// if is false the packets are received in sequence
        /// </summary>
        public bool ReceiveAsync
        {
            get { return socketCore.ReceiveAsync; }
            set { socketCore.ReceiveAsync = value; }
        }

        /// <summary>
        /// Gets the client is connected with the server
        /// </summary>
        public bool isConnected
        {
            get { return socketCore.connected; }
        }

        /// <summary>
        /// Register a cryptograph class for this client to encrypt and decrypt packaged send and receive
        /// </summary>
        /// <param name="Crypt">The object crypt</param>
        public void RegisterCrypt(AbstractCrypt Crypt)
        {
            socketCore.RegisterCrypt(Crypt);
        }

        /// <summary>
        /// We close a connection with server
        /// </summary>
        public void Close()
        {
            socketCore.Close();
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
            SendPacket(new PacketCreator(toSend));
        }

        /// <summary>
        /// Send a UTF8 String to the server
        /// </summary>
        /// <param name="toSend">The string to send</param>
        public void Send(string toSend)
        {
            Send(Encoding.UTF8.GetBytes(toSend));
        }

        /// <summary>
        /// Send a mapped with format to the server
        /// </summary>
        /// <param name="format">DarkKnight.client.PacketFormat object</param>
        public void Send(PacketFormat format)
        {
            Send(format, new byte[] { });
        }

        /// <summary>
        /// Send a array of bytes 8-bits mapped with format to the server
        /// </summary>
        /// <param name="format">DarkKnight.client.PacketFormat object</param>
        /// <param name="toSend">the int to send</param>
        public void Send(PacketFormat format, byte[] toSend)
        {
            SendPacket(new PacketCreator(format, toSend));
        }

        /// <summary>
        ///  Send a UTF8 String mapped with format to the server
        /// </summary>
        /// <param name="format">DarkKnight.client.PacketFormat object</param>
        /// <param name="toSend">the string to send</param>
        public void Send(PacketFormat format, string toSend)
        {
            Send(format, Encoding.UTF8.GetBytes(toSend));
        }

        private void SendPacket(PacketCreator packet)
        {
            socketCore.SendPacket(packet.data);
        }

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

    }
}
