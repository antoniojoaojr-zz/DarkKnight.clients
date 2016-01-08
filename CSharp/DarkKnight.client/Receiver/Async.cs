namespace DarkKnight.client.Receiver
{
    public abstract class Async
    {
        protected Connection _connection;
        protected Packet _packet;

        public Connection connection
        {
            get { return _connection; }
        }

        public Packet packet
        {
            get { return _packet; }
        }
    }
}
