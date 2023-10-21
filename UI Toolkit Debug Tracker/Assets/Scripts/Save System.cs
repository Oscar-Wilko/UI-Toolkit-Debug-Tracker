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
    public DateTime date;
    public float[] position;
    public string scene;
    public DebugInstance(DebugType _ty, string _ti, string _de, DateTime _da, float[] _po, string _sc)
    {
        type = _ty;
        title = _ti;
        description = _de;
        date = _da;
        position = _po;
        scene = _sc;
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

public static class SaveSystem
{
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
    }

    private static string GetFileLocation()
    {
        return Application.persistentDataPath + "/debugs.bug";
    }
}
