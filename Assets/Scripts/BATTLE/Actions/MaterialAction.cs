using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialAction : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    public GameObject MaterialPrefab;
    private GameObject MaterialObject;
    private GameObject targetedAction;
    private PlayerMaterial material;
    private int energyCost = 1;

    public static event Func<GameObject> OnMouseRelease;

    //enhance attack events
    public static event Action<float,int> OnAttackEnhance;

    //enhance defense events
    public static event Action<float> OnDefEnhance;

    public static event Action<int> OnSuccessEnhance;
    public static event Action OnAfterEnhance;

    private void Start()
    {

        material = this.GetComponent<PlayerMaterial>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (MaterialObject == null)
        {
            MaterialObject = Instantiate(MaterialPrefab, this.GetComponent<RectTransform>().position, Quaternion.identity);
            MaterialObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        }
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        if (MaterialObject != null)
        {
            targetedAction = OnMouseRelease?.Invoke();
            if (targetedAction.CompareTag("Attack"))
            {
                OnAttackEnhance?.Invoke(material.DamageModifier, material.NumOfHits);
                OnSuccessEnhance?.Invoke(energyCost);
            }

            if (targetedAction.CompareTag("Defend"))
            {
                OnDefEnhance?.Invoke(material.DefenseModifier);
                OnSuccessEnhance?.Invoke(energyCost);
            }

            OnAfterEnhance?.Invoke();
            Destroy(MaterialObject);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (MaterialObject != null)
        {
            MaterialObject.GetComponent<MaterialPrefab>().UpdatePosition();
        }
    }

}
