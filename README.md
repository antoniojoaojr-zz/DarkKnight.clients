# DarkKnight.clients

How to use

```sh
    class Program
    {
        static void Main(string[] args)
        {
            Connection("127.0.0.1", 2277);
        }

        static Connection(string ip, int port)
        {
            try
            {
                // we try connection with the server in the ip and port passed in parans
                // and setting the controller class of this connection and data handler CloudReceiver
                DarkKnight.client.Connection connection = new DarkKnight.client.Connection(ip, port, new CloudReceiver());
                // if connected is successfully, we send a package initial
                // this pckage initial is to be customized with your preference
                // in the serve with sync with client
                connection.Send(new DarkKnight.client.PacketFormat("CON"), "CONNECTION");
            }
            catch (Exception ex)
            {
                // if we have a error, show the error msg
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try again in 3 secs");
                // try the connection again in 3 secs later
                Thread.Sleep(3000);
                // try again
                Connection(ip, port);
            }
        }
    }
```

Classe CloudReceveir must be implement ICloud class from DarkKnight.client
```sh
    class CloudReceiver : ICloud
    {
        /// <summary>
        /// This method is automatic called when client is desconnect with server
        /// </summary>
        public override void Disconnected()
        {
            // ou customize ou code here
        }

        /// <summary>
        /// This method is automatic called when client receive a new package from server
        /// </summary>
        /// <param name="obj"></param>
        public override void Receiver(Async obj)
        {
            // obj is a DarkKnight.client.Receiver.Async object
            // your customize you code here
            // like exemple
            // if you receive a pckage format "Hello" with "World!" in your data
            if(obj.packet.format.getStringFormat == "Hello" && obj.packet.dataString=="World!")
            {
                // you response to the server, "Welcome!"
                obj.connection.Send("Welcome!");
            }            
        }
    }
```
