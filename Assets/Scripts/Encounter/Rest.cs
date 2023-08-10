using UnityEngine;

public class Rest : MonoBehaviour
{
    private float healPercentage = 0.3f;
    private float amtToUpgrade = 1f;

    private EventManager eventManager = EventManager.Instance;

    public void HealPlayer()
    {
        //gets the max HP of the player by triggering the event when the rest button is clicked
        float maxHP = eventManager.TriggerEvent<float>(Event.REST_HEAL);
        float healthToBeHealed = maxHP * healPercentage; // heals the player by a percentage of their max hp

        //trigger the event that calls the HealPlayer method in the Player class and passing the amount of hp to be healed in
        eventManager.TriggerEvent<float>(Event.REST_HEAL, healthToBeHealed);
        
        //ending the event
        eventManager.TriggerEvent(Event.REST_FINISHED);
        
    }

    public void UpgradeAttack()
    {
        //trigger the event that calls the UpgradeDamage method in the Player class and passing the amount of damage to be increased
        eventManager.TriggerEvent<float>(Event.REST_UPGRADEATTACK, amtToUpgrade);

        //ending the event
        eventManager.TriggerEvent(Event.REST_FINISHED);
    }

    public void UpgradeDefense()
    {
        //trigger the event that calls the UpgradeDefend method in the Player class and passing the amount of damage to be increased
        eventManager.TriggerEvent<float>(Event.REST_UPGRADEDEFEND, amtToUpgrade);

        //ending the event
        eventManager.TriggerEvent(Event.REST_FINISHED);     
    }



}
