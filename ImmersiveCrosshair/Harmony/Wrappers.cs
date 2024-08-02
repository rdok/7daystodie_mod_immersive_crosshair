using System;
using System.Linq;

namespace ImmersiveCrosshair.Harmony
{
    public class LocalPlayerUIWrapper : ILocalPlayerUI
    {
        private readonly LocalPlayerUI _localPlayerUI;

        public LocalPlayerUIWrapper(LocalPlayerUI playerUI)
        {
            _localPlayerUI =
                playerUI ?? throw new ArgumentNullException(nameof(playerUI));
        }

        public T GetComponentInChildren<T>()
        {
            Log.Out("Loading Patch: Requesting component of type " + typeof(T).Name);

            if (typeof(T) != typeof(IGuiWdwInGameHUD))
            {
                Log.Out("Loading Patch: Type is not IGuiWdwInGameHUD, proceeding with generic component retrieval.");
                var component = _localPlayerUI.GetComponentInChildren<T>();

                Log.Out(component == null
                    ? "Loading Patch: Component of type " + typeof(T).Name + " not found."
                    : "Loading Patch: Component of type " + typeof(T).Name + " found.");

                return component;
            }

            Log.Out("Loading Patch: Type is IGuiWdwInGameHUD, attempting to retrieve NGuiWdwInGameHUD.");
            var nGuiComponent = _localPlayerUI.GetComponentInChildren<NGuiWdwInGameHUD>();

            Log.Out(nGuiComponent == null
                ? "Loading Patch: NGuiWdwInGameHUD component not found."
                : "Loading Patch: NGuiWdwInGameHUD component found with type: " + nGuiComponent.GetType().Name);

            if (nGuiComponent != null)
            {
                Log.Out("Loading Patch: Wrapping NGuiWdwInGameHUD with IGuiWdwInGameHUD interface.");
                return (T)(object)new GuiWdwInGameHUDWrapper(nGuiComponent);
            }

            Log.Out("Loading Patch: Failed to wrap NGuiWdwInGameHUD. Returning null.");
            return default;
        }
    }

    public class EntityPlayerLocalWrapper : IEntityPlayerLocal
    {
        private readonly EntityPlayerLocal _entityPlayerLocal;

        public EntityPlayerLocalWrapper(EntityPlayerLocal entityPlayerLocal)
        {
            _entityPlayerLocal = entityPlayerLocal ?? throw new ArgumentNullException(nameof(entityPlayerLocal));
        }

        public ILocalPlayerUI playerUI => new LocalPlayerUIWrapper(_entityPlayerLocal.playerUI);
        public IInventory inventory => new InventoryWrapper(_entityPlayerLocal.inventory);
        public IWorldRayHitInfo HitInfo => new HitInfoWrapper(_entityPlayerLocal.HitInfo);
    }

    public class InventoryWrapper : IInventory
    {
        private readonly Inventory _inventory;

        public InventoryWrapper(Inventory inventory)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
        }

        public IItemValue holdingItemItemValue => new ItemValueWrapper(_inventory.holdingItemItemValue);
    }

    public class HitInfoWrapper : IWorldRayHitInfo
    {
        private readonly WorldRayHitInfo _worldRayHitInfo;

        public HitInfoWrapper(WorldRayHitInfo worldRayHitInfo)
        {
            _worldRayHitInfo = worldRayHitInfo ?? throw new ArgumentNullException(nameof(worldRayHitInfo));
        }

        public bool bHitValid => _worldRayHitInfo.bHitValid;
        public IHitInfoDetails hit => new HitInfoDetailsWrapper(_worldRayHitInfo.hit);
    }

    public class HitInfoDetailsWrapper : IHitInfoDetails
    {
        private readonly HitInfoDetails _hitInfoDetails;

        public HitInfoDetailsWrapper(HitInfoDetails hitInfoDetails)
        {
            _hitInfoDetails = hitInfoDetails;
        }

        public float distanceSq => _hitInfoDetails.distanceSq;
    }


    public class ItemValueWrapper : IItemValue
    {
        private readonly ItemValue _itemValue;

        public ItemValueWrapper(ItemValue itemValue)
        {
            _itemValue = itemValue;
        }

        public IItemClass ItemClass => new ItemClassWrapper(_itemValue.ItemClass);
    }

    public class ItemClassWrapper : IItemClass
    {
        private readonly ItemClass _itemClass;

        public ItemClassWrapper(ItemClass itemClass)
        {
            _itemClass = itemClass;
        }

        public IItemAction[] Actions => _itemClass.Actions?.Select(action => new ItemActionAdapter(action)).ToArray();
    }


    public class GuiWdwInGameHUDWrapper : IGuiWdwInGameHUD
    {
        private readonly NGuiWdwInGameHUD _nGuiWdwInGameHUD;

        public GuiWdwInGameHUDWrapper(NGuiWdwInGameHUD nGuiWdwInGameHUD)
        {
            _nGuiWdwInGameHUD = nGuiWdwInGameHUD;
        }

        public bool showCrosshair
        {
            get => _nGuiWdwInGameHUD.showCrosshair;
            set => _nGuiWdwInGameHUD.showCrosshair = value;
        }
    }


    public class ItemActionAdapter : IItemAction
    {
        private readonly ItemAction _itemAction;

        public ItemActionAdapter(ItemAction itemAction)
        {
            _itemAction = itemAction;
        }

        public bool IsRangedAction => _itemAction is ItemActionRanged;
    }
}