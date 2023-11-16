using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class PhysicalDebug : MonoBehaviour
{
    public DebugCustomiser _custom;
    public SpriteRenderer _visual;
    public SpriteRenderer _cb_icon;
    public TextMeshProUGUI _title;
    public Sprite[] _sprites;
    public Sprite[] _cb_sprites;
    public float _innerAngleFalloff;
    public float _outerAngleFalloff;
    public DebugInstance _data;
    private DebugManager _manager;

    private void Awake()
    {
        _custom = FindObjectOfType<DebugCustomiser>();
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position);
        RefreshAll();
        PositionCheck();
    }

    public void GenerateInstance(DebugInstance data, DebugManager manager)
    {
        _manager = manager;
        _data = data;
        transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        RefreshColour();
    }

    private void RefreshAll()
    {
        RefreshState();
        RefreshColour();
        RefreshIcon();
        RefreshCBIcon();
        RefreshTitle();
    }

    private void PositionCheck()
    {
        if (!SamePos(transform.position, _data.position))
        {
            RefreshPosData();
        }
    }

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

    private void RefreshPosData()
    {
        DebugInstance n_data = _data;
        n_data.position = new float[3] { transform.position.x, transform.position.y, transform.position.z };
        _manager.ReplaceDebug(_data, n_data);
        _data = n_data;
    }

    private void RefreshColour()
    {
        Color custom_colour = _custom.ColourOfType(_data.type);
        _visual.color = custom_colour;
        _cb_icon.color = custom_colour;
    }

    private void RefreshIcon()
    {
        _visual.sprite = _sprites[(int)_custom.data._iconType];
        _visual.transform.localScale = _custom.data._iconSize;
    }

    private void RefreshCBIcon()
    {
        _cb_icon.enabled = _custom.data._showCB;
        _cb_icon.sprite = _cb_sprites[(int)_data.type];
        _cb_icon.transform.localScale = _custom.data._iconSize * 0.1f;
    }

    private void RefreshState()
    {
        bool active = !_custom.data._showDebugs || _manager._debugMode;
        for (int i = 0; i < transform.childCount; i ++)
        {
            transform.GetChild(i).gameObject.SetActive(active);
        }
    }

    private void RefreshTitle()
    {
        Vector3 diffVec = transform.position - Camera.main.transform.position;
        float scale = Vector3.Angle(Camera.main.transform.forward, diffVec);
        scale = _outerAngleFalloff - scale;
        scale = Mathf.Clamp(scale, 0.0f, _innerAngleFalloff) / _innerAngleFalloff;
        //_title.transform.position = Camera.main.WorldToScreenPoint(transform.position); // For Non World Space Correction
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
