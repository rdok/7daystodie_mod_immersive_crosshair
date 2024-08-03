using System;
using System.Linq;

namespace ImmersiveCrosshair.Harmony
{
    public class LocalPlayerUIWrapper : ILocalPlayerUI
    {
        private readonly LocalPlayerUI _localPlayerUI;
        private readonly ILogger _logger = new Logger();

        public LocalPlayerUIWrapper(LocalPlayerUI playerUI)
        {
            _localPlayerUI =
                playerUI ?? throw new ArgumentNullException(nameof(playerUI));
        }

        public T GetComponentInChildren<T>()
        {
            _logger.Info("Loading Patch: Requesting component of type " + typeof(T).Name);

            if (typeof(T) != typeof(IGuiWdwInGameHUD))
            {
                _logger.Info(
                    "Loading Patch: Type is not IGuiWdwInGameHUD, proceeding with generic component retrieval.");
                var component = _localPlayerUI.GetComponentInChildren<T>();

                _logger.Info(component == null
                    ? "Loading Patch: Component of type " + typeof(T).Name + " not found."
                    : "Loading Patch: Component of type " + typeof(T).Name + " found.");

                return component;
            }

            _logger.Info("Loading Patch: Type is IGuiWdwInGameHUD, attempting to retrieve NGuiWdwInGameHUD.");
            var nGuiComponent = _localPlayerUI.GetComponentInChildren<NGuiWdwInGameHUD>();

            _logger.Info(nGuiComponent == null
                ? "Loading Patch: NGuiWdwInGameHUD component not found."
                : "Loading Patch: NGuiWdwInGameHUD component found with type: " + nGuiComponent.GetType().Name);

            if (nGuiComponent != null)
            {
                _logger.Info("Loading Patch: Wrapping NGuiWdwInGameHUD with IGuiWdwInGameHUD interface.");
                return (T)(object)new GuiWdwInGameHUDWrapper(nGuiComponent);
            }

            _logger.Info("Loading Patch: Failed to wrap NGuiWdwInGameHUD. Returning null.");
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

        public IItemAction[] Actions => _itemClass
            .Actions?
            .Select(action => new ItemActionAdapter(action))
            .ToArray();
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

        bool IItemAction.IsRanged => _itemAction is ItemActionRanged;
        bool IItemAction.IsRepair => _itemAction is ItemActionRepair;
        bool IItemAction.IsTerrain => _itemAction is ItemActionTerrainTool;

        public bool IsHarvest
        {
            get
            {
                if (_itemAction == null)
                {
                    Log.Out("IsHarvestingTool: ItemAction is null.");
                    return false;
                }

                Log.Out($"IsHarvestingTool: Checking harvesting capabilities for {_itemAction.GetType().Name}.");

                if (!_itemAction.bUseParticleHarvesting) return false;
                
                Log.Out("IsHarvestingTool: Item uses particle harvesting.");

                return true;
            }
        }


        public bool IsNull()
        {
            return _itemAction == null;
        }

        public new object GetType()
        {
            return _itemAction.GetType();
        }
    }
}