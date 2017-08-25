namespace BCM.Commands
{
  public class BCTileEntity : BCCommandAbstract
  {
    public override void Process()
    {
      if (Params.Count == 0)
      {
        SendOutput(GetHelp());
        
        return;
      }

      //use loc and current location, or 2x vector3i to define area, or current loc and a radius
      //alternative option of x y z instead of 2xV3i for single block change
      //if x y z is not a TE then report nearest in y only + any direction as second item, report none if nothung within limit

      switch (Params[0])
      {
        case "owner":
          //grants the target ownership of secure tiles in the area
          break;
        case "access":
          //grants the target access to secure tiles in the area
          break;
        case "add":
          //adds items to a secure tile in the area
          break;
        case "empty":
          //empties secure tiles in the area of a type or all
          break;
        case "remove":
          //removes secure tiles in the area of a type or all
          break;
      }
    }
  }
}
