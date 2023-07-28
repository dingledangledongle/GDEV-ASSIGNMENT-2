using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class MaterialAction : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    #region VARIABLES
    public GameObject MaterialPrefab;
    private GameObject MaterialObject;
    private GameObject targetedAction;
    private PlayerMaterial material;
    private int energyCost = 1;
    private Dictionary<string, int> materialList = new()
    {
        { "Cloth",0},
        { "Twine", 0 },
        { "Leather", 0 },
        { "Wood", 0 },
        { "Stone", 0 },
        { "Metal", 0 },
        { "Glass", 0 },
        { "EleCrystal", 0 },
    };
    #endregion

    #region EVENTS
    public static event Func<GameObject> OnMouseRelease;//MaterialPrefab.GetAction()

    //enhance attack events
    public static event Action<float,int> OnAttackEnhance;//Player.ModifyDamage()

    //enhance defense events
    public static event Action<float> OnDefEnhance;//Player.ModifyDefense()

    public static event Action<int> OnSuccessEnhance; //Player.ReduceEnergy()
    public static event Action OnAfterEnhance; // BattleManager.UpdateHud()

    public static event Action hello;
    #endregion

    private void Start()
    {
        material = this.GetComponent<PlayerMaterial>();
        DiceHandler.OnAllDiceLanded += GetMaterialList;
        DiceHandler.OnMaterialListUpdated += UpdateMaterialListUI;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (MaterialObject == null)
        {
            MaterialObject = Instantiate(MaterialPrefab, this.GetComponent<RectTransform>().position, Quaternion.identity);
            MaterialObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
            MaterialObject.name = this.name;
        }
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        if (MaterialObject != null)
        {
            targetedAction = OnMouseRelease?.Invoke();
            if(targetedAction != null)
            {
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

    private void UpdateMaterialListUI()
    {
        Debug.Log("MaterialAction updateUI");
        foreach (var item in materialList)
        {
            GameObject.Find(item.Key).GetComponent<TMP_Text>().text = item.Value.ToString();
        }
    }

    private Dictionary<string,int> GetMaterialList()
    {
        return materialList;
    }
}
