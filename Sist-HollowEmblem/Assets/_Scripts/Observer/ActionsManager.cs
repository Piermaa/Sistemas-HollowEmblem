using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ActionConstants
{
    public static string DEATH = "Death";
    public static string TAKE_DAMAGE = "TakeDamage";
}

public static class ActionsManager
{
    private static Dictionary<string, Action> actions = new ();

    public static void InvokeAction(string name)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to invoke {name}, but no action was found");
            return;
        }

        actions[name]?.Invoke();
    }

    public static void RegisterAction(string name)
    {
        if (actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to register {name}, but it is already registered");
            return;
        }

        actions.Add(name, new Action(() => { }));
    }

    public static void SubscribeToAction(string name, Action action)
    {
        if (!actions.ContainsKey(name))
        {
            RegisterAction(name);
        }

        actions[name] += action;
    }

    public static void UnsubscribeToAction(string name, Action action)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to unsubscribe to {name}, but no action was found");
            return;
        }

        actions[name] -= action;
    }

    public static void DeleteAction(string name)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to delete {name}, but no action was found");
            return;
        }

        actions[name] = null;
        actions.Remove(name);
    }

    public static void DeleteAllActions()
    {
        foreach (var item in actions)
        {
            DeleteAction(item.Key);
        }
    }
}

internal static class InventoryActionsManager
{
    private static Dictionary<string, Action<Slot>> actions = new Dictionary<string, Action<Slot>>();

    public static void InvokeAction(string name, Slot slot)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to invoke {name}, but no action was found");
            return;
        }

        actions[name]?.Invoke(slot);
    }

    public static void RegisterAction(string name)
    {
        if (actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to register {name}, but it is already registered");
            return;
        }

        actions.Add(name, new Action<Slot>((Slot s) => { }));
    }

    public static void SubscribeToAction(string name, Action<Slot> action)
    {
        if (!actions.ContainsKey(name))
        {
            RegisterAction(name);
        }

        actions[name] += action;
    }

    public static void UnsubscribeToAction(string name, Action<Slot> action)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to unsubscribe to {name}, but no action was found");
            return;
        }

        actions[name] -= action;
    }

    public static void DeleteAction(string name)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to delete {name}, but no action was found");
            return;
        }

        actions[name] = null;
        actions.Remove(name);
    }

    public static void DeleteAllActions()
    {
        foreach (var item in actions)
        {
            DeleteAction(item.Key);
        }
    }
}

internal static class IntActionsManager
{
    private static Dictionary<string, Action<int>> actions = new Dictionary<string, Action<int>>();

    public static void InvokeAction(string name, int value)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to invoke {name}, but no action was found");
            return;
        }

        actions[name]?.Invoke(value);
    }

    public static void RegisterAction(string name)
    {
        if (actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to register {name}, but it is already registered");
            return;
        }

        actions.Add(name, new Action<int>((int v) => { }));
    }

    public static void SubscribeToAction(string name, Action<int> action)
    {
        if (!actions.ContainsKey(name))
        {
            RegisterAction(name);
        }

        actions[name] += action;
    }

    public static void UnsubscribeToAction(string name, Action<int> action)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to unsubscribe to {name}, but no action was found");
            return;
        }

        actions[name] -= action;
    }

    public static void DeleteAction(string name)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogWarning($"Tried to delete {name}, but no action was found");
            return;
        }

        actions[name] = null;
        actions.Remove(name);
    }

    public static void DeleteAllActions()
    {
        foreach (var item in actions)
        {
            DeleteAction(item.Key);
        }
    }
}
