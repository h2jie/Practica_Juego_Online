using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CurrentGame;
using GameLibrary;

namespace Servidor
{

    class Program
    {
        private static IPAddress ServerIP;
        private static List<ClientConnected> ListaClientesConnectados = new List<ClientConnected>();
        private static readonly object locker = new object();
        private static int idCliente = 0;
        private static MyGame currentGame;




        static void Main(string[] args)
        {
            int ServerPort = 50000;
            ServerIP = IPAddress.Parse("127.0.0.1");
            TcpListener Server = new TcpListener(ServerIP, ServerPort);
            Console.WriteLine("Servedor creat");
            Server.Start();
            Console.WriteLine("Server iniciat");
            currentGame = new MyGame();
            currentGame.gameStatus = true;


            while (true)
            {
                //Cuando se conecta al cliente se imprime la mensaje
                TcpClient Client = Server.AcceptTcpClient();

                Console.WriteLine("Client connectat");

                Thread t = new Thread(Service);
                t.Start(Client);
            }

            Server.Stop();
        }

        static void Service(Object o)
        {
            TcpClient tcpCliente = (TcpClient)o;
            NetworkStream ServerNS = tcpCliente.GetStream();        
            ClientConnected clientes;
            int idClienteActual = idCliente;
            Posicion PosRecibido;

            try
            {
                //Añadir cliente a la lista
                clientes = new ClientConnected(idClienteActual, ServerNS);
                ListaClientesConnectados.Add(clientes);
                idCliente++;


                //Enviar id al cliente
                string idString = clientes.IdClient.ToString();
                byte[] idBytes = Encoding.UTF8.GetBytes(idString);

                ServerNS.Write(idBytes, 0, idBytes.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ha cerrado servidor");
            }



            

            //Recibir posicion del cliente y escribir en consola
            while(true)
            {
                byte[] BufferLocal = new byte[256];
                int BytesRebuts = ServerNS.Read(BufferLocal, 0, BufferLocal.Length);
                
                PosRecibido = Posicion.Deserialize(BufferLocal);
                Console.WriteLine("pos X: "+PosRecibido.PosX+"   pos Y: "+PosRecibido.PosY+"  jugador: "+idClienteActual );


                //Enviar posicion a otro cliente

                for (int i = 0; i < ListaClientesConnectados.Count; i++)
                {
                    
                    if (ListaClientesConnectados[i].IdClient != idClienteActual)
                    {
                        ListaClientesConnectados[i].NetworkStream.Write(BufferLocal, 0, BufferLocal.Length);
                    }
                }

            }

        }



    }
}
