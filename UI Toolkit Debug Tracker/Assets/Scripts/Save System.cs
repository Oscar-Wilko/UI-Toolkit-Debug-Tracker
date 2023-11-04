using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public struct DebugInstance
{
    public DebugType type;
    public string title;
    public string description;
    public string date;
    public float[] position;
    public string scene;
    public UrgencyType urgency;
    public string author;
    public string machine;
    public DebugInstance(DebugType _ty, string _ti, string _de, string _da, float[] _po, string _sc, UrgencyType _ur, string _au, string _ma)
    {
        type = _ty;
        title = _ti;
        description = _de;
        date = _da;
        position = _po;
        scene = _sc;
        urgency = _ur;
        author = _au;
        machine = _ma;
    }
}

public enum DebugType
{
    Fatal,
    Risk,
    Warning,
    Improvement,
    Design
}

public enum UrgencyType
{
    ASAP,
    Soon,
    Later,
    Whenever
}

public static class SaveSystem
{
    public class SavedDebugs
    {
        public List<DebugInstance> list;
        public SavedDebugs(List<DebugInstance> _list)
        {
            list = _list;
        }
    }

    /* BINARY FORMATTED SAVING
    public static void SaveDebugs(List<DebugInstance> debugs)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(GetFileLocation(), FileMode.Create);
        formatter.Serialize(stream, debugs);
        stream.Close();
    }
    
    public static List<DebugInstance> LoadDebugs()
    {
        if (!File.Exists(GetFileLocation()))
        {
            Debug.LogError("Debug file missing"); 
            return null;
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(GetFileLocation(), FileMode.Open);
        List<DebugInstance> data = formatter.Deserialize(stream) as List<DebugInstance>;
        stream.Close();
        return data;
    }*/

    public static void SaveDebugs(List<DebugInstance> debugs)
    {
        string saved_data = JsonUtility.ToJson(new SavedDebugs(debugs));
        File.WriteAllText(GetDebugsFileLocation(), saved_data);
    }

    public static List<DebugInstance> LoadDebugs()
    {
        if (File.Exists(GetDebugsFileLocation()))
        {
            string loaded_data = File.ReadAllText(GetDebugsFileLocation());
            List<DebugInstance> debugs = JsonUtility.FromJson<SavedDebugs>(loaded_data).list;
            return debugs;
        }
        return null;
    }
    
    public static void SaveCustomisation(DebugCustomiser.CustomData customisation)
    {
        string saved_data = JsonUtility.ToJson(customisation);
        File.WriteAllText(GetCustomisationFileLocation(), saved_data);
    }

    public static DebugCustomiser.CustomData LoadCustomisation()
    {
        if (File.Exists(GetCustomisationFileLocation()))
        {
            string loaded_data = File.ReadAllText(GetCustomisationFileLocation());
            DebugCustomiser.CustomData customisation = JsonUtility.FromJson<DebugCustomiser.CustomData>(loaded_data);
            return customisation;
        }
        return null;
    }

    private static string GetDebugsFileLocation()
    {
        return "Assets/SavedData/debugs.json";
    }

    private static string GetCustomisationFileLocation()
    {
        return "Assets/SavedData/customisation.json";
    }
}
