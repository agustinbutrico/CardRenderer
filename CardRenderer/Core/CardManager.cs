using CardsShared;
using UnityEngine;
using UnityEngine.UI;
using TexturesLib.Shared;

namespace CardRenderer.Core
{
    internal class CardManager
    {
        internal static GameObject CreateCardDisplay(
            CardDisplayData cardDisplayData, string parentPath, string sliceVariant = null, string prefabAspectName = null,
            Color? colorVariant = null, Color? textColorVariant = null
        )
        {
            string prefabAspectNameResolved = prefabAspectName ?? "DefaultHorizontalCard";
            string prefabPath = "CardRenderer/CardsAspects/" + prefabAspectNameResolved;
            GameObject prefabGO = GameObject.Find(prefabPath);

            if (prefabGO == null)
            {
                Plugin.Log.LogError($"Card prefab not found at '{prefabPath}'. Cannot display card.");
                return null;
            }

            // Find the parent GameObject where the card will be displayed
            GameObject parentGO = GameObject.Find(parentPath);
            if (parentGO == null)
            {
                Plugin.Log.LogError($"Parent GameObject not found at '{parentPath}'. Cannot display card.");
                return null;
            }

            // Instantiate card under the display parent
            GameObject cardGO = Object.Instantiate(prefabGO, parentGO.transform);
            cardGO.name = cardDisplayData.UnlockName;
            cardGO.SetActive(true);

            // Swap slice sprite on the card's background Image
            var cardSlice = cardGO.GetComponent<Image>();
            if (cardSlice != null)
            {
                string sliceName = sliceVariant ?? "UI9SliceBrownFilled";
                Sprite sliceSprite = SpriteHelper.FindSpriteByName(sliceName);
                if (sliceSprite != null)
                {
                    cardSlice.sprite = sliceSprite;
                    cardSlice.type = Image.Type.Sliced;
                }
                else
                {
                    Plugin.Log.LogWarning($"Slice sprite '{sliceName}' not found. Skipped swap for '{cardGO.name}'.");
                }
            }
            else
            {
                Plugin.Log.LogWarning($"No Image component found on '{cardGO.name}' to assign slice sprite.");
            }

            // Set sprite and color on child image
            var cardImage = cardGO.transform.Find("Image")?.GetComponent<Image>();
            if (cardImage != null)
            {
                cardImage.sprite = cardDisplayData.Sprite ?? SpriteHelper.FindSpriteByName(cardDisplayData.SpriteName);

                if (colorVariant.HasValue)
                    cardImage.color = colorVariant ?? ColorsHelper.DeepBrown;
            }

            // Set title text
            var cardTitle = cardGO.transform.Find("Title")?.GetComponent<Text>();
            if (cardTitle != null)
            {
                cardTitle.text = cardDisplayData.Title;

                if (textColorVariant.HasValue)
                    cardTitle.color = textColorVariant.Value;
            }

            // Set description text
            var cardDescription = cardGO.transform.Find("Description")?.GetComponent<Text>();
            if (cardDescription != null)
            {
                cardDescription.text = cardDisplayData.Description;

                if (textColorVariant.HasValue)
                    cardDescription.color = textColorVariant.Value;
            }

            return cardGO;
        }

        internal static void ConfigureCardPosition(
            GameObject cardGO, Vector3? localPosition = null, Vector2? anchoredPosition = null
        )
        {
            if (cardGO == null)
            {
                Plugin.Log.LogError("ConfigureCardRectPosition: card GameObject is null.");
                return;
            }

            RectTransform rect = cardGO.GetComponent<RectTransform>();
            if (rect == null)
            {
                Plugin.Log.LogError($"ConfigureCardRectPosition: No RectTransform on '{cardGO.name}'.");
                return;
            }

            // Set anchor and pivot to top-left
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);

            if (anchoredPosition.HasValue) rect.anchoredPosition = anchoredPosition.Value;
            if (localPosition.HasValue) rect.localPosition = localPosition.Value;
        }

        internal static void SwapCardSlice(
            string cardPath, string sliceVariant = null,
            Color? colorVariant = null, Color? textColorVariant = null
        )
        {
            Transform t = InactiveFinder.FindByPathIncludingInactive(cardPath);
            GameObject cardGO = t?.gameObject;

            if (cardGO == null)
            {
                Plugin.Log.LogWarning($"Card not found at path '{cardPath}'. Cannot swap slice.");
                return;
            }

            var cardSlice = cardGO.GetComponent<Image>();
            if (cardSlice != null)
            {
                string sliceName = sliceVariant ?? "UI9SliceBrown";
                Sprite sliceSprite = SpriteHelper.FindSpriteByName(sliceName);
                if (sliceSprite != null)
                {
                    cardSlice.sprite = sliceSprite;
                    cardSlice.type = Image.Type.Sliced;
                }
                else
                {
                    Plugin.Log.LogWarning($"Slice sprite '{sliceName}' not found. Skipped swap for '{cardGO.name}'.");
                }
            }
            else
            {
                Plugin.Log.LogWarning($"No Image component found on panel '{cardGO.name}' to assign slice sprite.");
            }

            var cardImage = cardGO.transform.Find("Image")?.GetComponent<Image>();
            if (cardImage != null && colorVariant != null)
            {
                cardImage.color = colorVariant.Value;
            }
            var cardTitle = cardGO.transform.Find("Title")?.GetComponent<Text>();
            if (cardTitle != null && textColorVariant != null)
            {
                cardTitle.color = textColorVariant.Value;
            }
            var cardDescription = cardGO.transform.Find("Description")?.GetComponent<Text>();
            if (cardDescription != null && textColorVariant != null)
            {
                cardDescription.color = textColorVariant.Value;
            }
        }
    }
}
