using BCM.PersistentData;
using System;
using System.Collections.Generic;

namespace BCM.Neurons
{
  public class QuestMonitor : NeuronAbstract
  {
    private PersistentContainer _pcCache;
    public QuestMonitor(Synapse s) : base(s)
    {
      //Log.Out(Config.ModPrefix + " Added Quest Monitor");
    }
    public override void Fire(int b)
    {
      // todo: move this to the abstract object??
      if (_pcCache == null && PersistentContainer.IsLoaded)
      {
        try
        {
          _pcCache = PersistentContainer.Instance.Clone();
        }
        catch
        {
          Log.Out($"{Config.ModPrefix} Cache Clone Error");
        }
      }

      if (ConnectionManager.Instance.ClientCount() <= 0 || _pcCache == null) return;

      using (var enumerator = ConnectionManager.Instance.GetClients().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current == null) continue;

          var playerName = enumerator.Current.playerName;
          var playerId = enumerator.Current.playerId;

          //Quests _currentQuests = new Quests();
          //Quests _cacheQuests = new Quests();
          try
          {
            var pcurr = PersistentContainer.Instance.Players[playerId, false];
            var pcache = _pcCache.Players[playerId, true]; // allow player to be created if it doesn't exist in cache
            if (pcurr != null)
            {
              //_currentQuests = pcurr.Quests;
            }
            if (pcache != null)
            {
              //_cacheQuests = pcache.Quests;
            }
            //else
            //{
            //  // test: need to create player in cache if they have just joined for the first time after deleting the data file
            //  try
            //  {
            //    _pcCache.Players[playerId, true].Quests.quests = pcurr.Quests.quests.Clone();
            //    pcache = 
            //  } catch (Exception e)
            //  {
            //    Log.Out(Config.ModPrefix + " QuestMonitoring.Fire Exception couldn't update new player quests: " + e);
            //  }
            //}
          }
          catch (Exception e)
          { Log.Out($"{Config.ModPrefix} QuestMonitoring.Fire Exception getting quest lists: {e}"); }

          var changedQuests = new List<Quest>();
          try
          {
            //foreach (Quest q in _currentQuests.quests)
            //{
            //  if (_cacheQuests != null)
            //  {
            //    Quest qq = _cacheQuests.quests.Find(x => x.Id == q.Id && x.CurrentState == q.CurrentState);
            //    if (qq == null)
            //    {
            //      //not found in list, so must be new or status changed
            //      _changedQuests.Add(q);
            //    }
            //    else
            //    {
            //      //found a match so remove 1 instance of quest
            //      _cacheQuests.quests.RemoveAt(_cacheQuests.quests.LastIndexOf(qq));
            //    }
            //  }
            //  else
            //  {
            //    _changedQuests.Add(q);
            //  }
            //}
            // update cached quests to current quests
            try
            {
              //_cacheQuests.quests = _currentQuests.quests.Clone();
            }
            catch (Exception e)
            {
              Log.Out($"{Config.ModPrefix} QuestMonitoring.Fire Exception assigning current quests to cache: {e}");
            }
          }
          catch (Exception e)
          {
            Log.Out($"{Config.ModPrefix} QuestMonitoring.Fire Exception computing changed quests: {e}");
            changedQuests = new List<Quest>(); //if an error computing changes then don't return a list or it spams log files
          }

          try
          {
            foreach (var q in changedQuests)
            {
              Log.Out($"{Config.ModPrefix} {playerName} Quest Status Changed to {q.CurrentState}:{q.ID}");
            }
          }
          catch (Exception e)
          { Log.Out($"{Config.ModPrefix} QuestMonitoring.Fire Exception: (changedquests.foreach) {e}"); }
        }
      }
    }
  }
}
