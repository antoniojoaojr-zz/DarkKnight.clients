using DarkKnight.client.Crypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DarkKnight.client
{
    class SocketCore
    {
        private DataTransport transportLayer;
        private CryptProvider cryptProvider = new CryptProvider();

        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public bool connected = false;
        public Connection _connection;
        public ICloud application;
        public bool ReceiveAsync = false;

        public void Connection(Connection connection)
        {
            transportLayer = new DataTransport(connection, socket);
            transportLayer.Send(new byte[] { 32, 32 });

            byte[] auth = new byte[2];
            socket.Receive(auth);
            if (auth[0] == 32 && auth[1] == 32)
                transportLayer.StartPing();
            else
                throw new Exception("Invalid handshake with server");

            connected = true;
            _connection = connection;
        }

        public void RegisterCrypt(AbstractCrypt Crypt)
        {
            cryptProvider.registerCrypt(Crypt);
        }

        public void SendPacket(byte[] packet)
        {
            transportLayer.Send(cryptProvider.encode(packet));
        }

        public void Close()
        {
            socket.Close();
            if (connected)
            {
                connected = false;
                application.Disconnected();
            }
        }

        public byte[] decodeCrypt(byte[] pack)
        {
            return cryptProvider.decode(pack);
        }
    }
}
