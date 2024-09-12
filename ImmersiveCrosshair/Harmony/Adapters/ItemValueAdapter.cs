namespace ImmersiveCrosshair.Harmony.Adapters
{
    public class ItemValueAdapter : IItemValue
    {
        private readonly ItemValue _itemValue;

        public ItemValueAdapter(ItemValue itemValue)
        {
            _itemValue = itemValue;
        }

        public IItemClass ItemClass => new ItemClassAdapter(_itemValue.ItemClass);
    }
}