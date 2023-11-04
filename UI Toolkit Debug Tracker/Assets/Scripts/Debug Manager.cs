using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class DebugManager : MonoBehaviour
{
    private List<DebugInstance> _loggedDebugs = new List<DebugInstance>();
    [SerializeField] private List<GameObject> _physicalDebugs = new List<GameObject>();
    public GameObject _physicalPrefab;

    private void Awake()
    {
        LoadAll();
    }

    private void OnDestroy()
    {
        SaveSystem.SaveDebugs(_loggedDebugs);
    }

    private void Update()
    {

    }

    private void OutputDebugs()
    {
        foreach(DebugInstance debug in _loggedDebugs)
        {
            Debug.Log(
                "Debug Output: " + 
                debug.type + " " + 
                debug.title + " " + 
                debug.description + " " + 
                debug.date);
        }
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
        for(int i = 0; i < _physicalDebugs.Count; i ++)
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

        _loggedDebugs.Remove(data);
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
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
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
        obj.GetComponent<PhysicalDebug>().GenerateInstance(debug);
        _physicalDebugs.Add(obj);
        return obj;
    }
}
