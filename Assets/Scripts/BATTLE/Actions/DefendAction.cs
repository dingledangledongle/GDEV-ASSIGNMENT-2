using System;
using UnityEngine;

public class DefendAction : MonoBehaviour
{
    private int energyCost = 1;
    public static event Action OnDefend;
    public static event Action<int> OnAfterDef;

    private void OnMouseDown()
    {
        OnDefend?.Invoke();
        OnAfterDef?.Invoke(energyCost);
    }
}
