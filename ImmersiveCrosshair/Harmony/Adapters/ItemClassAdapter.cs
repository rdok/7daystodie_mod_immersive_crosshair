using System.Linq;

namespace ImmersiveCrosshair.Harmony.Adapters
{
    public class ItemClassAdapter : IItemClass
    {
        private readonly ItemClass _itemClass;

        public ItemClassAdapter(ItemClass itemClass)
        {
            _itemClass = itemClass;
        }

        public IItemAction[] Actions => _itemClass
            .Actions?
            .Select(action => new ItemActionAdapter(action))
            .ToArray();
    }
}