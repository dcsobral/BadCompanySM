using RWG2.Rules;
using System;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMFilterData
  {
    [UsedImplicitly] public double Prob;
    [UsedImplicitly] public BCMVector2 Grid;

    public BCMFilterData([NotNull] FilterData filterData)
    {
      Prob = Math.Round(filterData.Probability, 3);
      if (!filterData.HasGridPosition) return;

      Grid = new BCMVector2(filterData.GridPosition);
    }
  }
}
