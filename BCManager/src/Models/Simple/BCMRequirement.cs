using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMRequirement
  {
    [UsedImplicitly] public int Level;
    [UsedImplicitly] public string SkillRequired;
    [UsedImplicitly] public int SkillLevelRequired;

    public BCMRequirement(Skill.Requirement requirement)
    {
      Level = requirement.Level;
      SkillRequired = requirement.SkillRequired;
      SkillLevelRequired = requirement.SkillLevelRequired;
    }
  }
}
