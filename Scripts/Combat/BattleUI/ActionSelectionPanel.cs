using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class ActionSelectionPanel : PanelContainer
{
    [Export] private PackedScene _actionPanel;
    [Export] private VBoxContainer _actionsBox;
    [Export] private Label _actionDescription;


    private List<CharacterAction> _actions = new List<CharacterAction>();
    private List<ActionDisplay> _actionPanels = new List<ActionDisplay>();
    private CombatPanel _battlePanel;

    [Export] Control _pickerMarker;

    [Export] private Color _unavailible;
    private int _actionIndex = 0;

    public void StartSelection(Character character, CombatPanel battlePanelForCallback)
    {
        foreach (var child in _actionsBox.GetChildren())
        {
            child.QueueFree();
        }
        foreach (var action in character.Actions) 
        {
            ActionDisplay actionPanel = _actionPanel.Instantiate() as ActionDisplay;
            _actionsBox.AddChild(actionPanel);

            actionPanel.SetUp(action);
            if (character.CanPerformAction(action) is true)
            {
                _actionPanels.Add(actionPanel);
                _actions.Add(action);
            }
            else
            {
                actionPanel.Modulate = _unavailible;
            }
        }

        Show();

        _battlePanel = battlePanelForCallback;

        PositionOnTarget();
    }
    public override void _Process(double delta)
    {
        if (_actions.Any() is false)
            return;
        PositionOnTarget();

        if (Input.IsActionJustPressed("ui_accept"))
        {
            _battlePanel.SetPlayerAction(_actions[_actionIndex]);

            _actionIndex = 0;

            _actions.Clear();
            _actionPanels.Clear();
            Hide();
        }
        else if (Input.IsActionJustPressed("ui_up") is true)
        {
            _actionIndex--;

            if (_actionIndex < 0)
                _actionIndex = _actions.Count - 1;

            _actionIndex %= _actions.Count;
            PositionOnTarget();
        }
        else if (Input.IsActionJustPressed("ui_down") is true)
        {
            _actionIndex++;

            _actionIndex %= _actions.Count;
            PositionOnTarget();
        }
    }
    private void PositionOnTarget()
    {
        _actionDescription.Text = _actions[_actionIndex].Description;

        _pickerMarker.GlobalPosition = _actionPanels[_actionIndex].GlobalPosition;
        _pickerMarker.Size = _actionPanels[_actionIndex].Size;
    }
}
