using UnityEngine;
using System;
using System.Collections.Generic;

namespace RacingGame.Events
{
    /// <summary>
    /// Global event system for decoupled communication between game systems.
    /// </summary>
    public class EventSystem
    {
        private static EventSystem _instance;
        public static EventSystem Instance => _instance ??= new EventSystem();

        private Dictionary<Type, List<Delegate>> _subscribers = new();

        public void Subscribe<T>(Action<T> handler) where T : GameEvent
        {
            var eventType = typeof(T);

            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = new List<Delegate>();
            }

            _subscribers[eventType].Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler) where T : GameEvent
        {
            var eventType = typeof(T);

            if (_subscribers.TryGetValue(eventType, out var handlers))
            {
                handlers.Remove(handler);
            }
        }

        public void Publish<T>(T gameEvent) where T : GameEvent
        {
            var eventType = typeof(T);

            if (_subscribers.TryGetValue(eventType, out var handlers))
            {
                foreach (var handler in handlers)
                {
                    ((Action<T>)handler)?.Invoke(gameEvent);
                }
            }
        }
    }

    /// <summary>
    /// Base class for all game events.
    /// </summary>
    public abstract class GameEvent
    {
        public float Timestamp { get; } = Time.time;
    }

    // Vehicle Events
    public class VehicleSpawnedEvent : GameEvent
    {
        public int VehicleId { get; set; }
        public Vector3 Position { get; set; }
    }

    public class VehicleDamagedEvent : GameEvent
    {
        public int VehicleId { get; set; }
        public float DamageAmount { get; set; }
    }

    public class VehicleDestroyedEvent : GameEvent
    {
        public int VehicleId { get; set; }
    }

    // Race Events
    public class RaceStartedEvent : GameEvent
    {
        public int RaceId { get; set; }
        public string RaceType { get; set; }
    }

    public class RaceFinishedEvent : GameEvent
    {
        public int RaceId { get; set; }
        public int WinnerId { get; set; }
        public float Time { get; set; }
    }

    public class CheckpointReachedEvent : GameEvent
    {
        public int CheckpointIndex { get; set; }
        public float ElapsedTime { get; set; }
    }

    // Player Events
    public class MoneyChangedEvent : GameEvent
    {
        public float NewBalance { get; set; }
        public float Delta { get; set; }
    }

    public class XPGainedEvent : GameEvent
    {
        public int Amount { get; set; }
    }

    public class LevelUpEvent : GameEvent
    {
        public int NewLevel { get; set; }
    }

    // Wanted Events
    public class WantedLevelChangedEvent : GameEvent
    {
        public int NewLevel { get; set; }
    }
}
