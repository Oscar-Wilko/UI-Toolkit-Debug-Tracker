using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DebugViewer : MonoBehaviour
{
    // References
    private UIDocument _doc;
    public VisualTreeAsset _viewDocument;
    public DebugTabs _tabs;
    public DebugManager _manager;
    public DebugEditor _editor;

    // UI References
    private VisualElement _scrollview;
    private Toggle _filterFatal;
    private Toggle _filterRisk;
    private Toggle _filterWarning;
    private Toggle _filterImprovement;
    private Toggle _filterDesign;
    private Toggle _filterASAP;
    private Toggle _filterSoon;
    private Toggle _filterLater;
    private Toggle _filterWhenever;
    private EnumField _sort;

    // UI Const String References
    private const string r_sort = "Sort";
    private const string r_filter_fa = "FilterFatal";
    private const string r_filter_ri = "FilterRisk";
    private const string r_filter_wa = "FilterWarning";
    private const string r_filter_im = "FilterImprovement";
    private const string r_filter_de = "FilterDesign";
    private const string r_filter_as = "FilterASAP";
    private const string r_filter_so = "FilterSoon";
    private const string r_filter_la = "FilterLater";
    private const string r_filter_wh = "FilterWhenever";
    private const string r_scrollview = "DebugScroll";
    private const string r_select_button = "ViewSelect";
    private const string r_delete_button = "ViewDelete";
    private const string r_title_value = "TitleValue";
    private const string r_type_value = "TypeValue";

    private List<FilterInstance> _filterList = new List<FilterInstance>();
    private SortChoices _prevSort;

    private struct FilterInstance
    {
        public VisualElement elm;
        public DebugInstance debug;
        public FilterInstance(VisualElement _elm, DebugInstance _debug)
        {
            elm = _elm;
            debug = _debug;
        }
    }

    public enum SortChoices
    {
        Type,
        Urgency,
        Newest,
        Oldest,
        Author
    }

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        GetUIReferences();
    }

    private void Update()
    {
        CheckClick();
        if ((SortChoices)_sort.value != _prevSort)
            SortList();
        FilterList();
    }

    /// <summary>
    /// Check If Player Clicked On Debug
    /// </summary>
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
        _scrollview = _root.Q<VisualElement>(r_scrollview);
        _sort = _root.Q<EnumField>(r_sort);
        _filterFatal = _root.Q<Toggle>(r_filter_fa);
        _filterRisk = _root.Q<Toggle>(r_filter_ri);
        _filterWarning = _root.Q<Toggle>(r_filter_wa);
        _filterImprovement = _root.Q<Toggle>(r_filter_im);
        _filterDesign = _root.Q<Toggle>(r_filter_de);
        _filterASAP = _root.Q<Toggle>(r_filter_as);
        _filterSoon = _root.Q<Toggle>(r_filter_so);
        _filterLater = _root.Q<Toggle>(r_filter_la);
        _filterWhenever = _root.Q<Toggle>(r_filter_wh);
    }

    /// <summary>
    /// Generate visual list of all debugs
    /// </summary>
    public void GenerateList()
    {
        _scrollview.Clear();
        _filterList.Clear();
        List<DebugInstance> debugs = _manager.GetDebugsInScene();

        for(int i = 0; i < debugs.Count; i ++)
        {
            VisualElement el = GenerateVisual(debugs[i], i);
            _scrollview.Add(el);
            _filterList.Add(new FilterInstance(el, debugs[i]));
        }

        FilterList();
        SortList();
    }

    /// <summary>
    /// Filter all view instances based on current filter
    /// </summary>
    private void FilterList()
    {
        foreach(FilterInstance inst in _filterList)
            inst.elm.style.display = 
                FilterCheck(inst.debug) ? 
                DisplayStyle.Flex : 
                DisplayStyle.None;
        _prevSort = (SortChoices)_sort.value;
    }
    
    /// <summary>
    /// Check if debug instance passes filter check
    /// </summary>
    /// <param name="inst">DebugInstance of debug to check</param>
    /// <returns>Bool if debug passes filters</returns>
    private bool FilterCheck(DebugInstance inst)
    {
        switch (inst.type)
        {
            case DebugType.Fatal:
                if (!_filterFatal.value)
                    return false;
                break;
            case DebugType.Risk:
                if (!_filterRisk.value)
                    return false;
                break;
            case DebugType.Warning:
                if (!_filterWarning.value)
                    return false;
                break;
            case DebugType.Improvement:
                if (!_filterImprovement.value)
                    return false;
                break;
            case DebugType.Design:
                if (!_filterDesign.value)
                    return false;
                break;
        }
        switch (inst.urgency)
        {
            case UrgencyType.ASAP:
                if (!_filterASAP.value)
                    return false;
                break;
            case UrgencyType.Soon:
                if (!_filterSoon.value)
                    return false;
                break;
            case UrgencyType.Later:
                if (!_filterLater.value)
                    return false;
                break;
            case UrgencyType.Whenever:
                if (!_filterWhenever.value)
                    return false;
                break;
        }
        return true;
    }

    /// <summary>
    /// Sort view instances based on sort type
    /// </summary>
    private void SortList()
    {
        switch ((SortChoices)_sort.value)
        {
            case SortChoices.Type:
                _filterList.Sort(CompareDebugByType); break;
            case SortChoices.Urgency:
                _filterList.Sort(CompareDebugByUrgency); break;
            case SortChoices.Newest:
                _filterList.Sort(CompareDebugByNew); break;
            case SortChoices.Oldest:
                _filterList.Sort(CompareDebugByOld); break;
            case SortChoices.Author:
                _filterList.Sort(CompareDebugByAuthor); break;
        }

        foreach(FilterInstance inst in _filterList)
            inst.elm.SendToBack();
    }

    /// <summary>
    /// Compare to debug instances by their debug type
    /// </summary>
    /// <param name="x">First FilterInstance</param>
    /// <param name="y">Second FilterInstance</param>
    /// <returns>Int of comparisson value</returns>
    private static int CompareDebugByType(FilterInstance x, FilterInstance y)
    {
        DebugType t_x = x.debug.type;
        DebugType t_y = y.debug.type;
        if (t_x == t_y)
            return 0;
        else if (t_x < t_y)
            return 1;
        else if (t_x > t_y)
            return -1;
        return 0;
    }

    /// <summary>
    /// Compare to debug instances by their urgency type
    /// </summary>
    /// <param name="x">First FilterInstance</param>
    /// <param name="y">Second FilterInstance</param>
    /// <returns>Int of comparisson value</returns>
    private static int CompareDebugByUrgency(FilterInstance x, FilterInstance y)
    {
        UrgencyType t_x = x.debug.urgency;
        UrgencyType t_y = y.debug.urgency;
        if (t_x == t_y)
            return 0;
        else if (t_x < t_y)
            return 1;
        else if (t_x > t_y)
            return -1;

        return 0;
    }

    /// <summary>
    /// Compare to debug instances by how new they are
    /// </summary>
    /// <param name="x">First FilterInstance</param>
    /// <param name="y">Second FilterInstance</param>
    /// <returns>Int of comparisson value</returns>
    private static int CompareDebugByNew(FilterInstance x, FilterInstance y)
    {
        DateTime t_x = DateTime.Parse(x.debug.date).ToUniversalTime();
        DateTime t_y = DateTime.Parse(y.debug.date).ToUniversalTime();
        if (t_x == t_y)
            return 0;
        else if (t_x < t_y)
            return -1;
        else if (t_x > t_y)
            return 1;

        return 0;
    }

    /// <summary>
    /// Compare to debug instances by how old they are
    /// </summary>
    /// <param name="x">First FilterInstance</param>
    /// <param name="y">Second FilterInstance</param>
    /// <returns>Int of comparisson value</returns>
    private static int CompareDebugByOld(FilterInstance x, FilterInstance y)
    {
        DateTime t_x = DateTime.Parse(x.debug.date).ToUniversalTime();
        DateTime t_y = DateTime.Parse(y.debug.date).ToUniversalTime();
        if (t_x == t_y)
            return 0;
        else if (t_x < t_y)
            return 1;
        else if (t_x > t_y)
            return -1;

        return 0;
    }

    /// <summary>
    /// Compare to debug instances by their author's name alphabetically
    /// </summary>
    /// <param name="x">First FilterInstance</param>
    /// <param name="y">Second FilterInstance</param>
    /// <returns>Int of comparisson value</returns>
    private static int CompareDebugByAuthor(FilterInstance x, FilterInstance y)
    {
        return string.Compare(y.debug.author, x.debug.author);
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

    /// <summary>
    /// Delete instance of a debug
    /// </summary>
    /// <param name="data">DebugInstance of debug to delete</param>
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
        visual.Q<Button>(r_select_button).clicked += () => { SelectViewer(index);  };
        visual.Q<Button>(r_delete_button).clicked += () => { DeleteViewer(data);  };
        visual.Q<Label>(r_title_value).text = data.title;
        visual.Q<Label>(r_type_value).text = data.type.ToString();
        return visual;
    }
}
