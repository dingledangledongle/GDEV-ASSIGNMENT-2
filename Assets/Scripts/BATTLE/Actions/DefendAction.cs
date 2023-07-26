using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefendAction : MonoBehaviour, IPointerDownHandler
{
    private int energyCost = 1;
    public static event Action OnDefend;
    public static event Action<int> OnAfterDef;

    public void OnPointerDown(PointerEventData data)
    {
        OnDefend?.Invoke();
        OnAfterDef?.Invoke(energyCost);
    }
}
