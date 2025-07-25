using System.Collections.Generic;

namespace CardsShared
{
    public static class CardTreeUtils
    {
        public static List<CardDisplayData> GetAllDescendants(CardTree root)
        {
            var result = new List<CardDisplayData>();
            CollectDescendants(root, result);
            return result;
        }

        private static void CollectDescendants(CardTree node, List<CardDisplayData> collected)
        {
            foreach (var child in node.Children)
            {
                collected.Add(child.Card);
                CollectDescendants(child, collected);
            }
        }
    }
}
