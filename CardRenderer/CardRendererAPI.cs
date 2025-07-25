using CardsShared;
using CardRenderer.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardRenderer
{
    public static class CardRendererAPI
    {
        public static GameObject NewMenu(
            string parentPath, string newRootName, string prefabAspectName = null
        )
        {
            return MenuManager.NewMenu(parentPath, newRootName, prefabAspectName);
        }

        public static GameObject NewPanel(
            string parentPath, string panelName, Vector2? startPosition = null,
            bool rightToLeft = false, string spriteName = "LargeUI9SliceBlue"
        )
        {
            Vector2 _startPosition = startPosition ?? new Vector2(0, 0);

            return MenuManager.NewPanel(parentPath, panelName, _startPosition, rightToLeft, spriteName);
        }

        public static GameObject ResizeToFitChildren(
            GameObject container,
            bool rightToLeft = false
        )
        {

            return MenuManager.ResizeToFitChildren(container, rightToLeft);
        }

        public static GameObject ResizeToFitChildrenVertical(
            GameObject container,
            bool rightToLeft = false
        )
        {
            return MenuManager.ResizeToFitChildrenVertical(container, rightToLeft);
        }

        public static GameObject CreateCardDisplay(
            CardDisplayData cardDisplayData, string parentPath = null, string sliceVariant = null,
            string prefabAspectName = null, Color? colorVariant = null, Color? textColorVariant = null
        )
        {
            return Core.CardManager.CreateCardDisplay(
                cardDisplayData, parentPath, sliceVariant,
                prefabAspectName, colorVariant, textColorVariant
            );
        }

        public static void ConfigureCardPosition(
            GameObject cardGO, Vector3? localPosition = null, Vector2? anchoredPosition = null
        )
        {
            Core.CardManager.ConfigureCardPosition(
                cardGO, localPosition, anchoredPosition
            );
        }

        public static void SwapCardSlice(
            string cardPath, string sliceVariant = null,
            Color? colorVariant = null, Color? textColorVariant = null
        )
        {
            Core.CardManager.SwapCardSlice(cardPath, sliceVariant, colorVariant, textColorVariant);
        }

        public static void DisplayCardTree(
            List<CardTree> roots, string parentPath, string sliceVariant = null, string prefabAspectName = null,
            Color? colorVariant = null, Color? textColorVariant = null, bool rightToLeft = false,
            float? xSpacing = null, float? ySpacing = null, float? depth0YSpacing = null, float? depth1YSpacing = null
        )
        {
            float _xSpacing = xSpacing ?? 225f;
            float _ySpacing = ySpacing ?? -90f;
            float _depth0YSpacing = depth0YSpacing ?? -7f;
            float _depth1YSpacing = depth1YSpacing ?? 0f;
            float directionMultiplier = rightToLeft ? -1f : 1f;

            Vector2 startPos = new Vector2(20f, -20f);

            foreach (var root in roots)
            {
                CardTreeManager.DisplayNodeRecursive(
                    root, parentPath, sliceVariant, prefabAspectName, colorVariant, textColorVariant, directionMultiplier,
                    startPos, 0, _xSpacing, _ySpacing, _depth0YSpacing, _depth1YSpacing
                );

                startPos.y += _ySpacing * CardTreeManager.CountLeaves(root);
            }
        }

        public static void DisplayCardTreeVertical(
            List<CardTree> roots, string parentPath, string sliceVariant = null, string prefabAspectName = null,
            Color? colorVariant = null, Color? textColorVariant = null, bool bottomToTop = false,
            float? xSpacing = null, float? ySpacing = null, float? depth0XSpacing = null, float? depth1XSpacing = null
        )
        {
            float _xSpacing = xSpacing ?? 225f;
            float _ySpacing = ySpacing ?? -90f;
            float _depth0XSpacing = depth0XSpacing ?? 7f;
            float _depth1XSpacing = depth1XSpacing ?? 0f;
            float directionMultiplier = bottomToTop ? -1f : 1f;

            Vector2 startPos = new Vector2(20f, -20f);

            foreach (var root in roots)
            {
                CardTreeManager.DisplayNodeRecursiveVertical(
                    root, parentPath, sliceVariant, prefabAspectName, colorVariant, textColorVariant, directionMultiplier,
                    startPos, 0, _xSpacing, _ySpacing, _depth0XSpacing, _depth1XSpacing
                );

                startPos.x += _xSpacing * CardTreeManager.CountLeaves(root);
            }
        }

        public static IEnumerator WaitForCardAspect(string cardAspect)
        {
            string path = $"CardRenderer/CardsAspects/{cardAspect}";
            while (GameObject.Find(path) == null)
                yield return null;
        }

        public static IEnumerator WaitForMenuAspect(string menuAspect)
        {
            string path = $"CardRenderer/MenusAspects/{menuAspect}";
            while (GameObject.Find(path) == null)
                yield return null;
        }
    }
}
