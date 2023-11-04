using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDebug : MonoBehaviour
{
    public SpriteRenderer _renderer;
    public DebugInstance _data;

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }

    public void GenerateInstance(DebugInstance data)
    {
        _data = data;
        transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        switch(data.type)
        {
            case DebugType.Fatal:       _renderer.color = Color.red; break;
            case DebugType.Risk:        _renderer.color = Color.yellow; break;
            case DebugType.Warning:     _renderer.color = Color.white; break;
            case DebugType.Improvement: _renderer.color = Color.blue; break;
            case DebugType.Design:      _renderer.color = Color.green; break;
            default:                    _renderer.color = Color.black; break;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, 0.75f);
    }
}
