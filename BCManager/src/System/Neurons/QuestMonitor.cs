using BCM.Models;
using BCM.PersistentData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BCM.Neurons
{
  public class QuestMonitor : NeuronAbstract
  {
    private PersistentContainer _pcCache = null;
    public QuestMonitor()
    {
      //Log.Out(Config.ModPrefix + " Added Quest Monitor");
    }
    public override bool Fire(int b)
    {
      // todo: move this to the abstract object??
      if (_pcCache == null && PersistentContainer.IsLoaded)
        {
        try
        {
          _pcCache = PersistentContainer.Instance.Clone();
        }
        catch { Log.Out(Config.ModPrefix + " CACHE CLONE ERROR"); }
      }

      // all Fire events should be wrapping in exception capture so that an error doesnt break the heartbeat
      try
      {
        if (ConnectionManager.Instance.ClientCount() > 0)
        {
          using (List<ClientInfo>.Enumerator enumerator = ConnectionManager.Instance.GetClients().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string playerName = enumerator.Current.playerName;
              string playerId = enumerator.Current.playerId;

              PDQuests _currentQuests = null;
              PDQuests _cacheQuests = null;
              try
              {
                Player pcurr = PersistentContainer.Instance.Players[playerId, false];
                Player pcache = _pcCache.Players[playerId, false];
                if (pcurr != null)
                {
                  _currentQuests = pcurr.Quests;
                }
                if (pcache != null)
                {
                  _cacheQuests = pcache.Quests;
                }
              }
              catch (Exception e)
              { Log.Out(Config.ModPrefix + " QuestMonitoring Fire Exception getting quest lists: " + e); }

              List<PDQuest> _changedQuests = new List<PDQuest>();
              try
              {
                foreach (PDQuest q in _currentQuests.quests)
                {
                  if (_cacheQuests != null)
                  {
                    PDQuest qq = _cacheQuests.quests.Find(x => x.ID == q.ID && x.CurrentState == q.CurrentState);
                    if (qq == null)
                    {
                      //not found in list, so must be new or status changed
                      _changedQuests.Add(q);
                    } else
                    {
                      //found a match so remove 1 instance of quest
                      _cacheQuests.quests.RemoveAt(_cacheQuests.quests.LastIndexOf(qq));
                    }
                  }
                  else
                  {
                    _changedQuests.Add(q);
                  }
                }
                // update cached quests to current quests
                _cacheQuests.quests = _currentQuests.quests.Clone();
              }
              catch (Exception e)
              { Log.Out(Config.ModPrefix + " QuestMonitoring Fire Exception computing changed quests: " + e); }


              try
              {
                foreach (PDQuest q in _changedQuests)
                {
                  Log.Out(Config.ModPrefix + " " + playerName + " Quest Status Changed to " + q.CurrentState + ":" + q.ID + "");
                }
              }
              catch (Exception e)
              { Log.Out(Config.ModPrefix + " QuestMonitoring Fire Exception: (changedquests.foreach) " + e); }
            }
          }
        }
      }
      catch (Exception e)
      {
        Log.Out(Config.ModPrefix + " QuestMonitoring Fire Exception x: " + e);
      }

      return true;
    }
  }
}
