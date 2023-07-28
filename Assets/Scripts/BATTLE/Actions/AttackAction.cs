using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class AttackAction : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    public GameObject ArrowPrefab;
    private GameObject arrowObject;
    private TargetDetection targetDetection;
    private int energyCost = 1;
    public AudioSource deniedSFX;
    public AudioSource attackSFX;

    //EVENTS
    public static event Func<DamageType> OnTargetGet; // Player.GetDamage();
    public static event Func<int,bool> BeforeAttack; // Player.IsEnoughEnergy()
    public static event Action<int> OnAfterAttack; // Player.ReduceCurrentEnergy();
    public static event Action OnAttackSuccess; // Player.ResetValues() , Player.PlayAttackAnim(), BattleManager.UpdateHud()

    public void OnPointerDown(PointerEventData data) {
        SpawnArrow(ArrowPrefab);
    }
    public void OnPointerUp(PointerEventData data) {
        if (BeforeAttack.Invoke(energyCost))
        {
            if (arrowObject != null)
            {
                if (targetDetection.isTargetSelected())
                {
                    Enemy target = targetDetection.GetTarget();
                    attackSFX.Play();
                    PerformAttackAction(target);
                }
                Destroy(arrowObject);
            }
        }
        else
        {
            deniedSFX.Play();
            Destroy(arrowObject);
        }
        
    }

    public void OnDrag(PointerEventData data)
    {
        arrowObject.GetComponent<BezierArrow>().GetMousePosition();
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
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        if (arrowObject == null)
        {
            arrowObject = Instantiate(ArrowPrefab, spawnPosition, Quaternion.identity);
            targetDetection = arrowObject.GetComponentInChildren<TargetDetection>();
        }
    }
}
