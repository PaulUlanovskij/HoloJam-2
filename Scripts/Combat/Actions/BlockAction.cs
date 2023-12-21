using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class BlockAction : CharacterAction
{
    public override bool IsUntargetable => true;
    protected override void PlayActionOverride(CharacterPanel caster, List<CharacterPanel> allies, List<CharacterPanel> enemies)
    {
        caster.AddStatusEffect(_alliesStatusEffects[0]);

    }
}
