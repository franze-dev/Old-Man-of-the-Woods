using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Service to store events
/// </summary>
public static class EventProvider
{
    private static Dictionary<Type, Delegate> _eventListeners = new();
    public static Dictionary<Type, Delegate> EventListeners { get { return _eventListeners; } }

    /// <summary>
    /// Subscribes a new action to the dictionary according to its type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="listener"></param>
    public static void Subscribe<T>(Action<T> listener) where T : IEvent
    {
        if (_eventListeners.TryGetValue(typeof(T), out var existingDelegate))
            _eventListeners[typeof(T)] = Delegate.Combine(existingDelegate, listener);
        else
            _eventListeners[typeof(T)] = listener;
    }

    /// <summary>
    /// Unsubscribe a specific action from the dictionary acording to its type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="listener"></param>
    public static void Unsubscribe<T>(Action<T> listener) where T : IEvent
    {
        if (_eventListeners.TryGetValue(typeof(T), out var existingDelegate))
        {
            var newDelegate = Delegate.Remove(existingDelegate, listener);

            if (newDelegate == null)
                _eventListeners.Remove(typeof(T));
            else
                _eventListeners[typeof(T)] = newDelegate;
        }
    }

}

/// <summary>
/// Triggers events
/// </summary>
public static class EventTriggerer
{
    public static void Trigger<T>(T myEvent) where T : IEvent
    {
        if (EventProvider.EventListeners.TryGetValue(typeof(T), out var action))
            (action as Action<T>)?.Invoke(myEvent);
    }
}

/// <summary>
/// Interface for all events
/// </summary>
public interface IEvent
{
}
