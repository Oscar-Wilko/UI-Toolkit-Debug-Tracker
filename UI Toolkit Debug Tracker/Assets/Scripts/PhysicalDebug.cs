using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class PhysicalDebug : MonoBehaviour
{
    public DebugCustomiser _custom;
    public SpriteRenderer _visual;
    public SpriteRenderer _cb_icon;
    public Sprite[] _sprites;
    public Sprite[] _cb_sprites;
    public DebugInstance _data;
    private DebugManager _manager;

    private void Awake()
    {
        _custom = FindObjectOfType<DebugCustomiser>();
    }

    private void Update()
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
        bool active = _custom.data._showDebugs || _manager._debugMode;
        for (int i = 0; i < transform.childCount; i ++)
        {
            transform.GetChild(i).gameObject.SetActive(active);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = _custom.data._gizmoColour;
        Gizmos.DrawSphere(transform.position, 0.75f);
    }
}
