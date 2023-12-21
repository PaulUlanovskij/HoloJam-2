using Godot;
using System.Collections.Generic;

public partial class TargetSelectionPanel : Control
{
    [Export] TextureRect _marker;

    private List<CharacterPanel> _targets = new();
    private CombatPanel _battlePanel;

    private bool _isTargeting;
    private int _index = 0;

    public void StartTargeting(CharacterAction action, List<CharacterPanel> allies, List<CharacterPanel> foes, CombatPanel battlePanelForCallback)
    {
        _targets = action.TargetAllies is true ? allies : foes;
        _battlePanel = battlePanelForCallback;
        _marker.Show();
        _isTargeting = true;
        PositionOnTarget();
    }

    public override void _Process(double delta)
    {
        if (_isTargeting is false)
            return;
        PositionOnTarget();

        if (Input.IsActionJustPressed("ui_accept"))
        {
            int returnValue = _index;

            _index = 0;
            _isTargeting = false;
            _marker.Hide();

            _battlePanel.SetPlayerTarget(_targets[returnValue]);

        }
        else if (Input.IsActionJustPressed("ui_down") is true)
        {
            _index--;

            if (_index < 0)
                _index = _targets.Count + _index;

            _index %= _targets.Count;
            PositionOnTarget();

        }
        else if (Input.IsActionJustPressed("ui_up") is true)
        {
            _index++;

            _index %= _targets.Count;
            PositionOnTarget();
        }

    }

    private void PositionOnTarget()
    {
        var target = _targets[_index];

        _marker.GlobalPosition = target.GlobalPosition;
        _marker.Size = target.Size;
    }
}
