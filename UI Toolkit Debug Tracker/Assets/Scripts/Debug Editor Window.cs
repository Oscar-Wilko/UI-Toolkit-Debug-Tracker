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
    private bool showCB;
    private bool showDebugs;
    private Color titleColour;
    private bool resetButton;
    private KeyCode createKey;
    private KeyCode editKey;
    private KeyCode viewKey;
    private bool showUI;

    [MenuItem("Window/Customizer")]
    public static void ShowWindow()
    {
        GetWindow<DebugEditorWindow>("Customizer");
    }

    private void OnGUI()
    {
        InitialiseVariables();
        if (customiser == null || customiser.data == null)
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

        EditorGUILayout.Space(5);

        titleColour = EditorGUILayout.ColorField("Colour of Debug Title", titleColour);
        customiser.data._titleColour = titleColour;

        EditorGUILayout.Space(10);

        showCB = EditorGUILayout.Toggle("Colour Blind Mode", showCB);
        customiser.data._showCB = showCB;

        EditorGUILayout.Space(5);

        showDebugs = EditorGUILayout.Toggle("Show Only In Debug Mode", showDebugs);
        customiser.data._showDebugs = showDebugs;
        
        EditorGUILayout.Space(5);

        showUI = EditorGUILayout.Toggle("Show Tabs UI", showUI);
        customiser.data._showUI = showUI;

        EditorGUILayout.Space(10);

        createKey = (KeyCode)EditorGUILayout.EnumPopup("Keybind To Create Debug", createKey);
        customiser.data._createKey = createKey;
        
        EditorGUILayout.Space(5);

        editKey = (KeyCode)EditorGUILayout.EnumPopup("Keybind To Edit Debug", editKey);
        customiser.data._editKey = editKey;
        
        EditorGUILayout.Space(5);

        viewKey = (KeyCode)EditorGUILayout.EnumPopup("Keybind To View Debug", viewKey);
        customiser.data._viewKey = viewKey;

        EditorGUILayout.Space(10);

        resetButton = GUILayout.Button("Reset To Default");
        if (resetButton)
            ResetToDefault();
    }

    private void InitialiseVariables()
    {
        customiser = FindObjectOfType<DebugCustomiser>();
        if (customiser)
            GetCustomVars(customiser.data);
    }

    private void ResetToDefault()
    {
        customiser.data = new DebugCustomiser.CustomData(customiser.defaultData);
        GetCustomVars(customiser.defaultData);
    }

    private void GetCustomVars(DebugCustomiser.CustomData data)
    {
        iconType = data._iconType;
        iconSize = data._iconSize;
        debugColours = data._debugColours;
        gizmoColour = data._gizmoColour;
        showCB = data._showCB;
        showDebugs = data._showDebugs;
        titleColour = data._titleColour;
        createKey = data._createKey;
        editKey = data._editKey;
        viewKey = data._viewKey;
        showUI = data._showUI;
    }
}
