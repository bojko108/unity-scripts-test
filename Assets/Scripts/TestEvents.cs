using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestEvents : MonoBehaviour
{
    private UnityAction someListener;

    private void Awake()
    {
        this.someListener = new UnityAction(this.SomeFunction);
    }

    private void OnEnable()
    {
        EventManager.On("start", this.someListener);
    }

    private void OnDisable()
    {
        EventManager.Off("start", this.someListener);
    }

    private void SomeFunction()
    {
        Debug.Log("sadsadasd");
    }
}
