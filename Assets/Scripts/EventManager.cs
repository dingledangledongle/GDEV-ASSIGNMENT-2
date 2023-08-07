using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static EventManager instance;

    public static EventManager Instance // making a eventmanager singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManager();
            }
            return instance;
        }
    }

    private Dictionary<Event,List<Delegate>> eventListeners;

    public EventManager()
    {
        eventListeners = new();
    }

    #region ADD/REMOVE/TRIGGER (NO PARAMETERS)
    public void AddListener(Event eventName, Action listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = new List<Delegate>();
        }

        eventListeners[eventName].Add(listener);
    }

    public void RemoveListener(Event eventName, Action listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName].Remove(listener);
        }
    }
    public void TriggerEvent(Event eventName)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            foreach (var listener in eventListeners[eventName])
            {
                if (listener is Action action)
                {
                    action.Invoke();
                }
            }
        }
        else
        {
            Debug.Log("event not in list");
        }

    }
    #endregion

    #region ADD/REMOVE/TRIGGER (W/ PARAM)
    public void AddListener<TParam>(Event eventName, Action<TParam> listener) 
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = new List<Delegate>();
        }

        eventListeners[eventName].Add(listener);
    }

    public void RemoveListener<TParam>(Event eventName, Action<TParam> listener)
    {
        if (eventListeners.ContainsKey(eventName)) 
        {
            eventListeners[eventName].Remove(listener);
        }
    }

    public void TriggerEvent<TParam>(Event eventName, TParam param)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            foreach (var listener in eventListeners[eventName])
            {
                if(listener is Action<TParam> action)
                {
                    action.Invoke(param);
                }
            }
        }
    }

    public void AddListener<TParam1,TParam2>(Event eventName, Action<TParam1,TParam2> listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = new List<Delegate>();
        }

        eventListeners[eventName].Add(listener);
    }

    public void RemoveListener<TParam1, TParam2>(Event eventName, Action<TParam1, TParam2> listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName].Remove(listener);
        }
    }

    public void TriggerEvent<TParam1, TParam2>(Event eventName, TParam1 param1,TParam2 param2)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            foreach (var listener in eventListeners[eventName])
            {
                if (listener is Action<TParam1, TParam2> action)
                {
                    action.Invoke(param1,param2);
                }
            }
        }
    }
    #endregion

    #region ADD/REMOVE/TRIGGER (W/ RETURN)
    public void AddListener<TResult>(Event eventName, Func<TResult> listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = new List<Delegate>();
        }

        eventListeners[eventName].Add(listener);
    }

    public void RemoveListener<TResult>(Event eventName, Func<TResult> listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName].Remove(listener);
        }
    }

    public TResult TriggerEvent<TResult>(Event eventName)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            var listeners = eventListeners[eventName].ToArray();

            foreach (var listener in listeners)
            {
                if (listener is Func<TResult> function)
                {
                    return function.Invoke();
                }
            }
        }

        Debug.Log("returning default...");
        return default(TResult);
    }
    #endregion

    #region ADD/REMOVE/TRIGGER (W/ RETURN + PARAM)
    public void AddListener<TParam,TResult>(Event eventName, Func<TParam,TResult> listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = new List<Delegate>();
        }

        eventListeners[eventName].Add(listener);
    }

    public void RemoveListener<TParam, TResult>(Event eventName, Func<TParam, TResult> listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName].Remove(listener);
        }
    }

    public TResult TriggerEvent<TParam, TResult>(Event eventName,TParam param)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            var listeners = eventListeners[eventName].ToArray();

            foreach (var listener in listeners)
            {
                if (listener is Func<TParam,TResult> function)
                {
                    return function.Invoke(param);
                }
            }
        }

        Debug.Log("returning default...");
        return default(TResult);
    }
    #endregion
}
public enum Event
{
    UPDATE_HUD,
    MAP_NODE_CLICKED,
    BATTLE_START,
    PLAYER_ATTACK,
    PLAYER_ATTACK_FINISHED,
    PLAYER_DEFEND,
    PLAYER_DEFEND_FINISHED,
    PLAYER_ENHANCE,
    PLAYER_ENHANCE_ATTACK,
    PLAYER_ENHANCE_DEFEND,
    PLAYER_ENHANCE_SUCCESS,
    REST_INITIALIZE,
    REST_HEAL,
    REST_UPGRADEATTACK,
    REST_UPGRADEDEFEND,
    REST_FINISHED,
    RAND_EVENT_INITIALIZE,
    RAND_EVENT_END,
    RAND_EVENT_STW_WHEELSTOP,
    RAND_EVENT_UPGRADEATTACK,
    RAND_EVENT_UPGRADEDEFEND,
    RAND_EVENT_UPGRADEHEALTH,
    RAND_EVENT_HEAL,
    RAND_EVENT_TAKEDAMAGE,
    RAND_EVENT_REDUCEMAXHEALTH,
    


}

