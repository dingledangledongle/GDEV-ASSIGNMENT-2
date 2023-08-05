using UnityEngine;

public class Rest : MonoBehaviour
{
    private float healPercentage = 0.3f;
    private float amtToUpgrade = 1f;

    private EventManager eventManager = EventManager.Instance;
    private Node currentNode;

    private void Start()
    {
        eventManager.AddListener<Node>(Event.REST_INITIALIZE, InitializeRest);
    }

    private void OnDestroy()
    {
        eventManager.RemoveListener<Node>(Event.REST_INITIALIZE, InitializeRest);
    }

    private void InitializeRest(Node node)
    {
        currentNode = node;

    }

    public void HealPlayer()
    {
        float maxHP = eventManager.TriggerEvent<float>(Event.REST_HEAL);
        float healthToBeHealed = maxHP * healPercentage;

        eventManager.TriggerEvent<float>(Event.REST_HEAL, healthToBeHealed);
        eventManager.TriggerEvent(Event.REST_FINISHED);
        
    }

    public void UpgradeAttack()
    {
        eventManager.TriggerEvent<float>(Event.REST_UPGRADEATTACK, amtToUpgrade);
        //eventManager.TriggerEvent(Event.REST_FINISHED);
    }

    public void UpgradeDefense()
    {

        eventManager.TriggerEvent<float>(Event.REST_UPGRADEDEFEND, amtToUpgrade);
        //eventManager.TriggerEvent(Event.REST_FINISHED);     
    }



}
