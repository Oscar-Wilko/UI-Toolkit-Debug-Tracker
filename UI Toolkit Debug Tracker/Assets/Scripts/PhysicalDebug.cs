using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PhysicalDebug : MonoBehaviour
{
    public DebugCustomiser _custom;
    public SpriteRenderer _renderer;
    public Sprite[] _sprites;
    public DebugInstance _data;

    private void Awake()
    {
        _custom = FindObjectOfType<DebugCustomiser>();
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        RefreshAll();
    }

    public void GenerateInstance(DebugInstance data)
    {
        _data = data;
        transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        RefreshColour();
    }

    private void RefreshAll()
    {
        RefreshColour();
        RefreshIcon();
    }

    private void RefreshColour()
    {
        Color custom_colour = _custom.ColourOfType(_data.type);
        _renderer.color = custom_colour;
    }

    private void RefreshIcon()
    {
        _renderer.sprite = _sprites[(int)_custom.data._iconType];
        _renderer.transform.localScale = _custom.data._iconSize;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = _custom.data._gizmoColour;
        Gizmos.DrawSphere(transform.position, 0.75f);
    }
}
