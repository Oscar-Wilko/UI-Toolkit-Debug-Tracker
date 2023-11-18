using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class DebugCustomiserWindow : EditorWindow
{
    // Refs
    private DebugCustomiser customiser;

    // UI
    [SerializeField] private VisualTreeAsset _root;
    private EnumField _iconType;
    private Vector3Field _iconSize;
    private List<ColorField> _debugColours = new List<ColorField>();
    private ColorField _gizmoColour;
    private ColorField _titleColour;
    private Toggle _showCB;
    private Toggle _showDebugs;
    private Toggle _showUI;
    private EnumField _createKey;
    private EnumField _editKey;
    private EnumField _viewKey;
    private Button _resetButton;

    private const string r_iconType = "IconType";
    private const string r_iconSize = "IconSize";
    private string[] r_debugColours = { "FatalColour" , "RiskColour" , "WarningColour" , "ImpColour" , "DesignColour"  };
    private const string r_gizmoColour = "GizmoColour";
    private const string r_titleColour = "TitleColour";
    private const string r_showCB = "ShowCB";
    private const string r_showDebugs = "ShowDebugs";
    private const string r_showUI = "ShowUI";
    private const string r_createKey = "CreateKey";
    private const string r_editKey = "EditKey";
    private const string r_viewKey = "ViewKey";
    private const string r_resetButton = "ResetButton";

    [MenuItem("Debug/Customiser")]
    public static void ShowEditor()
    {
        var window = GetWindow<DebugCustomiserWindow>();
        window.titleContent = new GUIContent("Customiser");
    }

    private void CreateGUI()
    {
        GetUIReferences();
        RegisterCallbackEvents();
        InitialiseVariables();
    }

    private void OnGUI()
    {
        SetCustomVars(customiser?.data);
    }

    private void RegisterCallbackEvents()
    {
        _resetButton?.RegisterCallback<ClickEvent>(ResetToDefault);
    }

    private void GetUIReferences()
    {
        _root.CloneTree(rootVisualElement);

        _iconType = rootVisualElement.Q<EnumField>(r_iconType);
        _iconSize = rootVisualElement.Q<Vector3Field>(r_iconSize);
        _debugColours.Clear();
        for(int i = 0; i < r_debugColours.Length; i++)
            _debugColours.Add(rootVisualElement.Q<ColorField>(r_debugColours[i]));
        _gizmoColour = rootVisualElement.Q<ColorField>(r_gizmoColour);
        _titleColour = rootVisualElement.Q<ColorField>(r_titleColour);
        _showCB = rootVisualElement.Q<Toggle>(r_showCB);
        _showDebugs = rootVisualElement.Q<Toggle>(r_showDebugs);
        _showUI = rootVisualElement.Q<Toggle>(r_showUI);
        _createKey = rootVisualElement.Q<EnumField>(r_createKey);
        _editKey = rootVisualElement.Q<EnumField>(r_editKey);
        _viewKey = rootVisualElement.Q<EnumField>(r_viewKey);
        _resetButton = rootVisualElement.Q<Button>(r_resetButton);
    }

    private void InitialiseVariables()
    {
        customiser = FindObjectOfType<DebugCustomiser>();
        if (!customiser)
            return;
        GetCustomVars(customiser.data);
    }

    private void ResetToDefault(ClickEvent evt)
    {
        customiser.data = new DebugCustomiser.CustomData(customiser.defaultData);
        GetCustomVars(customiser.defaultData);
    }

    private void GetCustomVars(DebugCustomiser.CustomData data)
    {
        _iconType.value = data._iconType;
        _iconSize.value = data._iconSize;
        for(int i = 0; i < data._debugColours.Count; i ++)
            _debugColours[i].value = data._debugColours[i].color;
        _gizmoColour.value = data._gizmoColour;
        _showCB.value = data._showCB;
        _showDebugs.value = data._showDebugs;
        _titleColour.value = data._titleColour;
        _createKey.value = data._createKey;
        _editKey.value = data._editKey;
        _viewKey.value = data._viewKey;
        _showUI.value = data._showUI;
    }

    private void SetCustomVars(DebugCustomiser.CustomData data)
    {
        if (data == null || _iconType == null)
            return;
        data._iconType = (DebugCustomiser.IconType)_iconType.value;
        data._iconSize = _iconSize.value;
        for (int i = 0; i < data._debugColours.Count; i++)
        {
            DebugCustomiser.CustomColour new_col = data._debugColours[i];
            new_col.color = _debugColours[i].value;
            data._debugColours[i] = new_col;
        }
        data._gizmoColour = _gizmoColour.value;
        data._showCB = _showCB.value;
        data._showDebugs = _showDebugs.value;
        data._titleColour = _titleColour.value;
        data._createKey = (KeyCode)_createKey.value;
        data._editKey = (KeyCode)_editKey.value;
        data._viewKey = (KeyCode)_viewKey.value;
        data._showUI = _showUI.value;
    }
}
