using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExecuteEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent[] events;
    void TriggerEvent(int index)
    {
        events[index].Invoke();
    }
}
