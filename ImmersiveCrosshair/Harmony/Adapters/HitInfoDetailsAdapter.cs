namespace ImmersiveCrosshair.Harmony.Adapters
{
    public class HitInfoDetailsAdapter : IHitInfoDetails
    {
        private readonly HitInfoDetails _hitInfoDetails;

        public HitInfoDetailsAdapter(HitInfoDetails hitInfoDetails)
        {
            _hitInfoDetails = hitInfoDetails;
        }

        public float distanceSq => _hitInfoDetails.distanceSq;
    }
}