using UnityEngine;
using System.Collections.Generic;

namespace RacingGame.World
{
    /// <summary>
    /// Manages the open world including terrain, POIs, and environmental systems.
    /// </summary>
    public class WorldManager : MonoBehaviour
    {
        private static WorldManager _instance;
        public static WorldManager Instance => _instance;

        [SerializeField] private Terrain _terrain;
        [SerializeField] private Light _sunLight;
        [SerializeField] private float _timeScale = 1f;

        private float _currentTime = 8f; // Start at 8 AM
        private float _worldTime;
        private List<WorldPOI> _pointsOfInterest = new();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        private void Update()
        {
            UpdateDayNightCycle();
        }

        private void UpdateDayNightCycle()
        {
            _currentTime += Time.deltaTime * _timeScale / 3600f;
            if (_currentTime >= 24f)
                _currentTime -= 24f;

            float sunRotation = (_currentTime / 24f) * 360f;
            _sunLight.transform.rotation = Quaternion.Euler(sunRotation - 90f, 170f, 0f);

            float sunIntensity = Mathf.Clamp01(Mathf.Cos((sunRotation - 90f) * Mathf.Deg2Rad));
            _sunLight.intensity = Mathf.Lerp(0.2f, 1.5f, sunIntensity);
        }

        public void RegisterPOI(WorldPOI poi)
        {
            _pointsOfInterest.Add(poi);
        }

        public WorldPOI GetNearestPOI(Vector3 position)
        {
            if (_pointsOfInterest.Count == 0)
                return null;

            WorldPOI nearest = _pointsOfInterest[0];
            float minDistance = Vector3.Distance(position, nearest.Position);

            foreach (var poi in _pointsOfInterest)
            {
                float distance = Vector3.Distance(position, poi.Position);
                if (distance < minDistance)
                {
                    nearest = poi;
                    minDistance = distance;
                }
            }

            return nearest;
        }

        public float GetCurrentTime() => _currentTime;
    }

    public class WorldPOI
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public POIType Type { get; set; }
    }

    public enum POIType
    {
        GasStation,
        ParkingLot,
        RaceTrack,
        Airport,
        IndustrialArea
    }
}
