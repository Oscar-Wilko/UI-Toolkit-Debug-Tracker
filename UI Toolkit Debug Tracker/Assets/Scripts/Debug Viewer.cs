using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DebugViewer : MonoBehaviour
{
    private UIDocument _doc;
    public DebugTabs _tabs;
    public VisualTreeAsset _viewDocument;
    public DebugManager _manager;
    public DebugEditor _editor;
    private VisualElement _scrollview;
    private const string _ref_scrollview = "DebugScroll";
    private const string _ref_select_button = "ViewSelect";
    private const string _ref_delete_button = "ViewDelete";
    private const string _ref_title_value = "TitleValue";
    private const string _ref_type_value = "TypeValue";

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        GetUIReferences();
    }

    private void Update()
    {
        CheckClick();
    }

    private void CheckClick()
    {
        if (!_manager._debugMode || !Input.GetMouseButtonDown(0) || EventSystem.current.IsPointerOverGameObject())
            return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 500.0f))
        {
            PhysicalDebug debug = hit.transform.GetComponent<PhysicalDebug>();
            if (debug)
            {
                if (_tabs._selectedTab != DebugTabs.Tabs.Edit)
                    _tabs.SelectTab(DebugTabs.Tabs.Edit);
                _editor.ViewSelectDebug(_manager.GetDebugs().IndexOf(debug._data));
            }
        }
    }

    /// <summary>
    /// Gather all ui references from reference strings
    /// </summary>
    private void GetUIReferences()
    {
        VisualElement _root = _doc.rootVisualElement;
        _scrollview = _root.Q<VisualElement>(_ref_scrollview);
    }

    /// <summary>
    /// Generate visual list of all debugs
    /// </summary>
    public void GenerateList()
    {
        _scrollview.Clear();
        List<DebugInstance> debugs = _manager.GetDebugs();

        for(int i = 0; i < debugs.Count; i ++)
        {
            VisualElement el = GenerateVisual(debugs[i], i);
            _scrollview.Add(el);
        }
    }

    /// <summary>
    /// Button function reference for selecting a debug
    /// </summary>
    /// <param name="index">Index of debug in saved list</param>
    public void SelectViewer(int index)
    {
        _tabs.SelectTab(DebugTabs.Tabs.Edit);
        _editor.ViewSelectDebug(index);
    }

    public void DeleteViewer(DebugInstance data)
    {
        _manager.RemoveDebug(data);
        GenerateList();
    }

    /// <summary>
    /// Create a VisualElement for the debug list
    /// </summary>
    /// <param name="data">DebugInstance of debug data</param>
    /// <param name="index">Index of debug in saved list</param>
    /// <returns>VisualElement of created ui object</returns>
    public VisualElement GenerateVisual(DebugInstance data, int index)
    {
        VisualElement visual = _viewDocument.Instantiate();
        visual.Q<Button>(_ref_select_button).clicked += () => { SelectViewer(index);  };
        visual.Q<Button>(_ref_delete_button).clicked += () => { DeleteViewer(data);  };
        visual.Q<Label>(_ref_title_value).text = data.title;
        visual.Q<Label>(_ref_type_value).text = data.type.ToString();
        return visual;
    }
}
