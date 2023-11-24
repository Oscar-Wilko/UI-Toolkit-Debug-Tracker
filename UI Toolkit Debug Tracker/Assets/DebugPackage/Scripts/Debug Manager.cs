using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class DebugManager : MonoBehaviour
{
    private List<DebugInstance> _loggedDebugs = new List<DebugInstance>();
    [SerializeField] private List<GameObject> _physicalDebugs = new List<GameObject>();
    public GameObject _physicalPrefab;
    public bool _debugMode;

    private void Awake()
    {
        LoadAll();
    }

    private void OnDestroy()
    {
        if (_loggedDebugs.Count > 0)
            SaveSystem.SaveDebugs(_loggedDebugs);
    }

    public void RefreshFromFile()
    {
        LoadAll();
    }

    private void Update()
    {
        CheckDebugToggle();
    }

    /// <summary>
    /// Check if debug mode toggle has been activated
    /// </summary>
    private void CheckDebugToggle()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.E))
            _debugMode = !_debugMode;
    }

    /// <summary>
    /// Remove debug at specific list index
    /// </summary>
    /// <param name="index">Int of index</param>
    public void RemoveDebug(int index)
    {
        _loggedDebugs.RemoveAt(index);
    }

    /// <summary>
    /// Replace an existing debug with a new debug
    /// </summary>
    /// <param name="old_debug">DebugInstance of old debug data</param>
    /// <param name="new_debug">DebugInstance of new debug data</param>
    public void ReplaceDebug(DebugInstance old_debug, DebugInstance new_debug)
    {
        int index = _loggedDebugs.IndexOf(old_debug);
        _loggedDebugs.Insert(index, new_debug);
        _loggedDebugs.Remove(old_debug);
        SaveSystem.SaveDebugs(_loggedDebugs);

        for (int i = 0; i < _physicalDebugs.Count; i++)
        {
            DebugInstance inst = _physicalDebugs[i].GetComponent<PhysicalDebug>()._data;
            if (inst.title != old_debug.title) continue;
            if (inst.type != old_debug.type) continue;
            if (inst.description != old_debug.description) continue;
            if (inst.date != old_debug.date) continue;
            if (inst.scene != old_debug.scene) continue;
            _physicalDebugs[i].GetComponent<PhysicalDebug>()._data = new_debug;
        }
    }

    /// <summary>
    /// Remove specific debug from list
    /// </summary>
    /// <param name="data">DebugInstance of data</param>
    public void RemoveDebug(DebugInstance data)
    {
        _loggedDebugs.Remove(data);
        SaveSystem.SaveDebugs(_loggedDebugs);

        for (int i = 0; i < _physicalDebugs.Count; i ++)
        {
            DebugInstance inst = _physicalDebugs[i].GetComponent<PhysicalDebug>()._data;
            if (inst.title != data.title) continue;
            if (inst.type != data.type) continue;
            if (inst.description != data.description) continue;
            if (inst.date != data.date) continue;
            if (inst.scene != data.scene) continue;
            Destroy(_physicalDebugs[i]);
            _physicalDebugs.RemoveAt(i);
            i++;
        }
    }

    /// <summary>
    /// Add new debug into list
    /// </summary>
    /// <param name="data">DebugInstance of new debug</param>
    public void AddNewDebug(DebugInstance data)
    {
        _loggedDebugs.Add(data);
        CreateInstance(data);
    }

    /// <summary>
    /// Get current debug list
    /// </summary>
    /// <returns>list of DebugInstances</returns>
    public List<DebugInstance> GetDebugs()
    {
        return _loggedDebugs;
    }
    
    /// <summary>
    /// Get current debug list of debugs in current scene
    /// </summary>
    /// <returns>list of DebugInstances</returns>
    public List<DebugInstance> GetDebugsInScene()
    {
        return GetDebugsInScene(SceneManager.GetActiveScene().name);
    }
    
    /// <summary>
    /// Get current debug list of debugs in current scene with scene name
    /// </summary>
    /// <returns>list of DebugInstances</returns>
    public List<DebugInstance> GetDebugsInScene(string name)
    {
        List<DebugInstance> instances = new List<DebugInstance>();
        foreach(DebugInstance inst in _loggedDebugs)
            if (inst.scene == name)
                instances.Add(inst);
        return instances;
    }

    /// <summary>
    /// Get list of titles of all debugs in file
    /// </summary>
    /// <returns>List of strings of titles</returns>
    public List<string> GetTitles()
    {
        List<string> titles = new List<string>();
        foreach(DebugInstance debug in _loggedDebugs)
        {
            titles.Add(debug.title + " " + debug.date);
        }
        return titles;
    }
    
    /// <summary>
    /// Get list of titles in current scene
    /// </summary>
    /// <returns>List of strings of titles</returns>
    public List<string> GetTitlesInScene()
    {
        return GetTitlesInScene(SceneManager.GetActiveScene().name);
    }
    
    /// <summary>
    /// Get list of titles in specific scene
    /// </summary>
    /// <param name="name">String of scene name</param>
    /// <returns>List of string of titles</returns>
    public List<string> GetTitlesInScene(string name)
    {
        List<string> titles = new List<string>();
        foreach(DebugInstance debug in _loggedDebugs)
            if (debug.scene == name)
                titles.Add(debug.title + " " + debug.date);
        return titles;
    }

    /// <summary>
    /// Load all debug instances
    /// </summary>
    private void LoadAll()
    {
        UnloadAll();
        _loggedDebugs = SaveSystem.LoadDebugs();
        if (_loggedDebugs == null) _loggedDebugs = new List<DebugInstance>();
        foreach(DebugInstance debug in _loggedDebugs)
        {
            if (debug.scene != SceneManager.GetActiveScene().name) continue;
            CreateInstance(debug);
        }
    }
    
    /// <summary>
    /// Unload all debug instances
    /// </summary>
    private void UnloadAll()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);
        _physicalDebugs.Clear();
        _loggedDebugs.Clear();
    }

    /// <summary>
    /// Create a visual instance of a debug
    /// </summary>
    /// <param name="debug">DebugInstance of input data</param>
    /// <returns>GameObject of new instance</returns>
    private GameObject CreateInstance(DebugInstance debug)
    {
        GameObject obj = Instantiate(_physicalPrefab, transform);
        obj.GetComponent<PhysicalDebug>().GenerateInstance(debug, this);
        _physicalDebugs.Add(obj);
        return obj;
    }
}
