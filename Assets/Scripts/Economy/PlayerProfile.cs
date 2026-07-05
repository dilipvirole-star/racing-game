using UnityEngine;
using RacingGame.Events;

namespace RacingGame.Economy
{
    /// <summary>
    /// Manages player profile including money, XP, and progression.
    /// </summary>
    [System.Serializable]
    public class PlayerProfile
    {
        [SerializeField] private float _money = 10000f;
        [SerializeField] private int _xp = 0;
        [SerializeField] private int _level = 1;
        [SerializeField] private int _wantedLevel = 0;
        [SerializeField] private int[] _ownedVehicleIds = new int[0];

        private const int XP_PER_LEVEL = 1000;
        private EventSystem _eventSystem = EventSystem.Instance;

        public float Money => _money;
        public int XP => _xp;
        public int Level => _level;
        public int WantedLevel => _wantedLevel;

        public void AddMoney(float amount)
        {
            _money += amount;
            _eventSystem.Publish(new MoneyChangedEvent { NewBalance = _money, Delta = amount });
        }

        public void RemoveMoney(float amount)
        {
            _money -= Mathf.Max(0, amount);
            _eventSystem.Publish(new MoneyChangedEvent { NewBalance = _money, Delta = -amount });
        }

        public void AddXP(int amount)
        {
            _xp += amount;
            _eventSystem.Publish(new XPGainedEvent { Amount = amount });

            int newLevel = (_xp / XP_PER_LEVEL) + 1;
            if (newLevel > _level)
            {
                _level = newLevel;
                _eventSystem.Publish(new LevelUpEvent { NewLevel = _level });
            }
        }

        public void IncreaseWantedLevel()
        {
            _wantedLevel = Mathf.Min(_wantedLevel + 1, 5);
            _eventSystem.Publish(new WantedLevelChangedEvent { NewLevel = _wantedLevel });
        }

        public void DecreaseWantedLevel()
        {
            _wantedLevel = Mathf.Max(_wantedLevel - 1, 0);
            _eventSystem.Publish(new WantedLevelChangedEvent { NewLevel = _wantedLevel });
        }
    }
}
