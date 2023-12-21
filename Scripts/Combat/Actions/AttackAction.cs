using Godot;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class AttackAction : CharacterAction
{
    [Export] public int Damage = 2;
    [Export] public DamageType DamageType;
    protected override void PlayActionOverride(CharacterPanel caster, List<CharacterPanel> allies, List<CharacterPanel> foes)
    {
        foes.First().Character.TakeDamage(Damage, DamageType);

    }
}
