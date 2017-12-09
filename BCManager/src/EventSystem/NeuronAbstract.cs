namespace BCM
{
  public class NeuronAbstract
  {
    public Synapse synapse;

    public NeuronAbstract(Synapse s)
    {
      synapse = s;
    }

    public virtual void Fire(int b)
    {
    }

    public virtual void Awake()
    {
    }
  }
}
