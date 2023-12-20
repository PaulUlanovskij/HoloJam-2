using Godot;

public partial class StatusEffect : Node
{
    protected Character _character;
    public void Apply()
    {
        _character = this.GetParentByType<CharacterPanel>().Character;
        OnApply();
        _character.OnTurnStart += OnTurnBeginns;
        _character.OnTurnEnd += OnTurnEnds;

    }
    protected virtual void OnApply()
    {

    }
    protected virtual void OnTurnBeginns()
    {

    }
    
    protected virtual void OnTurnEnds() 
    {

    }
    protected virtual void OnDispel()
    {

    }
    protected override void Dispose(bool disposing)
    {
        OnDispel();
        base.Dispose(disposing);
    }
}
