using System;

namespace BCM.PersistentData
{
  [Serializable]
  public class Attributes
  {
    private bool hideChatCommands;
    private String hideChatCommandPrefix;

    public bool HideChatCommands
    {
      get
      {
        return hideChatCommands;
      }
      set
      {
        hideChatCommands = value;
      }
    }

    public string HideChatCommandPrefix
    {
      get
      {
        if (hideChatCommandPrefix == null)
        {
          hideChatCommandPrefix = "";
        }
        return hideChatCommandPrefix;
      }
      set
      {
        hideChatCommandPrefix = value;
      }
    }

  }
}
