using UnityEngine;
using System.IO;
using RacingGame.Economy;

namespace RacingGame.Utilities
{
    /// <summary>
    /// Save/Load system for player progression and game state.
    /// </summary>
    public class SaveSystem
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "racing_game_save.json");

        public static void SavePlayerProfile(PlayerProfile profile)
        {
            string json = JsonUtility.ToJson(profile, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"Game saved to: {SavePath}");
        }

        public static PlayerProfile LoadPlayerProfile()
        {
            if (!File.Exists(SavePath))
            {
                Debug.Log("No save file found, creating new profile.");
                return new PlayerProfile();
            }

            string json = File.ReadAllText(SavePath);
            PlayerProfile profile = JsonUtility.FromJson<PlayerProfile>(json);
            Debug.Log("Game loaded successfully.");
            return profile;
        }

        public static bool SaveFileExists() => File.Exists(SavePath);

        public static void DeleteSaveFile()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
                Debug.Log("Save file deleted.");
            }
        }
    }
}
