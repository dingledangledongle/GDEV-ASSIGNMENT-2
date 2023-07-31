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
                    Debug.Log("Triggering event<T>");
                    action.Invoke(param);
                }
            }
        }
    }
    #endregion

    #region ADD/REMOVE/TRIGGER (W/ RETURN)
    public void AddListenerWithReturn<TResult>(Event eventName, Func<TResult> listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = new List<Delegate>();
        }

        eventListeners[eventName].Add(listener);
    }

    public void RemoveListenerWithReturn<TResult>(Event eventName, Func<TResult> listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName].Remove(listener);
        }
    }

    public TResult TriggerEventWithReturn<TResult>(Event eventName)
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
    public void AddListenerWithReturnAndArg<TParam,TResult>(Event eventName, Func<TParam,TResult> listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = new List<Delegate>();
        }

        eventListeners[eventName].Add(listener);
    }

    public void RemoveListenerWithReturnAndArg<TParam, TResult>(Event eventName, Func<TParam, TResult> listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName].Remove(listener);
        }
    }

    public TResult TriggerEventWithReturnAndArg<TParam, TResult>(Event eventName,TParam param)
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
    PLAYER_GET_DAMAGE,
    PLAYER_CHECK_ENERGY,
    PLAYER_REDUCE_ENERGY,
    PLAYER_ATTACK,
    PLAYER_ATTACK_FINISHED

}

