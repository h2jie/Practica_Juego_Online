using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameLibreria
{

    [Serializable]
    public class Posicion
    {
        public int PosX{get; set;}
        public int PosY { get; set; }

        public Posicion(int PosX, int PosY)
        {
            this.PosY = PosY;
            this.PosX = PosX;
        }

        public byte[] sendPosicion()
        {
            byte[] posicion = Serialize(this);
            return posicion;
        }
        


        public static byte[] Serialize(object obj)
        {
            byte[] bytesPos;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                bytesPos = ms.ToArray();
            }

            return bytesPos;
        }


        public static Posicion Deserialize(byte[] param)
        {
            Posicion obj = null;
            using (MemoryStream ms = new MemoryStream(param))
            {
                IFormatter br = new BinaryFormatter();
                obj = (br.Deserialize(ms) as Posicion);
            }

            return obj;
        }
    }



}
