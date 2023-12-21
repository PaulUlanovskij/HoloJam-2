using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;

public partial class Character : IEquatable<Character>
{
    public Character(CharacterSheet characterSheet)
    {
        Name = characterSheet.Name;

        Portrait = characterSheet.Portrait;

        Initiative = characterSheet.Initiative;

        
        ActionDecider = characterSheet.ActionDecider;
        Actions = characterSheet.Actions;
        SpriteFrames = characterSheet.SpriteFrames;

        HP.Value = MaxHP.Value = characterSheet.MaxHP;
        SP.Value = MaxSP.Value = characterSheet.MaxSP;

        HP.OnChangedValue += CheckHPStatus;
    }

    public string Name;

    public SignalingVariable<int> HP { get; } = new();
    public SignalingVariable<int> MaxHP { get; } = new();

    public SignalingVariable<int> SP { get; } = new();
    public SignalingVariable<int> MaxSP { get; } = new();

    public int Voice;
    public float Def;
    public float Res;

    public int Initiative;

    public Texture2D Portrait;

    public PackedScene ActionDecider;
    public CharacterAction[] Actions;

    public SpriteFrames SpriteFrames;

    public Action<Character> OnDeath;


    public bool IsDead;
    
    public void TakeDamage(int amount, DamageType damageType)
    {
        if(amount <= 0) 
        {
            return;
        }
        var modifier = (1 - (damageType == DamageType.Physical ? Def : Res));
        var actualDamage = amount * modifier;

        HP.Value -= (int)Math.Max(0, actualDamage);

    }
    public void Heal(int amount)
    {
        if (amount <= 0)
        {
            return;
        }
        HP.Value += amount;
    }
    private void CheckHPStatus(int newValue, int oldValue)
    {
        if (HP.Value <= 0)
        {
            IsDead = true;
            OnDeath?.Invoke(this);
        }
    }
    public bool CanPerformAction(CharacterAction action)
    {
        if (action.CostsHP is true)
        {
            return HP.Value > action.Cost;
        }
        else
        {
            return SP.Value >= action.Cost;
        }
    }
    public override bool Equals(object obj)
    {
        return obj is Character character &&
               Name == character.Name &&
               HP == character.HP &&
               MaxHP == character.MaxHP &&
               SP == character.SP &&
               MaxSP == character.MaxSP &&
               Voice == character.Voice &&
               Def == character.Def &&
               Res == character.Res &&
               Initiative == character.Initiative &&
               EqualityComparer<Texture2D>.Default.Equals(Portrait, character.Portrait) &&
               EqualityComparer<PackedScene>.Default.Equals(ActionDecider, character.ActionDecider) &&
               EqualityComparer<CharacterAction[]>.Default.Equals(Actions, character.Actions) &&
               EqualityComparer<Action<Character>>.Default.Equals(OnDeath, character.OnDeath) &&
               IsDead == character.IsDead;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Name);
        hash.Add(HP);
        hash.Add(MaxHP);
        hash.Add(SP);
        hash.Add(MaxSP);
        hash.Add(Voice);
        hash.Add(Def);
        hash.Add(Res);
        hash.Add(Initiative);
        hash.Add(Portrait);
        hash.Add(ActionDecider);
        hash.Add(Actions);
        hash.Add(OnDeath);
        hash.Add(IsDead);
        return hash.ToHashCode();
    }

    public bool Equals(Character other)
    {
        return Equals((object)other);
    }
}
