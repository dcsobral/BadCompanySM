namespace BCM.Neurons
{
  public class DeadIsDead : NeuronAbstract
  {
    public DeadIsDead()
    {
    }
    public override void Fire(int b)
    {
      // todo: implement
      // todo: IsDead checker, after death animation kick player and then rename ttp file and move to sub folder for archive


      //two modes, 'global_DiD',  'individual_DiD'
      //global_DiD:
      //monitor player deaths, any time a death count increases by 1+ active DiD process

      //individual_DiD:
      // same as global, but only process DiD if player has opted in.
      // when player opts in the death count is recorded and uses as the baseline to check for additional deaths

      //DiD process:
      //On player death
      // create copy of player .ttp and .map files in a subfolder (archives/steamid/timestamp.ttp etc)
      // kick player with DiD message
      // delete current player files
      // reset any claim blocks and door/chest ownerships?
      // allow player to rejoin with new character
      // log the DiD process and reset player stats in web app

      //admin option to restore player files


      Log.Out(Config.ModPrefix + " DeadIsDead");
    }
  }
}

