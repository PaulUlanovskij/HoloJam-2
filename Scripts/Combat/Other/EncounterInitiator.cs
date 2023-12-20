using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EncounterInitiator : Node
{
    [Export] CharacterSheet[] _allies;
    [Export] CharacterSheet[] _foes;

    [Export] BattlePanel _battle;
    public override void _Ready()
    {
        _battle.InitiateBattle(_foes.Select(x => new Character(x)).ToArray(), _allies.Select(x => new Character(x)).ToArray()) ;
        _battle.BattleEnded += Success;
        _battle.BattleLost += Failure;
    }

    private void Failure()
    {
        GD.Print("Battle lost");
    }

    private void Success(Character[] obj)
    {
        GD.Print("Battle won");
    }
}
