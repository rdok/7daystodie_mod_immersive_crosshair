using System;
using UnityEngine;

namespace ImmersiveCrosshair.Harmony.Adapters
{
    public class WorldHitInfoAdapter : IWorldRayHitInfo
    {
        private static readonly ILogger Logger = new Logger();
        private readonly WorldRayHitInfo _worldRayHitInfo;

        public WorldHitInfoAdapter(WorldRayHitInfo worldRayHitInfo)
        {
            _worldRayHitInfo = worldRayHitInfo ?? throw new ArgumentNullException(nameof(worldRayHitInfo));
        }

        public bool bHitValid => _worldRayHitInfo.bHitValid;
        public IHitInfoDetails hit => new HitInfoDetailsAdapter(_worldRayHitInfo.hit);

        public string tag
        {
            get => _worldRayHitInfo.tag;
            set => _worldRayHitInfo.tag = value;
        }

        public HitInfoDetails fmcHit
        {
            get => _worldRayHitInfo.fmcHit;
            set => _worldRayHitInfo.fmcHit = value;
        }

        public Transform transform
        {
            get => _worldRayHitInfo.transform;
            set => _worldRayHitInfo.transform = value;
        }

        public Collider hitCollider
        {
            get => _worldRayHitInfo.hitCollider;
            set => _worldRayHitInfo.hitCollider = value;
        }
    }
}