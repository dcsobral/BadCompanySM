using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCM.ConfigModels
{
  public class Command
  {
    public string name;
    public string[] commands;
    public int dpl;
    public string help;
    public string description;

    public Command()
    {
    }
    public Command(string _name, string[] _commands, int _dpl, string _help, string _description)
    {
      name = _name;
      commands = _commands;
      dpl = _dpl;
      help = _help;
      description = _description;
    }
  }
}
