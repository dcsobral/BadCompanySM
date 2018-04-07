using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMBuffInfo
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public string Id;
    [UsedImplicitly] public string Duration;
    [UsedImplicitly] public string Percent;

    public BCMBuffInfo(MultiBuff buff)
    {
      Name = buff.Name;
      Id = buff.MultiBuffClass.Id;
      Duration = $"{buff.MultiBuffClass.FDuration * buff.Timer.TimeFraction:0}/{buff.MultiBuffClass.FDuration}(s)";
      Percent = $"{buff.Timer.TimeFraction * 100:0.0}%";
    }
  }
}
