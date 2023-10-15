using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugManager : MonoBehaviour
{
    [System.Serializable]
    public struct DebugInstance
    {
        public DebugType type;
        public string title;
        public string description;
        public DateTime date;
    }

    public enum DebugType
    {
        Fatal,
        Risk,
        Warning,
        Improvement,
        Design
    }

    public List<DebugInstance> _loggedDebugs = new List<DebugInstance>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
