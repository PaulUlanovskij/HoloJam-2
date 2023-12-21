using Godot;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class HealAction : CharacterAction
{
    [Export] public int Heal = 2;

    public override bool TargetAllies => true;
    protected override void PlayActionOverride(CharacterPanel caster, List<CharacterPanel> allies, List<CharacterPanel> foes)
    {
        allies.First().Character.Heal(Heal);

    }
}
