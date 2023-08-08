using System;
using System.Collections;
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
    private EventManager eventManager = EventManager.Instance;

    //EVENTS

    public void OnPointerDown(PointerEventData data) {
        SpawnArrow(ArrowPrefab);
    }

    public void OnPointerUp(PointerEventData data) {
        bool EnoughEnergy = eventManager.TriggerEvent<int, bool>(Event.PLAYER_ATTACK, energyCost);
        if (EnoughEnergy)
        {
            if (arrowObject != null)
            {
                if (targetDetection.isTargetSelected())
                {
                    Enemy target = targetDetection.GetTarget();
                    attackSFX.Play();
                    StartCoroutine(PerformAttackAction(target));
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

    private IEnumerator PerformAttackAction(Enemy target)
    {
        DamageType damage = eventManager.TriggerEvent<DamageType>(Event.PLAYER_ATTACK);
        target.TakeDamage(damage);
        eventManager.TriggerEvent<int>(Event.PLAYER_ATTACK, energyCost);
        eventManager.TriggerEvent(Event.PLAYER_ATTACK);
        yield return new WaitUntil(target.IsDamageCalculationDone);
        target.IsFinished = false;
        eventManager.TriggerEvent(Event.PLAYER_ATTACK_FINISHED);
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
