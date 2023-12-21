using Godot;

public partial class BlockStatusEffect : StatusEffect
{
    [Export] public float DefencePercent = 0.3f;
    protected override void OnApply()
    {
        _characterPanel.Character.Def += DefencePercent;
    }
    protected override void OnTurnBeginns()
    {
        _characterPanel.Character.Def -= DefencePercent;
        this.GetParentByType<CharacterPanel>().PlayAnimation(AnimationType.idle);
        Dispose();
    }
}
