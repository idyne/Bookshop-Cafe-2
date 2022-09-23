using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace FateGames
{
    public static class SaveManager
    {
        public static void Save<T>(T data) where T : Data
        {
            if (data == null)
            {
                Debug.LogError("Cannot save null data!");
                return;
            }
            Debug.Log("Saving...");
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + typeof(T).Name + ".fate";
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static T Load<T>() where T : Data
        {
            return Load<T>(() => { });
        }

        public static T Load<T>(Action Callback) where T : Data
        {
            string path = Application.persistentDataPath + "/" + typeof(T).Name + ".fate";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                stream.Position = 0;
                T data = formatter.Deserialize(stream) as T;
                stream.Close();
                Callback();
                return data;
            }
            else
            {
                Debug.Log("Save file not found.");
                return null;
            }
        }
    }

}
