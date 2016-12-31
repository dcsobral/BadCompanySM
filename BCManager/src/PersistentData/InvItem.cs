using System;
using System.Runtime.Serialization;

namespace BCM.PersistentData
{
  [Serializable]
  public class InvItem
  {
    public string itemName;
    public int count;
    public int quality;
    public InvItem[] parts;
    public string icon = "";
    public string iconcolor = "";
    [OptionalField]
    public int maxUseTimes;
    [OptionalField]
    public int useTimes;

    public InvItem(string itemName, int count, int quality, int maxUseTimes, int maxUse)
    {
      this.itemName = itemName;
      this.count = count;
      this.quality = quality;
      this.maxUseTimes = maxUseTimes;
      this.useTimes = maxUse;
    }
  }
}
