using System;

namespace BCM.Commands
{
  public class BCExport : BCCommandAbstract
  {
    public override void Process()
    {
      var world = GameManager.Instance.World;
      if (world == null) return;

      var p1 = new Vector3i(int.MinValue, 0, int.MinValue);
      var p2 = new Vector3i(int.MinValue, 0, int.MinValue);
      string filename = null;

      switch (Params.Count)
      {
        case 1:
          //get loc and player current pos
          if (SenderInfo.RemoteClientInfo != null)
          {
            if (!world.Entities.dict.ContainsKey(SenderInfo.RemoteClientInfo.entityId)) return;

            var sender = world.Entities.dict[SenderInfo.RemoteClientInfo.entityId] as EntityPlayer;
            Vector3i currentPos;
            if (sender != null)
            {
              currentPos = new Vector3i((int) Math.Floor(sender.serverPos.x / 32f), (int) Math.Floor(sender.serverPos.y / 32f), (int) Math.Floor(sender.serverPos.z / 32f));
            }
            else
            {
              SendOutput("Error: unable to get player location");

              return;
            }

            var steamId = SenderInfo.RemoteClientInfo.ownerId;
            if (steamId != null)
            {
              p1 = BCLocation.GetPos(steamId);
              if (p1.x == int.MinValue)
              {
                SendOutput("No location stored. Use bc-loc to store a location.");

                return;
              }
              p2 = currentPos;

              filename = Params[0];
            }
          }
          else
          {
            SendOutput("Error: unable to get player location");

            return;
          }



          break;

        case 7:
          //parse params
          if (!int.TryParse(Params[0], out p1.x) || !int.TryParse(Params[1], out p1.y) || !int.TryParse(Params[2], out p1.z) || !int.TryParse(Params[3], out p2.x) || !int.TryParse(Params[4], out p2.y) || !int.TryParse(Params[5], out p2.z))
          {
            SendOutput("Error: unable to parse coordinates");

            return;
          }
          filename = Params[6];

          break;

        default:
          SendOutput("Error: Incorrect command format.");
          SendOutput(GetHelp());

          return;
      }

      if (filename == null)
      {
        SendOutput("Error: Prefab filename was null.");
      }
      else
      {
        var prefab = new Prefab();
        var p0 = prefab.CopyFromWorld(world, p1, p2);
        //_prefab.CopyFromWorldWithEntities();

        prefab.filename = filename;
        prefab.bCopyAirBlocks = true;
        prefab.addAllChildBlocks(); //credit: DJKRose
        //todo: lock doors etc?

        //todo: parse additional config from options

        var dir = "Data/Prefabs";
        if (Options.ContainsKey("backup"))
        {
          dir = "Data/Prefabs/Backup";
        }
        SendOutput(prefab.Save(dir, prefab.filename)
          ? $"Prefab {prefab.filename} exported @ {p0.x} {p0.y} {p0.z}, size={prefab.size.x} {prefab.size.y} {prefab.size.z}"
          : $"Error: Prefab {prefab.filename} failed to save.");
      }
    }
  }
}
