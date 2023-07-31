using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialAction : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    #region VARIABLES
    public GameObject MaterialPrefab;
    private GameObject MaterialObject;
    private GameObject targetedAction;
    public AudioSource deniedSFX;
    public AudioSource successSFX;
    private PlayerMaterial material;
    private int energyCost = 1;

    public static Dictionary<string, int> materialList = new()
    {
        { "Cloth",0},
        { "Twine", 0 },
        { "Leather", 0 },
        { "Wood", 0 },
        { "Stone", 0 },
        { "Metal", 0 },
        { "Glass", 0 },
        { "EleCrystal", 0 }
    };
    #endregion

    #region EVENTS
    public static event Func<GameObject> OnMouseRelease;//MaterialPrefab.GetAction()
    public static event Func<int, bool> BeforeAction; //Player.IsEnoughEnergy()

    //enhance attack events
    public static event Action<float,int> OnAttackEnhance;//Player.ModifyDamage()

    //enhance defense events
    public static event Action<float> OnDefEnhance;//Player.ModifyDefense()

    public static event Action<int> OnSuccessEnhance; //Player.ReduceEnergy()
    public static event Action OnAfterEnhance; // BattleManager.UpdateHud()

    public static event Action OnUpdateMaterialUI; //DiceHandler.DestroyThis();
    #endregion

    private void Start()
    {
        material = this.GetComponent<PlayerMaterial>();
        DiceHandler.OnAllDiceLanded += GetMaterialList;
        DiceHandler.OnMaterialListUpdated += FinishDiceRoll;
    }

    private void OnDestroy()
    {
        DiceHandler.OnAllDiceLanded -= GetMaterialList;
        DiceHandler.OnMaterialListUpdated -= FinishDiceRoll;
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

            if (targetedAction != null 
                && BeforeAction.Invoke(energyCost) 
                && materialList[this.name] > 0)
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
                successSFX.Play();
                ReduceMaterialCount();
            }
            else
            {
                deniedSFX.Play();
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

    private void ReduceMaterialCount()
    {
        materialList[this.name] -= 1;
        UpdateMaterialListUI();
    }

    private void FinishDiceRoll()
    {
        UpdateMaterialListUI();
        StartCoroutine(DelayDestroyDiceBox());
    }

    private void UpdateMaterialListUI()
    {
        foreach (var item in materialList)
        {
            GameObject.Find("Canvas/BottomUI/Resource/"+item.Key+"/Amount").GetComponent<TMP_Text>().text = item.Value.ToString();
        }
    }
    private IEnumerator DelayDestroyDiceBox()
    {
        yield return new WaitForSeconds(3f);
        OnUpdateMaterialUI.Invoke();
    }

    private Dictionary<string,int> GetMaterialList()
    {
        return materialList;
    }
}
