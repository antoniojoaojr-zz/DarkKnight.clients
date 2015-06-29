using System;
using System.Net.Sockets;
using System.Threading;

namespace DarkKnight.client
{
    /// <summary>
    /// This class deals with possible errors that can be generated when receiving a packet
    /// Here we can see if it was generated error and what error was generated.
    /// </summary>
    public abstract class AsyncReceiverError
    {
        protected bool _error = false;
        protected Exception _errorData = null;

        /// <summary>
        /// Gets the received is completed
        /// if not because a error is generated
        /// </summary>
        public bool isCompleted
        {
            get { return !_error; }
        }

        /// <summary>
        /// Gets detail error generated
        /// returns null in case no error
        /// </summary>
        public Exception error
        {
            get { return _errorData; }
        }

    }

    /// <summary>
    /// This class is the response of the receipt of a given from the server asynchronously.
    /// Here are the data that the server sent us.
    /// </summary>
    public abstract class AsyncReceiver : AsyncReceiverError
    {
        protected Socket _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private Client _clientObjt;
        private Action<AsyncReceiver> _objCallback;
        private Packet _packet;
        protected bool isReceiving = false;

        protected ManualResetEvent receiveWork = new ManualResetEvent(false);

        /// <summary>
        /// Gets the data received and flush to receive more data async
        /// Used in callback of ReceiveAsync
        /// </summary>
        /// <returns>DarkKnight.Packet obj with data</returns>
        public Packet EndReceiver()
        {
            receiveWork.Set();
            isReceiving = false;
            return _packet;
        }

        /// <summary>
        /// Gets the DarkKnight.client.Client object from this received
        /// </summary>
        public Client client
        {
            get { return _clientObjt; }
        }

        protected void ReceiveAsync(Action<AsyncReceiver> objCallback, Client _client)
        {
            lock (objCallback)
            {
                if (isReceiving)
                    throw new Exception("Wait for receiver");

                isReceiving = true;
                _error = false;
                _errorData = null;
                _objCallback = objCallback;
                _clientObjt = _client;

                new Thread(new ThreadStart(ThreadReceive)).Start();
            }
        }

        protected Packet Receive()
        {
            byte[] buffer = new byte[65535];
            int size = _client.Receive(buffer);
            if (size == 0)
                throw new Exception("No data received");

            byte[] length = new byte[size];
            Array.Copy(buffer, length, size);

            return new PacketHandler(length);
        }

        private void ThreadReceive()
        {
            try
            {
                _packet = Receive();
            }
            catch (Exception ex)
            {
                _error = true;
                _errorData = ex;
            }
            finally
            {
                _objCallback(this);
            }
        }
    }
}
