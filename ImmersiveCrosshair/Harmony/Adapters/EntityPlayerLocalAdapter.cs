using System;

namespace ImmersiveCrosshair.Harmony.Adapters
{
    public class EntityPlayerLocalAdapter : IEntityPlayerLocal
    {
        private readonly EntityPlayerLocal _entityPlayerLocal;

        public EntityPlayerLocalAdapter(EntityPlayerLocal entityPlayerLocal)
        {
            _entityPlayerLocal = entityPlayerLocal ?? throw new ArgumentNullException(nameof(entityPlayerLocal));
        }

        public ILocalPlayerUI playerUI => new LocalPlayerUIAdapter(_entityPlayerLocal.playerUI);
        public IInventory inventory => new InventoryAdapter(_entityPlayerLocal.inventory);
        public IWorldRayHitInfo HitInfo => new WorldHitInfoAdapter(_entityPlayerLocal.HitInfo);

        public bool bFirstPersonView
        {
            get => _entityPlayerLocal.bFirstPersonView;
            set => _entityPlayerLocal.bFirstPersonView = value;
        }
    }
}