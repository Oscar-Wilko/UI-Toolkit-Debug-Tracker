using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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

    /// <summary>
    /// Save List Of Debugs To JSON File
    /// </summary>
    /// <param name="debugs">List of DebugInstance's to save</param>
    public static void SaveDebugs(List<DebugInstance> debugs)
    {
        FolderCheck();
        string saved_data = JsonUtility.ToJson(new SavedDebugs(debugs), true);
        File.WriteAllText(GetDebugsFileLocation(), saved_data);
    }

    /// <summary>
    /// Get List Of Debugs From JSON File
    /// </summary>
    /// <returns></returns>
    public static List<DebugInstance> LoadDebugs()
    {
        FolderCheck();
        if (File.Exists(GetDebugsFileLocation()))
        {
            string loaded_data = File.ReadAllText(GetDebugsFileLocation());
            SavedDebugs data = JsonUtility.FromJson<SavedDebugs>(loaded_data);
            if (data != null)
            {
                List<DebugInstance> debugs = data.list;
                return debugs;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Save Customisation Data To JSON File
    /// </summary>
    /// <param name="customisation">CustomData of customisations</param>
    public static void SaveCustomisation(DebugCustomiser.CustomData customisation)
    {
        FolderCheck();
        string saved_data = JsonUtility.ToJson(customisation, true);
        File.WriteAllText(GetCustomisationFileLocation(), saved_data);
    }

    /// <summary>
    /// Load Customisation Data From JSON File
    /// </summary>
    /// <returns>CustomData of customisations</returns>
    public static DebugCustomiser.CustomData LoadCustomisation()
    {
        FolderCheck();
        if (File.Exists(GetCustomisationFileLocation()))
        {
            string loaded_data = File.ReadAllText(GetCustomisationFileLocation());
            DebugCustomiser.CustomData customisation = JsonUtility.FromJson<DebugCustomiser.CustomData>(loaded_data);
            return customisation;
        }
        return null;
    }

    /// <summary>
    /// Get File Location Of Debugs
    /// </summary>
    /// <returns>String of file location</returns>
    private static string GetDebugsFileLocation()
    {
        return "Assets/DebugPackage/SavedData/debugs.json";
    }

    /// <summary>
    /// Get File Location Of Cusomisation
    /// </summary>
    /// <returns>String of file location</returns>
    private static string GetCustomisationFileLocation()
    {
        return "Assets/DebugPackage/SavedData/customisation.json";
    }

    /// <summary>
    /// Get File Location Of SavedData Folder
    /// </summary>
    /// <returns>String of file location</returns>
    private static string GetSavedDataLocation()
    {
        return "Assets/DebugPackage/SavedData";
    }

    /// <summary>
    /// Checks if save folder exists, if it doesn't, then it creates it
    /// </summary>
    private static void FolderCheck()
    {
        if (!Directory.Exists(GetSavedDataLocation()))
            Directory.CreateDirectory(GetSavedDataLocation());
    }
}
