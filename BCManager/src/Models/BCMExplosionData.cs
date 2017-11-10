using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMDamageMultiplier
  {
    public string Type;
    public double Value;
  }

  public class BCMExplosionData
  {
    public int ParticleIndex;
    public int BlockRadius;
    public int EntityRadius;
    public int BuffsRadius;
    //public int BlastPower;
    public double EntityDamage;
    public double BlockDamage;
    public List<BCMDamageMultiplier> DamageMultipliers;
    public List<BCMLootBuffAction> BuffActions = new List<BCMLootBuffAction>();

    public BCMExplosionData(DynamicProperties _properties)
    {
      if (_properties.Values.ContainsKey("Explosion.ParticleIndex"))
      {
        ParticleIndex = (int)Utils.ParseFloat(_properties.Values["Explosion.ParticleIndex"]);
      }
      else
      {
        ParticleIndex = 0;
      }

      if (_properties.Values.ContainsKey("Explosion.RadiusBlocks"))
      {
        BlockRadius = (int)Utils.ParseFloat(_properties.Values["Explosion.RadiusBlocks"]);
      }

      if (_properties.Values.ContainsKey("Explosion.BlockDamage"))
      {
        BlockDamage = Utils.ParseFloat(_properties.Values["Explosion.BlockDamage"]);
      }
      else
      {
        BlockDamage = BlockRadius * BlockRadius;
      }

      if (_properties.Values.ContainsKey("Explosion.RadiusEntities"))
      {
        EntityRadius = (int)Utils.ParseFloat(_properties.Values["Explosion.RadiusEntities"]);
      }

      if (_properties.Values.ContainsKey("Explosion.EntityDamage"))
      {
        EntityDamage = Utils.ParseFloat(_properties.Values["Explosion.EntityDamage"]);
      }
      else
      {
        EntityDamage = 20f * EntityRadius;
      }

      if (_properties.Values.ContainsKey("Explosion.RadiusBuffs"))
      {
        BuffsRadius = (int)Utils.ParseFloat(_properties.Values["Explosion.RadiusBuffs"]);
      }
      else
      {
        BuffsRadius = EntityRadius;
      }

      //if (_properties.Values.ContainsKey("Explosion.BlastPower"))
      //{
      //  BlastPower = (int)Utils.ParseFloat(_properties.Values["Explosion.BlastPower"]);
      //}

      if (_properties.Values.ContainsKey("Explosion.Buff"))
      {
        BuffActions = GetExplosionBuffs(_properties);
      }

      DamageMultipliers = GetDamageMultiplier(_properties);
    }

    private static List<BCMLootBuffAction> GetExplosionBuffs(DynamicProperties _properties)
    {
      var ExplosionBuffs = new List<BCMLootBuffAction>();

      var names = _properties.Values["Explosion.Buff"].Split(',');
      string[] probs = null;
      if (_properties.Values.ContainsKey("Explosion.Buff_chance"))
      {
        probs = _properties.Values["Explosion.Buff_chance"].Split(',');
      }
      for (var i = 0; i < names.Length; i++)
      {
        var name = names[i].Trim();
        double prob = 1;
        if (probs != null && i < probs.Length)
        {
          prob = Utils.ParseFloat(probs[i].Trim());
        }

        ExplosionBuffs.Add(new BCMLootBuffAction { BuffId = name, Chance = prob });
      }

      return ExplosionBuffs;
    }

    public static List<BCMDamageMultiplier> GetDamageMultiplier(DynamicProperties _properties)
    {
      var DamageMultipliers = new List<BCMDamageMultiplier>();

      const string _prefix = "Explosion.DamageBonus.";
      using (var enumerator = _properties.Values.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          var current = enumerator.Current;
          if (current == null) continue;

          if (current.StartsWith(_prefix))
          {
            DamageMultipliers.Add(
              new BCMDamageMultiplier
              {
                Type = current.Substring(_prefix.Length),
                Value = Utils.ParseFloat(_properties.Values[current])
              });
          }
        }
      }

      return DamageMultipliers;
    }
  }
}
