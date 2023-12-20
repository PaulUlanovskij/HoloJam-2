using Godot;
using System;

public partial class ActionDisplay : MarginContainer
{
    [Export] private Label _name;
    [Export] private Label _cost;
    [Export] private Color _spColor;
    [Export] private Color _hpColor;

    public void SetUp(CharacterAction characterAction)
    {
        _name.Text = characterAction.Name;
        
        _cost.Text = characterAction.Cost is 0 ? " " : characterAction.Cost.ToString();
        _cost.Modulate = characterAction.CostsHP ? _hpColor : _spColor;
    }
}
