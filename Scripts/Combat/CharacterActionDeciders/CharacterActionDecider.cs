using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class CharacterActionDecider : Node
{
    public virtual CharacterAction PickFromList(CharacterPanel caster, List<CharacterPanel> allies, List<CharacterPanel> foes)
    {
        if (caster.Character.Actions is not null && caster.Character.Actions.Any())
        {
            return caster.Character.Actions[0];
        }

        return new CharacterAction();
    }
}
