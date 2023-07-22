using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendAction : MonoBehaviour
{
    public static event Action OnDefend;

    private void OnMouseDown()
    {
        OnDefend?.Invoke();
    }
}
