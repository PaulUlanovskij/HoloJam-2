using Godot;

public partial class BlockStatusEffect : StatusEffect
{
    [Export] public float DefencePercent = 0.3f;
    protected override void OnApply()
    {
        _character = this.GetParentByType<CharacterPanel>().Character;
        _character.Def += DefencePercent;
    }
    protected override void OnTurnBeginns()
    {
        _character.Def -= DefencePercent;
    }
}
