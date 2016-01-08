namespace DarkKnight.client
{
    public abstract class ICloud
    {
        /// <summary>
        /// this method is called when client is desconnected from the server
        /// </summary>
        public abstract void Disconnected();

        /// <summary>
        /// this method is called when client receive new package from the server
        /// </summary>
        /// <param name="obj"></param>
        public abstract void Receiver(Receiver.Async obj);
    }
}
