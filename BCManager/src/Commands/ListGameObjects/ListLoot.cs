using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListLoot : BCCommandAbstract
  {
    public virtual Dictionary<string, string> jsonObject()
    {
      Dictionary<string, string> data = new Dictionary<string, string>();

      if (_options.ContainsKey("containers"))
      {
        LootContainer[] containers = LootContainer.lootList;

        for (var i = 0; i <= containers.Length - 1; i++)
        {
          if (containers[i] != null)
          {
            var loot = containers[i];

            Dictionary<string, string> details = new Dictionary<string, string>();

            details.Add("Id", loot.Id.ToString());
            details.Add("bDestroyOnClose", loot.bDestroyOnClose.ToString());
            details.Add("lootQualityTemplate", (loot.lootQualityTemplate != null ? loot.lootQualityTemplate : ""));
            details.Add("maxCount", loot.maxCount.ToString());
            details.Add("minCount", loot.minCount.ToString());
            details.Add("openTime", loot.openTime.ToString());
            details.Add("soundClose", (loot.soundClose != null ? loot.soundClose : ""));
            details.Add("soundOpen", (loot.soundOpen != null ? loot.soundOpen : "" ));
            if (loot.size != null)
            {
              details.Add("size_x", loot.size.x.ToString());
              details.Add("size_y", loot.size.y.ToString());
            }

            //BUFF ACTIONS
            Dictionary<string, string> buffActions = new Dictionary<string, string>();
            if (loot.BuffActions != null)
            {
              var b = 0;
              foreach (MultiBuffClassAction current in loot.BuffActions)
              {
                Dictionary<string, string> multi = new Dictionary<string, string>();
                multi["Chance"] = current.Chance.ToString();
                multi["IsDebuffAction"] = current.IsDebuffAction.ToString();
                multi["BuffClassId"] = (current.Class != null && current.Class.Id != null ? current.Class.Id : "");
                string jsonMulti = BCUtils.toJson(multi);

                buffActions.Add(b.ToString(), jsonMulti);
                b++;
              }
            }
            string jsonBuffActions = BCUtils.toJson(buffActions);
            details.Add("BuffActions", jsonBuffActions);

            //ITEMS TO SPAWN
            List<string> itemsToSpawn = new List<string>();
            foreach (LootContainer.LootEntry current in loot.itemsToSpawn)
            {
              Dictionary<string, string> lootItems = new Dictionary<string, string>();
              lootItems.Add("item", (current.item != null ? current.item.itemValue.type.ToString() : ""));
              lootItems.Add("group", current.group != null ? current.group.name : "");
              lootItems.Add("lootProbTemplate", (current.lootProbTemplate != null ? current.lootProbTemplate : ""));
              lootItems.Add("minCount", current.minCount.ToString());
              lootItems.Add("maxCount", current.maxCount.ToString());
              lootItems.Add("minLevel", current.minLevel.ToString());
              lootItems.Add("maxLevel", current.maxLevel.ToString());
              lootItems.Add("minQuality", current.minQuality.ToString());
              lootItems.Add("maxQuality", current.maxQuality.ToString());
              lootItems.Add("prob", current.prob.ToString());

              string jsonLootItems = BCUtils.toJson(lootItems);
              itemsToSpawn.Add(jsonLootItems);
            }
            string jsonItemsToSpawn = BCUtils.toJson(itemsToSpawn);
            details.Add("itemsToSpawn", jsonItemsToSpawn);

            string jsonDetails = BCUtils.toJson(details);
            data.Add(i.ToString(), jsonDetails);
          }
        }
      }

      if (_options.ContainsKey("groups"))
      { 
        Dictionary<string, LootContainer.LootGroup>.ValueCollection groups = LootContainer.lootGroups.Values;

        if (groups != null)
        {
          var i = 0;
          foreach (var group in groups)
          {
            Dictionary<string, string> details = new Dictionary<string, string>();
            List<string> items = new List<string>();

            if (group != null)
            {
              details.Add("Name", (group.name != null ? group.name : ""));
              details.Add("lootQualityTemplate", (group.lootQualityTemplate != null ? group.lootQualityTemplate : ""));
              details.Add("minCount", group.minCount.ToString());
              details.Add("maxCount", group.maxCount.ToString());
              details.Add("minLevel", group.minLevel.ToString());
              details.Add("maxLevel", group.maxLevel.ToString());
              details.Add("minQuality", group.minQuality.ToString());
              details.Add("maxQuality", group.maxQuality.ToString());

              //ITEMS
              foreach (LootContainer.LootEntry current in group.items)
              {
                Dictionary<string, string> lootItems = new Dictionary<string, string>();
                lootItems.Add("item", (current.item != null ? current.item.itemValue.type.ToString() : ""));
                lootItems.Add("group", current.group != null ? current.group.name : "");
                lootItems.Add("lootProbTemplate", (current.lootProbTemplate != null ? current.lootProbTemplate : ""));
                lootItems.Add("minCount", current.minCount.ToString());
                lootItems.Add("maxCount", current.maxCount.ToString());
                lootItems.Add("minLevel", current.minLevel.ToString());
                lootItems.Add("maxLevel", current.maxLevel.ToString());
                lootItems.Add("minQuality", current.minQuality.ToString());
                lootItems.Add("maxQuality", current.maxQuality.ToString());
                lootItems.Add("prob", current.prob.ToString());

                string jsonLootItems = BCUtils.toJson(lootItems);
                items.Add(jsonLootItems);
              }
            }

            string jsonItems = BCUtils.toJson(items);
            details.Add("items", jsonItems);

            string jsonDetails = BCUtils.toJson(details);
            data.Add(i.ToString(), jsonDetails);
            i++;
          }
        }
      }

      return data;
    }

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
      if (_options.ContainsKey("json"))
      {
        if (_options.ContainsKey("tag"))
        {
          if (_options["tag"] == null)
          {
            _options["tag"] = "bc-loot";
          }

          SendOutput("{\"tag\":\"" + _options["tag"] + "\",\"data\":" + BCUtils.toJson(jsonObject()) + "}");
        }
        else
        {
          SendOutput(BCUtils.toJson(jsonObject()));
        }
      }
      else
      {
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
}
