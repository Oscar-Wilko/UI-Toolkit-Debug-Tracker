using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class PhysicalDebug : MonoBehaviour
{
    // Self References
    public DebugCustomiser _custom;
    public SpriteRenderer _visual;
    public SpriteRenderer _cb_icon;
    public TextMeshProUGUI _title;
    public Sprite[] _sprites;
    public Sprite[] _cb_sprites;

    // Variables
    public float _innerAngleFalloff;
    public float _outerAngleFalloff;

    // References
    public DebugInstance _data;
    private DebugManager _manager;

    private void Awake()
    {
        _custom = FindObjectOfType<DebugCustomiser>();
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position);
        if (_custom.data._lockX)
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        RefreshAll();
        PositionCheck();
    }

    /// <summary>
    /// Generate self with data to compare
    /// </summary>
    /// <param name="data">DebugInstance of debug data</param>
    /// <param name="manager">DebugManager to reference manager</param>
    public void GenerateInstance(DebugInstance data, DebugManager manager)
    {
        _manager = manager;
        _data = data;
        transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        RefreshColour();
    }

    /// <summary>
    /// Check all states to refresh any changes
    /// </summary>
    private void RefreshAll()
    {
        RefreshState();
        RefreshColour();
        RefreshIcon();
        RefreshCBIcon();
        RefreshTitle();
    }

    /// <summary>
    /// Check if debug moves in world position
    /// </summary>
    private void PositionCheck()
    {
        if (!SamePos(transform.position, _data.position))
            RefreshPosData();
    }

    /// <summary>
    /// Checks if a Vector3 is the same as a float array in values
    /// </summary>
    /// <param name="v3">Vector3 of inputed vector</param>
    /// <param name="arr">Float[] of inputed float array</param>
    /// <returns>Bool if their values are the same</returns>
    private bool SamePos(Vector3 v3, float[] arr)
    {
        if (v3.x != arr[0])
            return false;
        if (v3.y != arr[1])
            return false;
        if (v3.z != arr[2])
            return false;
        return true;
    }

    /// <summary>
    /// Refresh the position of debug instance in the manager
    /// </summary>
    private void RefreshPosData()
    {
        DebugInstance n_data = _data;
        n_data.position = new float[3] { transform.position.x, transform.position.y, transform.position.z };
        _manager.ReplaceDebug(_data, n_data);
        _data = n_data;
    }

    /// <summary>
    /// Refresh the colour of object based on debug type
    /// </summary>
    private void RefreshColour()
    {
        Color custom_colour = _custom.ColourOfType(_data.type);
        _visual.color = custom_colour;
        _cb_icon.color = custom_colour;
    }

    /// <summary>
    /// Refresh scale and type of icon
    /// </summary>
    private void RefreshIcon()
    {
        _visual.sprite = _sprites[(int)_custom.data._iconType];
        _visual.transform.localScale = _custom.data._iconSize;
    }

    /// <summary>
    /// Refresh state, scale and type of colourblind icon
    /// </summary>
    private void RefreshCBIcon()
    {
        _cb_icon.enabled = _custom.data._showCB;
        _cb_icon.sprite = _cb_sprites[(int)_data.type];
        _cb_icon.transform.localScale = _custom.data._iconSize * 0.1f;
    }

    /// <summary>
    /// Refresh overall active state of debug
    /// </summary>
    private void RefreshState()
    {
        bool active = !_custom.data._showDebugs || _manager._debugMode;
        for (int i = 0; i < transform.childCount; i ++)
            transform.GetChild(i).gameObject.SetActive(active);
    }

    /// <summary>
    /// Refresh scale, text and colour of debug title
    /// </summary>
    private void RefreshTitle()
    {
        Vector3 diffVec = transform.position - Camera.main.transform.position;
        float scale = Vector3.Angle(Camera.main.transform.forward, diffVec);
        scale = _outerAngleFalloff - scale;
        scale = Mathf.Clamp(scale, 0.0f, _innerAngleFalloff) / _innerAngleFalloff;
        //_title.transform.position = Camera.main.WorldToScreenPoint(transform.position); // For Non World Space Canvas
        _title.transform.localScale = Vector3.one * scale;
        _title.text = _data.title;
        _title.color = _custom.data._titleColour;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = _custom.data._gizmoColour;
        Gizmos.DrawSphere(transform.position, 0.75f);
    }
}
