using Godot;
using System;

public partial class CharacterPanel : Control
{
    [Export] public AnimatedSprite2D AnimatedSprite;

    [Export] private ProgressBar _HPBar;
    [Export] private ProgressBar _SPBar;

    [Export] private Node _statusEffectHolder;

    public Action OnStartOfTurn;
    public Action OnEndOfTurn;

    public Character Character;
    public CharacterActionDecider ActionDecider => this.GetChildByType<CharacterActionDecider>();
    
    public void SetCharacter(Character character)
    {
        if (character is null)
            return;
        
        Character = character;

        AnimatedSprite.Stop();
        AnimatedSprite.SpriteFrames = Character.SpriteFrames;

        _HPBar.MaxValue = Character.MaxHP.Value;
        _HPBar.Value = Character.HP.Value;

        _SPBar.MaxValue = Character.MaxSP.Value;
        _SPBar.Value = Character.SP.Value;


        Character.HP.OnChangedValue += delegate { _HPBar.Value = Character.HP.Value; };
        Character.MaxHP.OnChangedValue += delegate { _HPBar.MaxValue = Character.MaxHP.Value; };

        Character.SP.OnChangedValue += delegate { _SPBar.Value = Character.SP.Value; };
        Character.MaxSP.OnChangedValue += delegate { _SPBar.MaxValue = Character.MaxSP.Value; };


        Character.OnDeath += OnDeath;
    }
    public void PlayAnimation(AnimationType type)
    {
        GD.Print(type);
        AnimatedSprite.Play(type.ToString());
    }
    public override void _Process(double delta)
    {
        if (AnimatedSprite.IsPlaying() is false && AnimatedSprite.SpriteFrames is not null)
            PlayAnimation(AnimationType.idle);
    }
    public void RemoveAllStatusEffects()
    {
        foreach (var item in _statusEffectHolder.GetChildren())
        {
            item.QueueFree();
        }
    }
    public void AddStatusEffect(PackedScene scene)
    {
        var status = scene.Instantiate() as StatusEffect;
        _statusEffectHolder.AddChild(status);
        status.Apply();
    }

    public void OnDeath(Character character)
    {
        AnimatedSprite.Play("death");
        RemoveAllStatusEffects();
    }
    protected override void Dispose(bool disposing)
    {
        if (Character is not null)
        {
            Character.HP.OnChangedValue += delegate { _HPBar.Value = Character.HP.Value; };
            Character.MaxHP.OnChangedValue += delegate { _HPBar.MaxValue = Character.MaxHP.Value; };

            Character.SP.OnChangedValue += delegate { _SPBar.Value = Character.SP.Value; };
            Character.MaxSP.OnChangedValue += delegate { _SPBar.MaxValue = Character.MaxSP.Value; };


            Character.OnDeath += OnDeath;
        }

        base.Dispose(disposing);
    }
}
