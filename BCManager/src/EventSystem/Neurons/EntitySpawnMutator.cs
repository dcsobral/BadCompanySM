using System;

namespace BCM.Neurons
{
  public class EntitySpawnMutator : NeuronAbstract
  {
    private bool hasDelegate = false;

    public EntitySpawnMutator()
    {
    }
    public override bool Fire(int b)
    {
      if (!hasDelegate && API.IsAlive)
      {
        if (GameManager.Instance.World != null)
        {
          GameManager.Instance.World.EntityLoadedDelegates += new World.OnEntityLoadedDelegate(OnEntityLoaded);
          Log.Out("EntitySpawnMutator Initialised");

          hasDelegate = true;
        }
      }

      return true;
    }

    private void OnEntityLoaded(Entity _entity)
    {
      if (_entity is EntityFallingBlock || _entity is EntityItem || _entity is EntityLootContainer)
      {
        return;
      }

      if (_entity is EntityEnemy || _entity is EntityNPC)
      {
        string className = "";
        EntityClass ec = null;

        if (EntityClass.list.ContainsKey(_entity.entityClass))
        {
          ec = EntityClass.list[_entity.entityClass];
        }

        if (ec != null)
        {
          className = ec.entityClassName;
        }

        Log.Out(Config.ModPrefix + " Spawn Detected:" + className + "[" + _entity.entityId.ToString() + "](" + _entity.GetType().ToString() + ")@" + ((int)_entity.position.x).ToString() + " " + ((int)_entity.position.y).ToString() + " " + ((int)_entity.position.z).ToString());
        //todo: apply settings to entity based on config
      }

    }
  }
}
