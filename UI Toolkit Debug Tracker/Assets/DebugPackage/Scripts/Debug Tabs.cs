using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugTabs : MonoBehaviour
{    public enum Tabs
    {
        None,
        Create,
        Edit,
        View
    }
    // Debugs
    public DebugCustomiser _custom;
    public DebugAdder _adder;
    public DebugViewer _viewer;
    public DebugEditor _editor;
    // Documents
    public UIDocument _createDoc;
    public UIDocument _editDoc;
    public UIDocument _viewDoc;
    private UIDocument _doc;
    // UI References
    private VisualElement _root;
    private Button _create_button;
    private Button _edit_button;
    private Button _view_button;

    // UI Const String References 
    const string _ref_create_button = "CreateTab";
    const string _ref_edit_button = "EditTab";
    const string _ref_view_button = "ViewTab";

    public Tabs _selectedTab;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        GetUIReferences();
        AssignButtonCallbacks();
    }

    private void Start()
    {
        SelectTab(0);
    }

    private void Update()
    {
        _doc.enabled = _custom.data._showUI;
        if (AltTabKey())
        {
            if (Input.GetKeyDown(_custom.data._createKey))
                SelectTab(Tabs.Create);
            else if (Input.GetKeyDown(_custom.data._editKey))
                SelectTab(Tabs.Edit);
            else if (Input.GetKeyDown(_custom.data._viewKey))
                SelectTab(Tabs.View);
        }
    }

    private bool AltTabKey()
    {
        if (!_custom)
            return false;
        switch (_custom.data._tabKey)
        {
            case AltKeyCode.LeftControl:
                return Input.GetKey(KeyCode.LeftControl);
            case AltKeyCode.RightControl:
                return Input.GetKey(KeyCode.RightControl);
            case AltKeyCode.LeftAlt:
                return Input.GetKey(KeyCode.LeftAlt);
            case AltKeyCode.RightAlt:
                return Input.GetKey(KeyCode.RightAlt);
            case AltKeyCode.LeftShift:
                return Input.GetKey(KeyCode.LeftShift);
            case AltKeyCode.RightShift:
                return Input.GetKey(KeyCode.RightShift);
        }
        return false;
    }

    /// <summary>
    /// Gather all ui references from reference strings
    /// </summary>
    private void GetUIReferences()
    {
        _root           = _doc.rootVisualElement;
        _create_button  = _root.Q<Button>(_ref_create_button);
        _edit_button    = _root.Q<Button>(_ref_edit_button);
        _view_button    = _root.Q<Button>(_ref_view_button);
    }

    /// <summary>
    /// Assign all button events
    /// </summary>
    private void AssignButtonCallbacks()
    {
        _create_button?.RegisterCallback<ClickEvent>(SelectCreate);
        _edit_button?.RegisterCallback<ClickEvent>(SelectEdit);
        _view_button?.RegisterCallback<ClickEvent>(SelectView);
    }

    private void SelectCreate(ClickEvent evt) { SelectTab(Tabs.Create); }
    private void SelectEdit(ClickEvent evt) { SelectTab(Tabs.Edit); }
    private void SelectView(ClickEvent evt) { SelectTab(Tabs.View); }

    /// <summary>
    /// Select a tab
    /// </summary>
    /// <param name="tab">Tabs Enum of chosen tab</param>
    public void SelectTab(Tabs tab)
    {
        if (tab == _selectedTab) 
            tab = Tabs.None;
        _selectedTab = tab;
        _createDoc.rootVisualElement.visible= tab == Tabs.Create;
        _editDoc.rootVisualElement.visible = tab == Tabs.Edit;
        _viewDoc.rootVisualElement.visible = tab == Tabs.View;
        _viewer.GenerateList();
        _editor.Refresh();
    }
}
