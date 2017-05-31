using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListEntityGroups : BCCommandAbstract
  {
    public virtual Dictionary<string, string> jsonObject()
    {
      Dictionary<string, string> data = new Dictionary<string, string>();
      DictionarySave<string, List<SEntityClassAndProb>> groups = EntityGroups.list;

      foreach (string name in groups.Keys)
      {
        Dictionary<string, string> details = new Dictionary<string, string>();
        List<SEntityClassAndProb> groupList = groups[name];

        details.Add("Name", name);
        details.Add("Count", groupList.Count.ToString());

        List<string> groupEntities = new List<string>();

        for (int i = 0; i < groupList.Count; i++)
        {
          Dictionary<string, string> classandprob = new Dictionary<string, string>();
          SEntityClassAndProb group = groupList[i];
          classandprob.Add("entityClassId", group.entityClassId.ToString());
          classandprob.Add("prob", group.prob.ToString());
          classandprob.Add("reqMin", group.reqMin.ToString());
          classandprob.Add("reqMax", group.reqMax.ToString());

          groupEntities.Add(BCUtils.toJson(classandprob));
        }

        string jsonClassandProb = BCUtils.toJson(groupEntities);
        details.Add("GroupList", jsonClassandProb);

        var jsonDetails = BCUtils.toJson(details);
        data.Add(name, jsonDetails);
      }

      return data;
    }

    public override void Process()
    {
      string output = "";
      if (_options.ContainsKey("json"))
      {
        output = BCUtils.toJson(jsonObject());
        SendOutput(output);
      }
      else
      {
        foreach (string entitygroupname in EntityGroups.list.Keys)
        {
          List<SEntityClassAndProb> EG = EntityGroups.list[entitygroupname];
          output += entitygroupname + ":" + EG.Count + "" + _sep;
        }
        SendOutput(output);
      }
    }
  }
}
