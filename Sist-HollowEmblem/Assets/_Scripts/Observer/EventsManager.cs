using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EventsManager
{
    public static EventsManager Instance
    {
        get
        {
            // Lazy singleton
            // No se puede usar en un MonoBehaviours
            if (instance == null)
                instance = new EventsManager();

            return instance;
        }
    }

    private static EventsManager instance;

    //Simple Events
    private Dictionary<string, List<IListener>> simpleEvents = new();

    public void AddListener(string eventID, IListener p_listener)
    {
        if (simpleEvents.TryGetValue(eventID, out var listeners) && !listeners.Contains(p_listener))
        {
            listeners.Add(p_listener);
        }
    }

    public void RemoveListener(string eventID, IListener p_listener)
    {
        if (simpleEvents.TryGetValue(eventID, out var listeners) && listeners.Contains(p_listener))
        {
            listeners.Remove(p_listener);
        }
    }

    public void DispatchSimpleEvent(string eventID)
    {
        if (simpleEvents.TryGetValue(eventID, out var listeners))
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventDispatch();
            }
        }

        else
        {
            Debug.LogWarning($"{eventID} NO SE ENCONTRO");
        }
    }

    public void RegisterEvent(string eventID)
    {
        if (!simpleEvents.ContainsKey(eventID))
        {
            simpleEvents[eventID] = new List<IListener>();
        }
    }
}
