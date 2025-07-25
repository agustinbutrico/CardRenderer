using System.Collections.Generic;
using System.Linq;

namespace CardsShared
{
    public class CardTree
    {
        public CardDisplayData Card;
        public List<CardTree> Children = new List<CardTree>();
    }

    public static class CardTreeBuilder
    {
        public static List<CardTree> BuildCardTrees(List<CardDisplayData> cards)
        {
            var validCards = cards.Where(c => !string.IsNullOrEmpty(c.UnlockName)).ToList();

            var reverseLookup = new HashSet<string>(
                validCards
                    .Where(c => c.Unlocks != null)
                    .SelectMany(c => c.Unlocks)
                    .Where(unlock => !string.IsNullOrEmpty(unlock))
            );

            var roots = validCards.Where(c => !reverseLookup.Contains(c.UnlockName)).ToList();

            var nodeMap = new Dictionary<string, CardTree>();

            foreach (var card in validCards)
            {
                if (nodeMap.ContainsKey(card.UnlockName))
                {
                    UnityEngine.Debug.LogWarning($"[CardTreeBuilder] Duplicate UnlockName detected and skipped: {card.UnlockName}");
                    continue;
                }

                nodeMap[card.UnlockName] = new CardTree { Card = card };
            }

            foreach (var node in nodeMap.Values)
            {
                var unlocks = node.Card.Unlocks;
                if (unlocks == null) continue;

                foreach (var unlock in unlocks)
                {
                    if (string.IsNullOrEmpty(unlock)) continue;
                    if (nodeMap.TryGetValue(unlock, out var child))
                        node.Children.Add(child);
                }
            }

            return roots
                .Where(r => nodeMap.ContainsKey(r.UnlockName))
                .Select(r => nodeMap[r.UnlockName])
                .ToList();
        }
    }
}
