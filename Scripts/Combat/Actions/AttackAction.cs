using Godot;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class AttackAction : CharacterAction
{
    [Export] public int Damage = 2;
    [Export] public DamageType DamageType;
    protected override void PlayActionOverride(Character caster, List<Character> allies, List<Character> foes)
    {
        foes.First().TakeDamage(Damage, DamageType);

        GD.Print($"{caster.Name} attacked {foes.First().Name}.\n{Description}.\n");
    }
}
