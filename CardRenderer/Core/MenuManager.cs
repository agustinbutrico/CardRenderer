using TexturesLib.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace CardRenderer.Core
{
    internal class MenuManager
    {
        internal static GameObject NewMenu(string parentPath, string newRootName, string prefabAspectName)
        {
            string prefabAspectNameResolved = prefabAspectName ?? "DefaultMenu";
            string prefabPath = "CardRenderer/MenusAspects/" + prefabAspectNameResolved;
            GameObject prefabGO = GameObject.Find(prefabPath);

            if (prefabGO == null)
            {
                Plugin.Log.LogError($"Menu prefab not found at '{prefabPath}'. Cannot create menu.");
                return null;
            }

            GameObject parentGO = GameObject.Find(parentPath);
            if (parentGO == null)
            {
                Plugin.Log.LogError($"Parent GameObject not found at '{parentPath}'. Cannot attach menu.");
                return null;
            }

            GameObject menuInstance = UnityEngine.Object.Instantiate(prefabGO, parentGO.transform);
            menuInstance.name = newRootName;
            menuInstance.SetActive(true);

            return menuInstance;
        }

        internal static GameObject NewPanel(string parentPath, string panelName, Vector2 startPosition, bool rightToLeft, string spriteName)
        {
            GameObject parentGO = GameObject.Find(parentPath);
            if (parentGO == null)
            {
                Plugin.Log.LogWarning($"NewPanel: GameObject '{parentPath}' not found.");
                return null;
            }

            Transform targetContainer = parentGO.transform;

            Transform existingPanel = targetContainer.Find(panelName);
            if (existingPanel != null)
            {
                Plugin.Log.LogInfo($"NewPanel: Panel '{panelName}' already exists under {parentPath}.");
                return existingPanel.gameObject;
            }

            // Create new panel
            GameObject newPanel = new GameObject(panelName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            newPanel.transform.SetParent(targetContainer, false);

            // Set panel's position and pivot
            RectTransform containerRect = newPanel.GetComponent<RectTransform>();

            if (rightToLeft)
            {
                containerRect.anchorMin = new Vector2(1, 1);
                containerRect.anchorMax = new Vector2(1, 1);
                containerRect.pivot = new Vector2(1, 1);
            }
            else
            {
                containerRect.anchorMin = new Vector2(0, 1);
                containerRect.anchorMax = new Vector2(0, 1);
                containerRect.pivot = new Vector2(0, 1);
            }
            containerRect.anchoredPosition = startPosition;

            // Configure Image component
            Image image = newPanel.GetComponent<Image>();
            Sprite sprite = SpriteHelper.FindSpriteByName(spriteName);
            if (sprite != null)
            {
                image.sprite = sprite;
                image.type = Image.Type.Sliced;
            }
            else
            {
                Plugin.Log.LogWarning($"NewPanel: Sprite '{spriteName}' not found.");
            }

            return newPanel;
        }

        internal static GameObject ResizeToFitChildren(GameObject container, bool rightToLeft)
        {
            RectTransform containerRect = container.GetComponent<RectTransform>();
            if (containerRect == null || containerRect.childCount == 0)
                return container;

            CalculateBoundingBox(containerRect, out float minX, out float maxX, out float minY, out float maxY);
            ApplyContainerLayout(containerRect, minX, maxX, minY, maxY, rightToLeft, out float width, out float height, out float padding);

            if (rightToLeft)
            {
                float startXPos = 20f;
                float shift = width - startXPos - padding;

                foreach (RectTransform child in containerRect)
                {
                    if (!child.gameObject.activeInHierarchy)
                        continue;

                    float childWidth = child.sizeDelta.x;
                    child.anchoredPosition += new Vector2(shift - childWidth, 0f);
                }
            }

            containerRect.sizeDelta = new Vector2(width, height);
            return container;
        }

        internal static GameObject ResizeToFitChildrenVertical(GameObject container, bool rightToLeft)
        {
            RectTransform containerRect = container.GetComponent<RectTransform>();
            if (containerRect == null || containerRect.childCount == 0)
                return container;

            CalculateBoundingBox(containerRect, out float minX, out float maxX, out float minY, out float maxY);
            ApplyContainerLayout(containerRect, minX, maxX, minY, maxY, rightToLeft, out float width, out float height, out _);

            containerRect.sizeDelta = new Vector2(width, height);
            return container;
        }


        private static void CalculateBoundingBox(
            RectTransform containerRect,
            out float minX, out float maxX,
            out float minY, out float maxY)
        {
            minX = float.MaxValue; minY = float.MaxValue;
            maxX = float.MinValue; maxY = float.MinValue;

            foreach (RectTransform child in containerRect)
            {
                if (!child.gameObject.activeInHierarchy)
                    continue;

                Vector2 pos = child.anchoredPosition;
                Vector2 size = child.sizeDelta;
                Vector2 pivot = child.pivot;

                float left = pos.x - (size.x * pivot.x);
                float right = pos.x + (size.x * (1 - pivot.x));
                float bottom = pos.y - (size.y * pivot.y);
                float top = pos.y + (size.y * (1 - pivot.y));

                minX = Mathf.Min(minX, left);
                maxX = Mathf.Max(maxX, right);
                minY = Mathf.Min(minY, bottom);
                maxY = Mathf.Max(maxY, top);
            }
        }

        private static void ApplyContainerLayout(
            RectTransform containerRect,
            float minX, float maxX,
            float minY, float maxY,
            bool rightToLeft,
            out float width, out float height,
            out float padding)
        {
            padding = 20f;

            width = (maxX - minX) + padding * 2f;
            height = (maxY - minY) + padding * 2f;

            if (rightToLeft)
            {
                containerRect.anchorMin = new Vector2(1, 1);
                containerRect.anchorMax = new Vector2(1, 1);
                containerRect.pivot = new Vector2(1, 1);
            }
            else
            {
                containerRect.anchorMin = new Vector2(0, 1);
                containerRect.anchorMax = new Vector2(0, 1);
                containerRect.pivot = new Vector2(0, 1);
            }
        }
    }
}
