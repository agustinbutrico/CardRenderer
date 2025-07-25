using UnityEngine;

namespace CardRenderer.Utils
{
    internal class GameObjectHelper
    {
        internal static GameObject GetOrCreatePersistentGO(string name)
        {
            GameObject go = GameObject.Find(name);
            if (go == null)
            {
                go = new GameObject(name);
                Object.DontDestroyOnLoad(go);
            }
            return go;
        }

        internal static GameObject GetOrCreateChild(Transform parent, string childName)
        {
            Transform existing = parent.Find(childName);
            if (existing != null)
                return existing.gameObject;

            GameObject child = new GameObject(childName);
            child.transform.SetParent(parent, false);
            return child;
        }

        internal static Transform FindDeepChild(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                    return child;
                var result = FindDeepChild(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
