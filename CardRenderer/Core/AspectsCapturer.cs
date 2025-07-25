using CardRenderer.Utils;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CardRenderer.Core
{
    internal class AspectsCapturer
    {
        internal static void CaptureBaseCardPrefab()
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Plugin.Log.LogError("Canvas not found in MainMenu.");
                return;
            }

            Transform cardParent = GameObjectHelper.FindDeepChild(canvas.transform, "Ballista");
            if (cardParent == null)
            {
                Plugin.Log.LogError("Ballista parent not found in MainMenu.");
                return;
            }

            Transform cardTransform = cardParent.Find("Ballista");
            if (cardTransform == null)
            {
                Plugin.Log.LogError("Ballista card prefab not found inside Ballista parent.");
                return;
            }

            bool wasActive = cardTransform.gameObject.activeSelf;
            cardTransform.gameObject.SetActive(false);

            GameObject cardClone = UnityEngine.Object.Instantiate(cardTransform.gameObject);
            cardClone.name = "DefaultHorizontalCard";
            cardClone.SetActive(false);

            cardTransform.gameObject.SetActive(wasActive);

            GameObject cardRendererGO = GameObjectHelper.GetOrCreatePersistentGO("CardRenderer");
            GameObject cardsAspectsGO = GameObjectHelper.GetOrCreateChild(cardRendererGO.transform, "CardsAspects");

            cardClone.transform.SetParent(cardsAspectsGO.transform, false);
            UnityEngine.Object.DontDestroyOnLoad(cardClone);

            CleanCard(cardClone.transform);

            // Adjust the card's width (sizeDelta.x += 5f)
            RectTransform cardRect = cardClone.GetComponent<RectTransform>();
            if (cardRect != null)
            {
                cardRect.sizeDelta += new Vector2(5f, 0f);
            }

            // Move title and description downward by 5f
            Transform title = GameObjectHelper.FindDeepChild(cardClone.transform, "Title");
            Transform description = GameObjectHelper.FindDeepChild(cardClone.transform, "Description");

            if (title != null)
            {
                RectTransform titleRect = title.GetComponent<RectTransform>();
                if (titleRect != null)
                    titleRect.anchoredPosition += new Vector2(0f, -5f);
            }

            if (description != null)
            {
                RectTransform descRect = description.GetComponent<RectTransform>();
                if (descRect != null)
                    descRect.anchoredPosition += new Vector2(0f, -5f);
            }

        }

        private static void CleanCard(Transform card)
        {
            var button = card.GetComponent<Button>();
            if (button != null) UnityEngine.Object.Destroy(button);

            var upgradeButton = card.GetComponent<UpgradeButton>();
            if (upgradeButton != null) UnityEngine.Object.Destroy(upgradeButton);

            var eventTrigger = card.GetComponent<UnityEngine.EventSystems.EventTrigger>();
            if (eventTrigger != null) UnityEngine.Object.Destroy(eventTrigger);
        }

        internal static void CaptureBaseMenuPrefab()
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Plugin.Log.LogError("Canvas not found in MainMenu.");
                return;
            }

            // Find the 'UpgradeMenu' GameObject
            Transform upgradeMenuTransform = GameObjectHelper.FindDeepChild(canvas.transform, "UpgradeMenu");
            if (upgradeMenuTransform == null)
            {
                Plugin.Log.LogError("UpgradeMenu not found in MainMenu.");
                return;
            }

            bool wasActive = upgradeMenuTransform.gameObject.activeSelf;
            upgradeMenuTransform.gameObject.SetActive(false);

            // Clone the UpgradeMenu GameObject
            GameObject clone = UnityEngine.Object.Instantiate(upgradeMenuTransform.gameObject);
            clone.name = "DefaultMenu";
            clone.SetActive(false);

            // Restore original object state
            upgradeMenuTransform.gameObject.SetActive(wasActive);

            // Remove all children that are NOT Panel or SlidingScreen
            foreach (Transform child in clone.transform.Cast<Transform>().ToList())
            {
                if (child.name != "Panel" && child.name != "SlidingScreen")
                    UnityEngine.Object.DestroyImmediate(child.gameObject);
            }

            Transform slidingScreen = clone.transform.Find("SlidingScreen");
            if (slidingScreen != null)
            {
                for (int i = slidingScreen.childCount - 1; i >= 0; i--)
                {
                    UnityEngine.Object.DestroyImmediate(slidingScreen.GetChild(i).gameObject);
                }
            }

            // Move to CardRenderer/MenusAspects
            GameObject cardRendererGO = GameObjectHelper.GetOrCreatePersistentGO("CardRenderer");
            GameObject panelsAspectsGO = GameObjectHelper.GetOrCreateChild(cardRendererGO.transform, "MenusAspects");
            clone.transform.SetParent(panelsAspectsGO.transform, false);

            UnityEngine.Object.DontDestroyOnLoad(clone);
        }
    }
}
