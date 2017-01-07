using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListLoot : BCCommandAbstract
  {
    private string GetHeader(LootContainer loot)
    {
      string output = "";
      output += "[";
      output += "min=" + loot.minCount + _sep;
      output += "max=" + loot.maxCount + _sep;
      output += "size=(" + loot.size.x + "x" + loot.size.y + ")" + _sep;
      output += "opentime=" + loot.openTime + _sep;
      output += "soundopen=" + loot.soundOpen + _sep;
      output += "soundclose=" + loot.soundClose + _sep;
      output += "destroy=" + loot.bDestroyOnClose;
      if (loot.BuffActions != null)
      {
        if (loot.BuffActions.Count > 0)
        {
          output += _sep + "buffactions(" + loot.BuffActions.Count + ")={";
          bool first = true;
          foreach (MultiBuffClassAction buff in loot.BuffActions)
          {
            if (!first) { output += _sep; } else { first = false; }
            output += "" + buff.Class.Id + "(" + buff.Chance + ")";
          }
          output += "}";
        }
      }
      output += "]";

      return output;
    }
    private string GetItems(List<LootContainer.LootEntry> lootentry)
    {
      string output = "";
      bool first = true;
      output += "items(" + lootentry.Count + ")={";
      if (lootentry.Count > 0)
      {
        foreach (LootContainer.LootEntry entry in lootentry)
        {
          if (!first) { output += _sep; } else { first = false; }
          if (entry.item != null)
          {
            int ivt = entry.item.itemValue.type;
            int ivt2 = ivt;
            if (ivt > 4096)
            {
              ivt2 = ivt - 4096;
            }
            if (ItemClass.list[ivt] != null)
            {
              ItemClass ic = ItemClass.list[ivt];
              output += "item=" + ic.Name + "(" + ivt2 + ")" + _sep;

            }
            else
            {
              output += "Invalid Item id" + ivt2;
            }
          }
          if (entry.group != null)
          {
            output += "group=" + entry.group.name + _sep;
          }
          output += "prob=" + entry.prob;
        }
        if (_options.ContainsKey("expanded"))
        {
          // todo: show fully expanded loot list with change for each item and count range
        }
      }
      output += _sep;

      return output;
    }

    public override void Process()
    {
      string output = "";
      if (_options.ContainsKey("groups"))
      {
        Dictionary<string, LootContainer.LootGroup> lootgroups = LootContainer.lootGroups;
        foreach (string group in lootgroups.Keys)
        {
          if (_params.Count > 0)
          {
            bool match = true;
            foreach (string param in _params)
            {
              match = match && group.Contains(param);
            }
            if (match)
            {
              output += group + _sep;
              output += GetItems(lootgroups[group].items);
              // todo: search to allow for special chars like + in name
              // todo: output group details
            }
          }
          else
          {
            output += group + _sep;
          }
        }
      }
      else
      {
        if (_params.Count == 0)
        {
          for (int i = 0; i <= LootContainer.lootList.Length - 1; i++)
          {
            if (LootContainer.lootList[i] != null)
            {
              LootContainer loot = LootContainer.lootList[i];
              output += loot.Id;
              if (_options.ContainsKey("details"))
              {
                if (loot.itemsToSpawn.Count > 0)
                {
                  output += "(" + loot.itemsToSpawn.Count + ")";
                }
                output += GetHeader(loot);
              }
              output += _sep;
            }
          }
        }
        if (_params.Count == 1)
        {
          if (_options.ContainsKey("groups"))
          {
            Dictionary<string, LootContainer.LootGroup> lootgroups = LootContainer.lootGroups;
            foreach (string group in lootgroups.Keys)
            {
              output += group + _sep;
            }
          }
          else
          {
            int i = 0;
            if (!int.TryParse(_params[0], out i))
            {
              output += "Loot Container id " + _params[0] + " isn't a number.";
            }
            else
            {
              if (LootContainer.lootList[i] != null)
              {
                LootContainer loot = LootContainer.lootList[i];
                output += "id=" + loot.Id;
                output += GetHeader(loot);
                output += GetItems(loot.itemsToSpawn);
              }
              else
              {
                output += "No Loot Container with id " + _params[0] + " exists.";
              }
            }
          }
        }
      }
      SendOutput(output);
    }
  }
}
