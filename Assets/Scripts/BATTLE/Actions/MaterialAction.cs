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

    private EventManager eventManager = EventManager.Instance;
    #endregion

    private void Start()
    {
        material = this.GetComponent<PlayerMaterial>();
        eventManager.AddListener(Event.PLAYER_DICE, GetMaterialList);
        eventManager.AddListener(Event.PLAYER_DICE, FinishDiceRoll);
    }

    private void OnDestroy()
    {
        eventManager.RemoveListener(Event.PLAYER_DICE, GetMaterialList);
        eventManager.RemoveListener(Event.PLAYER_DICE, FinishDiceRoll);
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
        bool isEnoughEnergy = eventManager.TriggerEvent<int,bool>(Event.PLAYER_ENHANCE, energyCost);
        if (MaterialObject != null)
        {
            targetedAction = eventManager.TriggerEvent<GameObject>(Event.PLAYER_ENHANCE_MOUSE_RELEASE);

            if (targetedAction != null 
                && isEnoughEnergy 
                && materialList[this.name] > 0)
            {
                if (targetedAction.CompareTag("Attack"))
                {
                    eventManager.TriggerEvent<float,int>(Event.PLAYER_ENHANCE_ATTACK, material.DamageModifier, material.NumOfHits);
                    eventManager.TriggerEvent<int>(Event.PLAYER_ENHANCE_SUCCESS, energyCost);
                }

                if (targetedAction.CompareTag("Defend"))
                {
                    eventManager.TriggerEvent<float>(Event.PLAYER_ENHANCE_DEFEND,material.DefenseModifier);
                    eventManager.TriggerEvent<int>(Event.PLAYER_ENHANCE_SUCCESS, energyCost);
                }
                successSFX.Play();
                ReduceMaterialCount();
            }
            else
            {
                deniedSFX.Play();
            }
            eventManager.TriggerEvent(Event.PLAYER_ENHANCE_SUCCESS);
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
        eventManager.TriggerEvent(Event.PLAYER_MATERIALUPDATED);
    }

    private Dictionary<string,int> GetMaterialList()
    {
        return materialList;
    }
}
