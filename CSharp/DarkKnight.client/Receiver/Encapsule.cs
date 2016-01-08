using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkKnight.client.Receiver
{
    class Encapsule : Async
    {
        public Encapsule(Connection connection, Packet packet)
        {
            _connection = connection;
            _packet = packet;
        }
    }
}
