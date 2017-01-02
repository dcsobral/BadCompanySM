namespace BCM.Neurons
{
  public class QuestMonitor : NeuronAbstract
  {
    public override bool Fire(int b)
    {
      //Log.Out(Config.ModPrefix + " QuestMonitoring Fired");
      return true;
    }
  }
}
