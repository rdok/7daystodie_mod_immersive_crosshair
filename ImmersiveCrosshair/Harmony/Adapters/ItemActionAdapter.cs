using System.Linq;
using JetBrains.Annotations;

namespace ImmersiveCrosshair.Harmony.Adapters
{
    public class ItemActionAdapter : IItemAction
    {
        private static readonly ILogger Logger = new Logger();
        private readonly ItemAction _itemAction;

        public ItemActionAdapter(ItemAction itemAction)
        {
            _itemAction = itemAction;
        }

        bool IItemAction.IsTool =>
            HasTags(_itemAction, new[] { "tool", "harvestingSkill", "knife", "perkBrawler" }, TagCheckType.Any);

        bool IItemAction.IsMelee =>
            HasTags(_itemAction, new[] { "meleeWeapon" }, TagCheckType.Any);

        public bool IsBowWithNoSights => HasTags(_itemAction, new[] { "bow" }, TagCheckType.All) &&
                                         _itemAction?.item?.Name != "gunBowT3CompoundBow";

        private static bool HasTags([CanBeNull] ItemAction _itemAction, string[] tagNames, TagCheckType checkType)
        {
            if (_itemAction?.item == null) return false;

            if (!_itemAction.item.Properties.Values.ContainsKey("Tags"))
            {
                Logger.Debug("HasAnyTag: No tags in _itemAction.item.Properties.Values");
                return false;
            }

            var tags = _itemAction.item.Properties.Values["Tags"];
            Logger.Debug($"HasAnyTag: Item Tags: {tags}");
            var tagsArray = tags.Split(',');
            bool hasTags;

            switch (checkType)
            {
                case TagCheckType.Any:
                    hasTags = tagNames.Any(tagName => tagsArray.Contains(tagName));
                    break;
                case TagCheckType.All:
                    hasTags = tagNames.All(tagName => tagsArray.Contains(tagName));
                    break;
                default:
                    Logger.Debug("HasTags: Invalid check type");
                    return false;
            }

            Logger.Debug(hasTags
                ? $"The item has {checkType.ToString().ToLower()} of the specified tags."
                : $"The item does not have {checkType.ToString().ToLower()} of the specified tags.");

            return hasTags;
        }

        private enum TagCheckType
        {
            Any,
            All
        }
    }
}