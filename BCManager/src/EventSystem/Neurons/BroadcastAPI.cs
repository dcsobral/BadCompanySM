using BCM.Models;
using BCM.PersistentData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BCM.Neurons
{
  public class BroadcastAPI : NeuronAbstract
  {
    class Listener
    {
      public string name;
      public string endpoint;
      public DateTime updated;
      public HttpWebRequest req;
      public Listener(string name, string endpoint)
      {
        this.name = name;
        this.endpoint = endpoint;
        this.updated = new DateTime();
        this.req = null;
      }
    }

    Dictionary<string, Listener> listeners = new Dictionary<string, Listener>();

    public BroadcastAPI()
    {
    }

    public bool addListener(string name, string endpoint)
    {
      if (!listeners.ContainsKey(name))
      {
        Listener l = new Listener(name, endpoint);
        listeners.Add(l.name, l);
        return true;
      }
      return false;
    }

    public bool removeListener(string name)
    {
      if (listeners.ContainsKey(name))
      {
        listeners.Remove(name);
        return true;
      }
      return false;
    }

    private Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
    {
      Dictionary<string, string> _options = new Dictionary<string, string>();
      _options.Add("json", null);


      Dictionary<string, string> data = new Dictionary<string, string>();

      Dictionary<string, string> info = new ClientInfoList(_pInfo, _options).GetInfo();
      Dictionary<string, string> stats = new StatsList(_pInfo, _options).GetStats();
      foreach (string key in info.Keys)
      {
        data.Add(key, info[key]);
      }
      foreach (string key in stats.Keys)
      {
        if (!data.ContainsKey(key))
        {
          data.Add(key, stats[key]);
        }
      }
      return data;
    }

    private static string build(Dictionary<string, string> _query)
    {
      string[] p = new string[_query.Count];
      int i = 0;
      foreach (KeyValuePair<string, string> kvp in _query)
      {
        p[i++] = kvp.Key + "=" + Uri.EscapeDataString(kvp.Value);
      }
      return string.Join("&", p);
    }

    public override bool Fire(int b)
    {
      //check for any registered listener servers before processing any data
      if (listeners.Count > 0 && ConnectionManager.Instance.ClientCount() > 0)
      {
        List<ClientInfo> clients = ConnectionManager.Instance.GetClients();


        Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>();

        foreach (ClientInfo _ci in clients)
        {
          var _id = _ci.steamId.ToString();
          data.Add(_id, jsonPlayer(new GetPlayer().BySteamId(_id)));
        }
        var output = BCUtils.toJson(data);

        foreach(Listener l in listeners.Values)
        {
          //compare listeners to connections (listLis.except(listCon))
          //if listener is not in connections then initiate api auth with endpoint
          //
          // send data and log ack in queue



          Dictionary<string, string> query = new Dictionary<string, string>();

          //todo
          query.Add("key", "val");


          byte[] postData = Encoding.ASCII.GetBytes(build(query));
          //todo: generate and store array of listeners in connections?
          if (l.req == null)
          {
            l.req = (HttpWebRequest)WebRequest.Create("http://" + l.endpoint + "/api/v1");

          }
          l.req.Method = "POST";
          l.req.ContentType = "application/x-www-form-urlencoded";
          l.req.ContentLength = postData.Length;
          l.req.Headers.Add(HttpRequestHeader.AcceptLanguage, "en");
          using (Stream st = l.req.GetRequestStream())
          {
            st.Write(postData, 0, postData.Length);
          }

          //todo: change to async

          HttpWebResponse response = (HttpWebResponse)l.req.GetResponse();
          string responseString = null;
          using (Stream st = response.GetResponseStream())
          {
            using (StreamReader str = new StreamReader(st))
            {
              responseString = str.ReadToEnd();
            }
          }

          if (responseString.ToLower().Contains("is_valid:true"))
          {
            l.updated = DateTime.Now;
            return true;
          }
          else
          {
            //todo: check time vs updated and delay next update if no reply, 
            //    use a ping to check for remote server on slower ticker until a sucessful response is received
            return false;
          }
        }



        //get id list for players online
        //get generic stat data
        //get specific detailed data if requested

        //format data into json strings

        //send data to remote server api's in the registered servers list
        //if ack fails for over 30 seconds then cancel watch and revert to pinging every x minutes for server returning to service
      }

      Log.Out(Config.ModPrefix + " BroadcastAPI");

      return true;
    }
  }
}
