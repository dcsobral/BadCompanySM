using JetBrains.Annotations;

namespace BCM.Models
{
  public struct BCMLockedItem
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public int UnlockLevel;
    [UsedImplicitly] public string ItemType;

    public BCMLockedItem(Skill.LockedItem item)
    {
      Name = item.Name;
      UnlockLevel = item.UnlockLevel;
      ItemType = item.ItemType.ToString();
    }
  }
}
