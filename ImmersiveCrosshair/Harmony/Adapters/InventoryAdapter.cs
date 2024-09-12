using System;

namespace ImmersiveCrosshair.Harmony.Adapters
{
    public class InventoryAdapter : IInventory
    {
        private readonly Inventory _inventory;

        public InventoryAdapter(Inventory inventory)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
        }

        public IItemValue holdingItemItemValue => new ItemValueAdapter(_inventory.holdingItemItemValue);
    }
}