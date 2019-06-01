using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace PnCCasualGameKit
{
    /// <summary>
    /// A Generic implementation for creating , fetching and saving player data into the persistant storage.
    /// Extend from this class and declare the data fields in it.
    /// </summary>
    [Serializable]
    public class PlayerDataHandler<T> where T : new()
    {
        static protected T instance;

        /// <summary>
        /// If instance is null create new insance otherwise return the instance.
        /// </summary>
        static public T Instance
        {
            get
            {
                if (instance == null)
                {
                    Create();
                }
                return instance;
            }
        }

        static string FilePath = Application.persistentDataPath + "/gameData.sv";

        /// <summary>
        /// Save the data
        /// </summary>
        public void SaveData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(FilePath);
            bf.Serialize(file, instance);
            file.Close();
        }

        /// <summary>
        /// Create the instance. If data exists in the storage, fetch it and save it as an instance. 
        /// Else create a fresh new instance
        /// </summary>
        public static void Create()
        {
            if (File.Exists(FilePath))
            {
                //Debug.Log(typeof(T).FullName + " " + FilePath);
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(FilePath, FileMode.Open);
                instance = (T)bf.Deserialize(file);
                file.Close();
            }
            else
            {
                instance = new T();
            }
        }

        /// <summary>
        /// Clears data by deleting the date file and creating a new empty instance.
        /// </summary>
        public static void Clear()
        {
            try
            {
                File.Delete(FilePath);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            instance = new T();
        }
    }
}