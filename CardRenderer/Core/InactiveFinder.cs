using UnityEngine;
using System.Linq;

namespace CardRenderer.Core
{
    internal static class InactiveFinder
    {
        public static Transform FindByPathIncludingInactive(string path)
        {
            var allTransforms = Resources.FindObjectsOfTypeAll<Transform>();

            foreach (var t in allTransforms)
            {
                if (t.name != path.Split('/').Last())
                    continue;

                string fullPath = GetFullPath(t);
                if (fullPath == path)
                    return t;
            }

            return null;
        }

        private static string GetFullPath(Transform t)
        {
            string path = t.name;
            while (t.parent != null)
            {
                t = t.parent;
                path = t.name + "/" + path;
            }
            return path;
        }
    }
}
