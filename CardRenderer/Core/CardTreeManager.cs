using CardsShared;
using System.Linq;
using UnityEngine;

namespace CardRenderer.Core
{
    internal static class CardTreeManager
    {
        internal static void DisplayNodeRecursive(
            CardTree node, string parentPath, string sliceVariant, string prefabAspectName,
            Color? colorVariant, Color? textColorVariant, float directionMultiplier,
            Vector2 position, int depth, float xSpacing, float ySpacing, float depth0YSpacing, float depth1YSpacing
        )
        {
            Vector2 cardPos = new Vector2(position.x + depth * xSpacing * directionMultiplier, position.y);

            var cardGO = CardManager.CreateCardDisplay(node.Card, parentPath, sliceVariant, prefabAspectName, colorVariant, textColorVariant);
            CardManager.ConfigureCardPosition(cardGO, anchoredPosition: cardPos);

            float childOffsetY = 0;
            foreach (var child in node.Children)
            {
                float subtreeHeight = CountLeaves(child);
                Vector2 childPos = new Vector2(position.x, position.y + childOffsetY);
                DisplayNodeRecursive(
                    child, parentPath, sliceVariant, prefabAspectName,
                    colorVariant, textColorVariant, directionMultiplier,
                    childPos, depth + 1, xSpacing, ySpacing, depth0YSpacing, depth1YSpacing
                );

                float effectiveYSpacing = ySpacing;
                if (depth == 0)
                    effectiveYSpacing += depth0YSpacing;
                else if (depth == 1)
                    effectiveYSpacing += depth1YSpacing;

                childOffsetY += subtreeHeight * effectiveYSpacing;
            }
        }

        internal static void DisplayNodeRecursiveVertical(
            CardTree node, string parentPath, string sliceVariant, string prefabAspectName,
            Color? colorVariant, Color? textColorVariant, float directionMultiplier,
            Vector2 position, int depth, float xSpacing, float ySpacing, float depth0XSpacing, float depth1XSpacing
        )
        {
            Vector2 cardPos = new Vector2(position.x, position.y + depth * ySpacing * directionMultiplier);

            var cardGO = CardManager.CreateCardDisplay(node.Card, parentPath, sliceVariant, prefabAspectName, colorVariant, textColorVariant);
            CardManager.ConfigureCardPosition(cardGO, anchoredPosition: cardPos);

            float childOffsetX = 0;
            foreach (var child in node.Children)
            {
                float subtreeWidth = CountLeaves(child);
                Vector2 childPos = new Vector2(position.x + childOffsetX, position.y);
                DisplayNodeRecursiveVertical(
                    child, parentPath, sliceVariant, prefabAspectName,
                    colorVariant, textColorVariant, directionMultiplier,
                    childPos, depth + 1, xSpacing, ySpacing, depth0XSpacing, depth1XSpacing
                );

                float effectiveXSpacing = xSpacing;
                if (depth == 0)
                    effectiveXSpacing += depth0XSpacing;
                else if (depth == 1)
                    effectiveXSpacing += depth1XSpacing;

                childOffsetX += subtreeWidth * effectiveXSpacing;
            }
        }

        internal static int CountLeaves(CardTree node)
        {
            if (node.Children.Count == 0)
                return 1;

            return node.Children.Sum(CountLeaves);
        }
    }
}
