using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class TargetSelectionPanel : Control
{
    [Export] NinePatchRect _marker;

    private List<CharacterPanel> _targets = new();
    private BattlePanel _battlePanel;

    private bool _isTargeting;
    private int _index = 0;

    public void StartTargeting(CharacterAction action, List<CharacterPanel> allies, List<CharacterPanel> foes, BattlePanel battlePanelForCallback)
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
            _battlePanel.SetPlayerTarget(_targets[_index].Character);

            _marker.Hide();
            _index = 0;
            _isTargeting = false;
        }
        else if (Input.IsActionJustPressed("ui_left") is true)
        {
            _index--;

            if (_index < 0)
                _index = _targets.Count + _index;

            _index %= _targets.Count;
            PositionOnTarget();

        }
        else if (Input.IsActionJustPressed("ui_right") is true)
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
