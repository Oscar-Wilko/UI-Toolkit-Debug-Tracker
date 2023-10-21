using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DebugAdder : MonoBehaviour
{
    private UIDocument _doc;
    public DebugManager _manager;
    public Transform _target;
    // Reference Elements
    private VisualElement _root;
    private Button _reset_button;
    private Button _generate_button;
    private EnumField _debug_type;
    private TextField _debug_title;
    private TextField _debug_message;
    // Reference Strings
    const string _ref_reset_button      = "ResetButton";
    const string _ref_generate_button   = "GenerateButton";
    const string _ref_debug_type        = "DebugEnum";
    const string _ref_debug_title       = "DebugTitle";
    const string _ref_debug_message     = "DebugMessage";

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        GetUIReferences();
        AssignButtonCallbacks();
        ResetButton(null);
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
        _debug_title        = _root.Q<TextField>(_ref_debug_title);
        _debug_message      = _root.Q<TextField>(_ref_debug_message);
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
        _debug_title.SetValueWithoutNotify("Type Title Here...");
        _debug_message.SetValueWithoutNotify("Type Message Here...");
    }

    /// <summary>
    /// Geneate a debug instance from ui references
    /// </summary>
    /// <param name="_event">ClickEvent of button</param>
    private void GenerateButton(ClickEvent _event)
    {
        Debug.Log("Generate: " + _debug_type?.value.ToString() + ", " + _debug_title?.value + ", " + _debug_message?.value + ", " + DateTime.Now.ToString());
        _manager.AddNewDebug(new DebugInstance(
            (DebugType)_debug_type?.value, 
            _debug_title?.value, 
            _debug_message?.value, 
            DateTime.Now, 
            new float[] { _target.position.x, _target.position.y, _target.position.z }, 
            SceneManager.GetActiveScene().name));
        ResetButton(null);
    }
}
