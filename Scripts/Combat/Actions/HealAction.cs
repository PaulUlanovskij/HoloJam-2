using Godot;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class HealAction : CharacterAction
{
    [Export] public int Heal = 2;

    public override bool TargetAllies => true;
    protected override void PlayActionOverride(Character caster, List<Character> allies, List<Character> foes)
    {
        allies.First().Heal(Heal);

        GD.Print($"{caster.Name} healed {allies.First().Name}.\n{Description}.\n");
    }
}
