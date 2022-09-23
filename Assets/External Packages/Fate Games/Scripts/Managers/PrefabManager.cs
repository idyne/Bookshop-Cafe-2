using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FateGames
{
    public class PrefabManager : MonoBehaviour
    {
        private static PrefabManager instance;
        [SerializeField] private PrefabIdentifier[] prefabs;
        private Dictionary<string, GameObject> prefabDictionary;

        public static PrefabManager Instance { get => instance; }

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
                return;
            }
            prefabDictionary = new Dictionary<string, GameObject>();
            for (int i = 0; i < prefabs.Length; i++)
            {
                PrefabIdentifier prefab = prefabs[i];
                prefabDictionary.Add(prefab.Tag, prefab.Prefab);
            }
        }

        public GameObject GetPrefab(string tag)
        {
            if (prefabDictionary.ContainsKey(tag))
            {
                return prefabDictionary[tag];
            }
            else
            {
                Debug.LogError(string.Format("There is no \"{0}\" tagged prefab!"));
                return null;
            }
        }

        [System.Serializable]
        private class PrefabIdentifier
        {
            [SerializeField] private string tag;
            [SerializeField] private GameObject prefab;

            public string Tag { get => tag; }
            public GameObject Prefab { get => prefab; }
        }
    }

}