using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class UnityEventExtension
{
    public delegate bool Condition();
    public static void AddOneTimeListener(this UnityEvent unityEvent, UnityAction newAction)
    {
        UnityAction action = null;
        action = () =>
        {
            newAction();
            unityEvent.RemoveListener(action);
        };
        unityEvent.AddListener(action);
    }

    public static void AddConditionalOneTimeListener(this UnityEvent unityEvent, UnityAction newAction, Condition condition)
    {
        UnityAction action = null;
        action = () =>
        {
            newAction();
            if (condition())
                unityEvent.RemoveListener(action);
        };
        unityEvent.AddListener(action);
    }

    public static void AddOneTimeListener<T>(this UnityEvent<T> unityEvent, UnityAction<T> newAction)
    {
        UnityAction<T> action = null;
        action = (T) =>
        {
            newAction(T);
            unityEvent.RemoveListener(action);
        };
        unityEvent.AddListener(action);
    }

    public static void AddConditionalOneTimeListener<T>(this UnityEvent<T> unityEvent, UnityAction<T> newAction, Condition condition)
    {
        UnityAction<T> action = null;
        action = (T) =>
        {
            newAction(T);
            if (condition())
                unityEvent.RemoveListener(action);
        };
        unityEvent.AddListener(action);
    }
}
