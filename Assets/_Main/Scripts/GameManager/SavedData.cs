using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Serializable data
/// For the ease of use, use SavedData instead 
/// </summary>
[System.Serializable]
public class SerializableSavedData
{

    public int count;

}


public class SavedData
{
    public int count;
    private static readonly string gameName = "MyOrdinaryLife.dat";

    public SerializableSavedData GetSeralizableData()
    {
        SerializableSavedData serializableData = new SerializableSavedData();

        serializableData.count = count;


        return serializableData;
    }

    public void FromSerializableData(SerializableSavedData serializableData)
    {
        count = serializableData.count;
    }


    public void Save(string additionalPath)
    {

        BinaryFormatter bf = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, additionalPath + gameName);
        FileStream file = File.Create(path);

        //Since Unity doesn't support serializing Dictionary, we have to convert it to List 
        SerializableSavedData serializableData = this.GetSeralizableData();

        bf.Serialize(file, serializableData);

        file.Close();


    }

    /// <summary>
    /// Load custom data and return true 
    /// </summary>
    /// <returns></returns>
    public bool Load(string addtionalPath)
    {
        string path = Path.Combine(Application.persistentDataPath, addtionalPath + gameName);

        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);

            SerializableSavedData serializableData = (SerializableSavedData)bf.Deserialize(file);

            this.FromSerializableData(serializableData);

            file.Close();

            return true;
        }

        return false;

    }

    public static void DeleteData()
    {
        string[] paths = Directory.GetFiles(Application.persistentDataPath, "*" + gameName);
        for (int i = 0; i < paths.Length; i++)
        {
            File.Delete(paths[i]);
        }
    }
}

#if UNITY_EDITOR
class MyWindow : Editor
{
    [MenuItem("Tool/Delete All SavedData")]
    public static void ShowWindow()
    {
        SavedData.DeleteData();
    }
}
#endif