using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DebugAdder : MonoBehaviour
{
    private UIDocument _doc;
    [SerializeField] private GameObject popup;
    public DebugManager _manager;
    public Transform _target;
    // Reference Elements
    private VisualElement _root;
    private Button _reset_button;
    private Button _generate_button;
    private EnumField _debug_type;
    private EnumField _debug_urgency;
    private TextField _debug_title;
    private TextField _debug_message;
    private TextField _debug_author;
    private TextField _debug_machine;
    // Reference Strings
    const string _ref_reset_button      = "ResetButton";
    const string _ref_generate_button   = "GenerateButton";
    const string _ref_debug_type        = "DebugEnum";
    const string _ref_debug_title       = "DebugTitle";
    const string _ref_debug_message     = "DebugMessage";
    const string _ref_debug_urgency     = "UrgencyEnum";
    const string _ref_debug_author      = "DebugAuthor";
    const string _ref_debug_machine     = "MachineInfo";

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        GetUIReferences();
        AssignButtonCallbacks();
        ResetButton(null);
        _debug_machine.SetValueWithoutNotify(GetMachineInfo());
    }

    /// <summary>
    /// Gather all ui references from reference strings
    /// </summary>
    private void GetUIReferences()
    {
        _root               = _doc.rootVisualElement;
        _reset_button       = _root.Q<Button>(_ref_reset_button);
        _generate_button    = _root.Q<Button>(_ref_generate_button);
        _debug_type         = _root.Q<EnumField>(_ref_debug_type);
        _debug_urgency      = _root.Q<EnumField>(_ref_debug_urgency);
        _debug_title        = _root.Q<TextField>(_ref_debug_title);
        _debug_message      = _root.Q<TextField>(_ref_debug_message);
        _debug_author       = _root.Q<TextField>(_ref_debug_author);
        _debug_machine      = _root.Q<TextField>(_ref_debug_machine);
    }

    /// <summary>
    /// Assign all button events
    /// </summary>
    private void AssignButtonCallbacks()
    {
        _reset_button?.RegisterCallback<ClickEvent>(ResetButton);
        _generate_button?.RegisterCallback<ClickEvent>(GenerateButton);
    }

    /// <summary>
    /// Reset ui elements to original state
    /// </summary>
    /// <param name="_event">ClickEvent of button</param>
    private void ResetButton(ClickEvent _event)
    {
        _debug_type.SetValueWithoutNotify((DebugType)0);
        _debug_title.SetValueWithoutNotify("");
        _debug_message.SetValueWithoutNotify("");
        _debug_urgency.SetValueWithoutNotify((UrgencyType)0);
    }

    private string GetMachineInfo()
    {
        string info = "";
        info += "Device: " + SystemInfo.deviceModel;
        info += "\nOS: " + SystemInfo.operatingSystem;
        info += "\nCPU: " + SystemInfo.processorType;
        info += "\nGPU: " + SystemInfo.graphicsDeviceName;
        info += "\nRAM: " + SystemInfo.systemMemorySize + "MB";
        return info;
    }

    /// <summary>
    /// Geneate a debug instance from ui references
    /// </summary>
    /// <param name="_event">ClickEvent of button</param>
    private void GenerateButton(ClickEvent _event)
    {
        if (!Validate()) return;
        _manager.AddNewDebug(new DebugInstance(
            (DebugType)_debug_type?.value, 
            _debug_title?.value, 
            _debug_message?.value, 
            DateTime.Now.ToString(), 
            new float[] { _target.position.x, _target.position.y, _target.position.z }, 
            SceneManager.GetActiveScene().name,
            (UrgencyType)_debug_urgency?.value,
            _debug_author?.value,
            _debug_machine?.value));
        ResetButton(null);
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
