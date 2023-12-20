using Godot;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class CharacterAction : Resource
{
    [Export] private int HPCost;
    [Export] private int SPCost;

    [Export] public string Name;
    [Export] public string Description;

    [Export] protected PackedScene[] _foesStatusEffects;
    [Export] protected PackedScene[] _alliesStatusEffects;

    public int Cost => SPCost > 0 ? SPCost : HPCost;
    public bool CostsHP => SPCost > 0 ? false : true;

    public virtual bool IsUntargetable { get; } = false;
    public virtual bool TargetAllies { get; } = false;

    public void Play(Character caster, List<Character> allies = null, List<Character> foes = null)
    {
        if (CostsHP is true) 
        {
            caster.HP.Value -= Cost;
        }
        else
        {
            caster.SP.Value -= Cost;
        }
        PlayActionOverride(caster, allies, foes);
        AddStatusEffectsOverride(caster, allies, foes);
    }
    protected virtual void PlayActionOverride(Character caster, List<Character> allies, List<Character> foes)
    {
        GD.Print($"{caster.Name} played {Name}.\n{Description}.\n");
    }
    protected virtual void AddStatusEffectsOverride(Character caster, List<Character> affectedAllies, List<Character> affectedFoes)
    {
        if (_alliesStatusEffects is not null && _alliesStatusEffects.Any() is true)
        {
            foreach (Character character in affectedAllies)
            {
                character.AddStatusEffect(_alliesStatusEffects[0]);
            }
        }
        if (_foesStatusEffects is not null && _foesStatusEffects.Any() is true)
        {
            foreach (Character character in affectedFoes)
            {
                character.AddStatusEffect(_foesStatusEffects[0]);
            }

        }
    }
}
