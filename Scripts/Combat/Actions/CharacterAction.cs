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
    [Export] public AnimationType AnimationType; 

    public void Play(CharacterPanel caster, List<CharacterPanel> allies = null, List<CharacterPanel> foes = null)
    {
        if (CostsHP is true) 
        {
            caster.Character.HP.Value -= Cost;
        }
        else
        {
            caster.Character.SP.Value -= Cost;
        }
        PlayActionOverride(caster, allies, foes);
    }
    protected virtual void PlayActionOverride(CharacterPanel caster, List<CharacterPanel> allies, List<CharacterPanel> foes)
    {
        if (_alliesStatusEffects is not null && _alliesStatusEffects.Any() is true)
        {
            foreach (var panel in allies)
            {
                panel.AddStatusEffect(_alliesStatusEffects[0]);
            }
        }
        if (_foesStatusEffects is not null && _foesStatusEffects.Any() is true)
        {
            foreach (var panel in foes)
            {
                panel.AddStatusEffect(_alliesStatusEffects[0]);
            }
        }
    }
}
