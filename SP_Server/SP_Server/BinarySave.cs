using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP_Server
{
    public class BinarySave
    {
        public static void Serialize<Object>(Object obj, string fileName)
        {
            try
            {
                FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);

                using (stream)
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, obj);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }           
        }

        public static Object Deserialize<Object>(string fileName) where Object : new()
        {            
            Object ret = CreateInstance<Object>();

            try
            {
                if (File.Exists(fileName) == false)
                    return ret;

                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                using (stream)
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    ret = (Object)bin.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }            

            return ret;
        }

        private static Object CreateInstance<Object>() where Object : new()
        {
            return (Object)Activator.CreateInstance(typeof(Object));
        }
    }    
}
