using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class CharacterActionDecider : Node
{
    public virtual void PickFromList(Character caster, List<Character> allies, List<Character> foes)
    {
        if (caster.Actions is not null && caster.Actions.Any())
        {
            caster.Actions[0].Play(caster, allies, foes);
            return;
        }

        new CharacterAction().Play(caster, allies, foes);
    }
}
