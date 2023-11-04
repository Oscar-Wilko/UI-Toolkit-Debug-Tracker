using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DebugEditor : MonoBehaviour
{
    private UIDocument _doc;
    [SerializeField] private GameObject popup;
    public DebugManager _manager;
    private int _currentIndex = -1;
    // Reference Elements
    private VisualElement _root;
    private VisualElement _info_element;
    private Button _reset_button;
    private Button _save_button;
    private EnumField _debug_type;
    private EnumField _debug_urgency;
    private TextField _debug_title;
    private TextField _debug_message;
    private DropdownField _debug_select;
    private TextField _debug_author;
    private TextField _debug_machine;
    // Reference Strings
    const string _ref_reset_button = "ResetButton";
    const string _ref_save_button = "SaveButton";
    const string _ref_debug_type = "DebugEnum";
    const string _ref_debug_title = "DebugTitle";
    const string _ref_debug_message = "DebugMessage";
    const string _ref_debug_select = "DebugSelect";
    const string _ref_info_element = "BottomElement";
    const string _ref_debug_urgency = "UrgencyEnum";
    const string _ref_debug_author = "DebugAuthor";
    const string _ref_debug_machine = "MachineInfo";

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        GetUIReferences();
        AssignButtonCallbacks();
    }

    private void Update()
    {
        if (_currentIndex != _debug_select.index && _debug_select.index != -1) SelectDebug(_debug_select.index);
    }

    /// <summary>
    /// Gather all ui references from reference strings
    /// </summary>
    private void GetUIReferences()
    {
        _root           = _doc.rootVisualElement;
        _reset_button   = _root.Q<Button>(_ref_reset_button);
        _save_button    = _root.Q<Button>(_ref_save_button);
        _debug_type     = _root.Q<EnumField>(_ref_debug_type);
        _debug_urgency  = _root.Q<EnumField>(_ref_debug_urgency);
        _debug_title    = _root.Q<TextField>(_ref_debug_title);
        _debug_message  = _root.Q<TextField>(_ref_debug_message);
        _debug_select   = _root.Q<DropdownField>(_ref_debug_select);
        _info_element   = _root.Q<VisualElement>(_ref_info_element);
        _debug_author   = _root.Q<TextField>(_ref_debug_author);
        _debug_machine  = _root.Q<TextField>(_ref_debug_machine);
        _info_element.visible = false;
    }

    /// <summary>
    /// Assign all button events
    /// </summary>
    private void AssignButtonCallbacks()
    {
        _reset_button?.RegisterCallback<ClickEvent>(ResetButton);
        _save_button?.RegisterCallback<ClickEvent>(SaveButton);
    }

    /// <summary>
    /// Reset ui elements to original state
    /// </summary>
    /// <param name="_event">ClickEvent of button</param>
    private void ResetButton(ClickEvent _event)
    {
        DebugInstance data = _manager.GetDebugs()[_currentIndex];
        _debug_type.SetValueWithoutNotify(data.type);
        _debug_urgency.SetValueWithoutNotify(data.urgency);
        _debug_title.SetValueWithoutNotify(data.title);
        _debug_message.SetValueWithoutNotify(data.description);
    }

    /// <summary>
    /// Select debug item of specfic index
    /// </summary>
    /// <param name="index">Int of debug index</param>
    private void SelectDebug(int index)
    {
        DebugInstance data = _manager.GetDebugs()[index];
        _currentIndex = index;
        _info_element.visible = true;
        _debug_type.SetValueWithoutNotify(data.type);
        _debug_urgency.SetValueWithoutNotify(data.urgency);
        _debug_title.SetValueWithoutNotify(data.title);
        _debug_message.SetValueWithoutNotify(data.description);
        _debug_author.SetValueWithoutNotify(data.author);
        _debug_machine.SetValueWithoutNotify(data.machine);
    }

    /// <summary>
    /// External reference to change selected debug
    /// </summary>
    /// <param name="index">Int of debug index</param>
    public void ViewSelectDebug(int index)
    {
        _debug_select.index = index;
        SelectDebug(index);
    }

    /// <summary>
    /// Save debug instance changes from ui references
    /// </summary>
    /// <param name="_event">ClickEvent of button</param>
    private void SaveButton(ClickEvent _event)
    {
        if (!Validate()) return;
        DebugInstance old_data = _manager.GetDebugs()[_currentIndex];
        DebugInstance new_data = new DebugInstance(
            (DebugType)_debug_type.value,
            _debug_title.value, 
            _debug_message.value, 
            old_data.date, 
            old_data.position, 
            old_data.scene,
            (UrgencyType)_debug_urgency.value,
            old_data.author,
            old_data.machine);
        _manager.ReplaceDebug(old_data, new_data);
        _debug_select.choices = _manager.GetTitles();
    }

    public void Refresh()
    {
        _debug_select.choices = _manager.GetTitles();
        _debug_select.index = -1;
        _currentIndex = -1;
        _info_element.visible = false;
    }

    private bool Validate()
    {
        if (_debug_title?.value == "")
        {
            Popup inst = Instantiate(popup, transform.parent).GetComponent<Popup>();
            inst.SetInfo("Invalid Title", "You cannot have an empty title.");
            return false;
        }
        else if (_debug_message?.value == "")
        {
            Popup inst = Instantiate(popup, transform.parent).GetComponent<Popup>();
            inst.SetInfo("Invalid Message", "You cannot have an empty message.");
            return false;
        }
        else if (_debug_author?.value == "")
        {
            Popup inst = Instantiate(popup, transform.parent).GetComponent<Popup>();
            inst.SetInfo("Invalid Author", "You cannot have an empty author.");
            return false;
        }
        return true;
    }
}
