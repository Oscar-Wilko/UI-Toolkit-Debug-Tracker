using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DebugEditorWindow : EditorWindow
{
    private DebugCustomiser customiser;
    private DebugCustomiser.IconType iconType;
    private List<DebugCustomiser.CustomColour> debugColours;
    private Color gizmoColour;
    private Vector3 iconSize;

    [MenuItem("Window/Customizer")]
    public static void ShowWindow()
    {
        GetWindow<DebugEditorWindow>("Customizer");
    }

    private void OnGUI()
    {
        InitialiseVariables();
        if (customiser == null)
        {
            GUILayout.Label("No Debug Customiser Found In Scene. Check If Debug Prefab Is In Hierarchy.");
            return;
        }

        EditorGUILayout.LabelField("Style And Size Of Icons", EditorStyles.whiteLargeLabel);

        iconType = (DebugCustomiser.IconType)EditorGUILayout.EnumPopup("Icon Type For Debugs", iconType);
        customiser.data._iconType = iconType;

        EditorGUILayout.Space(10);

        iconSize = EditorGUILayout.Vector3Field("Icon Size For Debugs", iconSize);
        customiser.data._iconSize = iconSize;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Colour Of Debug Icons", EditorStyles.whiteLargeLabel);

        for (int i = 0; i < debugColours.Count; i ++)
        {
            DebugCustomiser.CustomColour new_col = debugColours[i];
            new_col.color = EditorGUILayout.ColorField("Colour Of " + new_col.type + " Debugs", new_col.color);
            debugColours[i] = new_col;
            customiser.data._debugColours[i] = new_col;
            EditorGUILayout.Space(5);
        }

        EditorGUILayout.Space(10);

        gizmoColour = EditorGUILayout.ColorField("Colour Of Debug Gizmos", gizmoColour);
        customiser.data._gizmoColour = gizmoColour;
    }

    private void InitialiseVariables()
    {
        customiser = FindObjectOfType<DebugCustomiser>();
        iconType = customiser.data._iconType;
        iconSize = customiser.data._iconSize;
        debugColours = customiser.data._debugColours;
        gizmoColour = customiser.data._gizmoColour;
    }
}
