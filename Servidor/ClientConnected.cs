using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using GameLibrary;

namespace Servidor
{
    public class ClientConnected
    {
        public int IdClient { get; set; }
        public NetworkStream NetworkStream { get; set; }
        public Posicion Posicion { get; set; }

        public ClientConnected(int IdClient, NetworkStream NetworkStream)
        {
            this.IdClient = IdClient;
            this.NetworkStream = NetworkStream;
        }

        
    }
}
