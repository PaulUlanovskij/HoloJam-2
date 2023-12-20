using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class BlockAction : CharacterAction
{
    public override bool IsUntargetable => true;
    protected override void PlayActionOverride(Character caster, List<Character> allies, List<Character> enemies)
    {
        GD.Print($"{caster.Name} blocks \n{Description}.\n");
    }
    protected override void AddStatusEffectsOverride(Character caster, List<Character> affectedAllies, List<Character> affectedFoes)
    {
        caster.AddStatusEffect(_alliesStatusEffects[0]);
    }
}
