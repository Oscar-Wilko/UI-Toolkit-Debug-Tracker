using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Popup : MonoBehaviour
{
    [SerializeField] private float duration;
    private float tracker;
    private VisualElement element;
    private VisualElement _root;

    private void Awake()
    {
        tracker = duration;
        _root = GetComponent<UIDocument>().rootVisualElement;
        element = _root.Q<VisualElement>("Window");
        StartCoroutine(Timer());
    }

    /// <summary>
    /// Setup the popup info
    /// </summary>
    /// <param name="title">String of popup title</param>
    /// <param name="desc">String of popup description</param>
    public void SetInfo(string title, string desc)
    {
        Label _title = _root.Q<Label>("Title");
        Label _desc = _root.Q<Label>("Description");
        _title.text = title;
        _desc.text = desc;
    }

    /// <summary>
    /// Destroy object after timer, lowering opacity as it decays
    /// </summary>
    /// <returns>OEnumerator of function</returns>
    private IEnumerator Timer()
    {
        while (tracker > 0)
        {
            element.style.opacity = tracker / duration;
            tracker -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
