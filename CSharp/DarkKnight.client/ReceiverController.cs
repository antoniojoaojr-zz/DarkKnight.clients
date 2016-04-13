
using System;
using System.Threading;

namespace DarkKnight.client
{
    class ReceiverController
    {
        private SocketCore socketCore;

        public ReceiverController(SocketCore socketCore, ICloud receiver)
        {
            this.socketCore = socketCore;
            socketCore.application = receiver;

            // Start receive package from server async
            (new Thread(new ThreadStart(Receive))).Start();
        }

        private void Receive()
        {
            if (!socketCore.connected)
                return;

            byte[] buffer = new byte[65535];
            try
            {
                int size = socketCore.socket.Receive(buffer);
                if (size == 0)
                {
                    if (!socketCore.socket.Connected)
                    {
                        socketCore.Close();
                        return;
                    }
                    Receive();
                }

                byte[] length = new byte[size];
                Array.Copy(buffer, length, size);

                PacketHandler packet = new PacketHandler(socketCore.decodeCrypt(length));
                if (packet.format.getStringFormat == "???" && packet.data.Length == 0)
                {
                    socketCore.Close();
                    throw new Exception("Invalid data received from server {debug: {" + packet.invalidData + "}}");
                }

                foreach (Packet p in packet.packetHandled)
                {
                    // send packet received to the app
                    if (!socketCore.ReceiveAsync)
                        socketCore.application.Receiver(new Receiver.Encapsule(socketCore._connection, p));
                    else
                        (new Thread(() => socketCore.application.Receiver(new Receiver.Encapsule(socketCore._connection, p)))).Start();
                }

                Receive();
            }
            catch
            {
                socketCore.Close();
            }
        }
    }
}
