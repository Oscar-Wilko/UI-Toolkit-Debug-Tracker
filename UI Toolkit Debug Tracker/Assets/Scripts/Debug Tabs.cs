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
    public DebugViewer _viewer;
    public DebugEditor _editor;
    // Documents
    public UIDocument _createDoc;
    public UIDocument _editDoc;
    public UIDocument _viewDoc;
    private UIDocument _doc;
    // Reference Elements
    private VisualElement _root;
    private Button _create_button;
    private Button _edit_button;
    private Button _view_button;
    // Reference Strings
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

    public void SelectTab(Tabs tab)
    {
        if (tab == _selectedTab) tab = Tabs.None;
        _selectedTab = tab;
        _createDoc.rootVisualElement.visible= tab == Tabs.Create;
        _editDoc.rootVisualElement.visible = tab == Tabs.Edit;
        _viewDoc.rootVisualElement.visible = tab == Tabs.View;
        _viewer.GenerateList();
        _editor.Refresh();
    }
}
