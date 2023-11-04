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
    }
    public CustomData data;

    private void Awake()
    {
        CustomData in_data = SaveSystem.LoadCustomisation();
        if (in_data != null) data = in_data;
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
