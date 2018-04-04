using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Net.Sockets;
using System.Net;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using GameLibrary;
using System.Threading;
using System.Text;

namespace Game
{
    public class Ball
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public BallGraphics BallDraw;

        public Ball (int posX, int posY, int width, int height, Color color)
        {
            PosX = posX;
            PosY = posY;

            BallDraw = new BallGraphics(width, height, color);
        }
    }

    public class BallGraphics
    {
        public Ellipse ShapeBall { get; set; }
        public Color Color { get; set; }

        public BallGraphics(int width, int height, Color color)
        {
            ShapeBall = new Ellipse();
            ShapeBall.Width = width;
            ShapeBall.Height = height;
            Color = color;
            ShapeBall.Fill = new SolidColorBrush(Color);

        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dTimer = new DispatcherTimer();
        int NumBalls = 0;
        List<Ball> Balls = new List<Ball>();
        private static IPAddress ServerIP;
        public static NetworkStream ClientNS;
        public int idJugador;
        public static int other,me;



        public MainWindow()
        {
            InitializeComponent();
            int ServerPort = 50000;
            ServerIP = IPAddress.Parse("127.0.0.1");
            TcpClient client = new TcpClient();
                
            client.Connect(ServerIP, ServerPort);


            if (client.Connected)
            {
                ClientNS = client.GetStream();
                byte[] BufferLocal = new byte[256];
                int BufferRebut = ClientNS.Read(BufferLocal, 0, BufferLocal.Length);


                string id;
                id = Encoding.UTF8.GetString(BufferLocal, 0, BufferRebut);
                idJugador = Int32.Parse(id);

                CreateBall();
                CrearSegundaBola();
                puntoFinal();

                //un if para controlar la id del mio y otro cliente
                if (idJugador == 0)
                {
                    me = 0;
                    other = 1;
                }
                else if(idJugador==1)
                {
                    me = 1;
                    other = 0;
                }

                //Crear hilo para recibir informacion desde servidor
                Thread t = new Thread(recibirPos);

                t.Start(ClientNS);

            }
            
            Loop();
        }

        void movimientoPuntoFinal()
        {
            while ()
            {

                //Balls[3].
            }
        }


        //Recibir posicion de otra bola desde servidor, ponemos while para que puede escuchar todo rato
        void recibirPos(object clientNS)
        {
            NetworkStream NS = (NetworkStream)clientNS;

            while (true)
            {

                try
                {
                    byte[] BufferRecibido = new byte[256];
                    int bytesRecibido = NS.Read(BufferRecibido, 0, BufferRecibido.Length);

                    Posicion pos = Posicion.Deserialize(BufferRecibido) as Posicion;

                    Balls[other].PosX = pos.PosX;
                    Balls[other].PosY = pos.PosY;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ha cerrado jugador");
                }

            }

            
        }



        public void CreateBall()
        {
            
                //Creem una bola
                Ball ball = new Ball(200, 325, 50, 50, Colors.Red);

                //Dibuixem la bola al taulell
                CanvasBalls.Children.Add(ball.BallDraw.ShapeBall);
                DrawBall(ball);

                //Guardem informació de la bola
                Balls.Add(ball);
                NumBalls++;
            
            
        }

        public void CrearSegundaBola()
        {
            Ball ball = new Ball(20, 20, 50, 50, Colors.Black);

            CanvasBalls.Children.Add(ball.BallDraw.ShapeBall);
            DrawBall(ball);

            Balls.Add(ball);
            NumBalls++;
            
        }
        public void puntoFinal()
        {
            Ball ball = new Ball(200, 200, 50, 50, Colors.Aqua);

            CanvasBalls.Children.Add(ball.BallDraw.ShapeBall);
            DrawBall(ball);

            Balls.Add(ball);
            NumBalls++;

        }
        public void Loop()
        {
            dTimer.Interval = TimeSpan.FromMilliseconds(30);
            dTimer.Tick += Timer_Tick;
            dTimer.Start();
        }

        void Timer_Tick(object sender, EventArgs e)
        {

            //creamos un foreach para que puede dibujar los bola que están en la lista
            foreach (Ball ball in Balls)
            {
                DrawBall(ball);
            }

            

        }

        public void DrawBall(Ball infoBall)
        {
            Canvas.SetLeft(infoBall.BallDraw.ShapeBall, infoBall.PosX);
            Canvas.SetTop(infoBall.BallDraw.ShapeBall, infoBall.PosY);
        }



        void CanvasKeyDown(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.D:
                    if(Balls[me].PosX < 656)
                    {
                        Balls[me].PosX = Balls[me].PosX + 4;
                    }
                    break;
                case Key.A:
                    if(Balls[me].PosX > 0)
                    {
                        Balls[me].PosX = Balls[me].PosX - 4;
                    }
                    break;
                case Key.S:
                    if(Balls[me].PosY < 421)
                    {
                        Balls[me].PosY = Balls[me].PosY + 4;
                    }
                    break;
                case Key.W:
                    if (Balls[me].PosY >1)
                    {
                        Balls[me].PosY = Balls[me].PosY - 4;
                    }
                    
                    break;
                default:                    
                    break;
            }

            //Enviar posicion al servidor
            Posicion posicion = new Posicion(Balls[me].PosX, Balls[me].PosY);
            byte[] posicionToBytes = Posicion.Serialize(posicion);
            ClientNS.Write(posicionToBytes, 0, posicionToBytes.Length);
            

        }

        
    }
}
