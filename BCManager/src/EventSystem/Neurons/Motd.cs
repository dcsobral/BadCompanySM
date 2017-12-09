namespace BCM.Neurons
{
  public class Motd : NeuronAbstract
  {
    public Motd(Synapse s) : base(s)
    {
    }
    public override void Fire(int b)
    {
      //preiodic messages sent to chat from server



      Log.Out(Config.ModPrefix + " Motd");
    }
  }
}
