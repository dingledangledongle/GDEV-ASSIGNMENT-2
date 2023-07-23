using System;
using UnityEngine;
public class AttackAction : MonoBehaviour
{
    public GameObject ArrowPrefab;
    private GameObject arrowObject;
    private TargetDetection targetDetection;
    private int energyCost = 1;

    //EVENTS
    public static event Func<DamageType> OnTargetGet;
    public static event Action<int> OnAfterAttack;
    public static event Action OnAttackSuccess;


    private void OnMouseDown()
    {
        SpawnArrow(ArrowPrefab);
    }
    private void OnMouseDrag()
    {
        arrowObject.GetComponent<BezierArrow>().GetMousePosition();
    }
   
    private void OnMouseUp()
    {
        if (arrowObject != null)
        {
            if (targetDetection.isTargetSelected())
            {
                Enemy target = targetDetection.GetTarget();
                PerformAttackAction(target);
            }
            Destroy(arrowObject);
        }
    }

    private void PerformAttackAction(Enemy target)
    {
        DamageType damage = OnTargetGet?.Invoke();
        target.TakeDamage(damage);
        OnAfterAttack?.Invoke(energyCost);
        OnAttackSuccess?.Invoke();
    }

    private void SpawnArrow(GameObject ArrowPrefab)
    {
        Vector3 spawnPosition = this.GetComponent<RectTransform>().position;

        if (arrowObject == null)
        {
            arrowObject = Instantiate(ArrowPrefab, spawnPosition, Quaternion.identity);
            arrowObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
            arrowObject.transform.localScale = new Vector3(1, 1, 1);
            targetDetection = arrowObject.GetComponentInChildren<TargetDetection>();
        }
    }
}
