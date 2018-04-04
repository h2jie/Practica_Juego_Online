using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulador
{
    class Program
    {
        static void Main(string[] args)
        {
            Boolean Seguir = true;
            String Let;

            while (Seguir)
            {
                Console.WriteLine("Crear nuevo cliente [S/N]");
                Let = Console.ReadLine();
                if (Let.Equals("N") | Let.Equals("n"))
                {
                    Seguir = false;
                }
                else
                {
                    Process process = new Process();
                    process.StartInfo.FileName = @"C:\Users\hjie\Desktop\C#\Game_Hangjie_Huang\Game\bin\Debug\CatchGame.exe";
                    process.Start();

                }
            }
        }
    }
}
