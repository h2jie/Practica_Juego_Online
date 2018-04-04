using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Client
{
    class Program
    {
        private static IPAddress ServerIP;
        private static int ClientPort;

        static void Main(string[] args)
        {


            ClientPort = 50000;
            ServerIP = IPAddress.Parse("127.0.0.1");

            TcpClient Client = new TcpClient();

            Client.Connect(ServerIP, ClientPort);

            if (Client.Connected)
                Console.WriteLine("Connectat al servidor");

            NetworkStream ClientNS = Client.GetStream();


            Thread t = new Thread(recibir);
            t.Start(ClientNS);

            Boolean Seguir = true;

            while (Seguir)
            {
                Console.WriteLine("Escriu una frase:");
                string frase = Console.ReadLine();

                /*if (frase.Equals("q") | frase.Equals("Q"))
                {
                    Console.WriteLine("final");

                    //Passem de string a bytes
                    byte[] fraseBytes = Encoding.UTF8.GetBytes(frase);

                    //Enviem al servidor
                    ClientNS.Write(fraseBytes, 0, fraseBytes.Length);

                    ClientNS.Close();
                    Client.Close();
                    Seguir = false;

                }
                else
                {*/
                    //Passem de string a bytes
                    byte[] fraseBytes = Encoding.UTF8.GetBytes(frase);
                    //Enviem al servidor
                    ClientNS.Write(fraseBytes, 0, fraseBytes.Length);

                    /*byte[] BufferLocal = new byte[256];
                    int BytesRebuts = ClientNS.Read(BufferLocal, 0, BufferLocal.Length);

                    string s = "";
                    //Passem de bytes a string
                    s = Encoding.UTF8.GetString(BufferLocal, 0, BytesRebuts);

                    Console.WriteLine("{0}", s);*/
                //}

            }

        }

        static void recibir(object o)
        {
            NetworkStream NScliente = (NetworkStream)o;

            while (true)
            {
                byte[] BufferLocal = new byte[256];
                int BytesRebuts = NScliente.Read(BufferLocal, 0, BufferLocal.Length);

                string s = "";
                //Passem de bytes a string
                s = Encoding.UTF8.GetString(BufferLocal, 0, BytesRebuts);

                Console.WriteLine("{0}", s);
            }
 
        }


    }
}
