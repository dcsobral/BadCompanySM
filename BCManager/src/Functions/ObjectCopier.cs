using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BCM
{
  public static class ObjectCopier
  {
    public static T Clone<T>(this T source)
    {
      if (!typeof(T).IsSerializable)
      {
        Log.Out($"{Config.ModPrefix} Clone object requires a serializable object");
      }

      // Don't serialize a null object, simply return the default for that object
      if (ReferenceEquals(source, null))
      {
        return default(T);
      }

      IFormatter formatter = new BinaryFormatter();
      Stream stream = new MemoryStream();
      using (stream)
      {
        formatter.Serialize(stream, source);
        stream.Seek(0, SeekOrigin.Begin);
        return (T)formatter.Deserialize(stream);
      }
    }
  }
}
