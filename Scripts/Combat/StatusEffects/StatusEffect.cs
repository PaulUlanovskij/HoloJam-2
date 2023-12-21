using Godot;

public partial class StatusEffect : Node
{
    protected CharacterPanel _characterPanel;
    public void Apply()
    {
        _characterPanel = this.GetParentByType<CharacterPanel>();
        OnApply();
        _characterPanel.OnStartOfTurn += OnTurnBeginns;
        _characterPanel.OnEndOfTurn += OnTurnEnds;

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
        _characterPanel.OnStartOfTurn -= OnTurnBeginns;
        _characterPanel.OnEndOfTurn -= OnTurnEnds;
        OnDispel();
        base.Dispose(disposing);
    }
}
