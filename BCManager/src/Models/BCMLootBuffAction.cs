using System;

namespace BCM.Models
{
  public class BCMLootBuffAction
  {
    public string BuffId;
    public double Chance;
    //public bool IsDebuff;

    public BCMLootBuffAction()
    {
    }

    public BCMLootBuffAction(MultiBuffClassAction buffAction)
    {
      BuffId = buffAction.Class.Id;
      Chance = Math.Round(buffAction.Chance, 6);
      //IsDebuff = buffAction.IsDebuffAction;
    }
  }
}
