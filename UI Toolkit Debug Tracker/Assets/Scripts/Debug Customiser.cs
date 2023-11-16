using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class DebugCustomiser : MonoBehaviour
{
    public enum IconType
    {
        Circle,
        Box,
        Pillar
    }

    [Serializable]
    public struct CustomColour
    {
        public DebugType type;
        public Color color;
    }

    [Serializable]
    public class CustomData
    {
        public IconType _iconType;
        public Vector3 _iconSize;
        public List<CustomColour> _debugColours;
        public Color _gizmoColour;
        public bool _showCB;
        public bool _showDebugs;
        public Color _titleColour;
        public KeyCode _createKey;
        public KeyCode _editKey;
        public KeyCode _viewKey;
        public bool _showUI;

        public CustomData(CustomData data)
        {
            _iconType = data._iconType; 
            _iconSize = data._iconSize;
            _debugColours = new List<CustomColour>(data._debugColours);
            _gizmoColour = data._gizmoColour;
            _showCB = data._showCB;
            _showDebugs = data._showDebugs;
            _titleColour = data._titleColour;
            _createKey = data._createKey;
            _editKey = data._editKey;
            _viewKey = data._viewKey;
            _showUI = data._showUI;
        }
    }
    public CustomData data;
    public CustomData defaultData;

    private void Awake()
    {
        CustomData in_data = SaveSystem.LoadCustomisation();
        if (in_data != null) 
            data = in_data;
    }

    private void OnDestroy()
    {
        SaveSystem.SaveCustomisation(data);
    }

    public Color ColourOfType(DebugType type)
    {
        foreach(CustomColour col in data._debugColours)
        {
            if (col.type == type)
                return col.color;
        }
        return Color.white;
    }
}
