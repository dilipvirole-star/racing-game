using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

namespace RacingGame.Utilities
{
    /// <summary>
    /// Asset loading utility using Addressables system for efficient resource management.
    /// </summary>
    public class AssetLoader
    {
        private static Dictionary<string, AsyncOperationHandle> _loadedAssets = new();

        public static void LoadAssetAsync<T>(string address, System.Action<T> onComplete) where T : Object
        {
            if (_loadedAssets.ContainsKey(address))
            {
                var cachedAsset = _loadedAssets[address];
                if (cachedAsset.Result is T result)
                {
                    onComplete?.Invoke(result);
                    return;
                }
            }

            var handle = Addressables.LoadAssetAsync<T>(address);
            handle.Completed += (op) =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    _loadedAssets[address] = op;
                    onComplete?.Invoke(op.Result);
                }
                else
                {
                    Debug.LogError($"Failed to load asset: {address}");
                }
            };
        }

        public static void UnloadAsset(string address)
        {
            if (_loadedAssets.TryGetValue(address, out var handle))
            {
                Addressables.Release(handle);
                _loadedAssets.Remove(address);
            }
        }
    }
}
