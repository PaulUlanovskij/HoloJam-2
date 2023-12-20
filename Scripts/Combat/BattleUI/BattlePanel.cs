using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BattlePanel : Control
{
    [Export] PackedScene _blankCharacterPanel;

    [Export] Control _foesPlacement;
    [Export] Control _alliesPlacement;

    [Export] ActionSelectionPanel _actionSelectionPanel;
    [Export] TargetSelectionPanel _targetSelectionPanel;

    private List<Character> _foes = new();
    private List<Character> _allies = new();

    private List<CharacterPanel> _foesPanels = new();
    private List<CharacterPanel> _alliesPanels = new();

    [Export] private TurnQueue _turnQueue;
    
    private bool _isPlayerTurn;
    private bool _isInBattle;
    
    private Character _activeCharacter;
    private CharacterAction _selectedAction;

    public Action<Character[]> BattleEnded;
    public Action BattleLost;

    public void InitiateBattle(Character[] foes, Character[] allies)
    {
        _turnQueue = GetChildren().Where(x => x is TurnQueue).FirstOrDefault() as TurnQueue;

        foreach (var child in _foesPlacement.GetChildren().Concat(_alliesPlacement.GetChildren()))
            child.QueueFree();
            
        _allies.Clear();
        _foes.Clear();

        foreach (var character in foes)
        {
            var characterPanel = CreateCharacterPanel(character);
            characterPanel.AddChild(character.ActionDecider.Instantiate());
            _foesPlacement.AddChild(characterPanel);

            _foes.Add(character);
            _foesPanels.Add(characterPanel);

            character.BecameOutOfAction += OnCharacterBecameOutOfAction;
        }
        foreach (var character in allies)
        {
            var characterPanel = CreateCharacterPanel(character);
            _alliesPlacement.AddChild(characterPanel);
         
            _allies.Add(character);
            _alliesPanels.Add(characterPanel);

            character.BecameOutOfAction += OnCharacterBecameOutOfAction;
        }

        _turnQueue.SetCharactersForQueue(_foes.Concat(_allies).ToList());

        _isInBattle = true;

        Show();
    }

    private void OnCharacterBecameOutOfAction(Character character)
    {
        bool isBattleOver = false;

        if (_foes.Where(x => x.IsOutOfAction is false).Any() is false)
        {
            isBattleOver = true;
            BattleEnded?.Invoke(_allies.ToArray());
        }
        else if (_allies.Where(x => x.IsOutOfAction is false).Any() is false)
        {
            isBattleOver = true;
            BattleLost?.Invoke();
        }

        if (isBattleOver is false)
            return;

        _isInBattle = false;

        foreach (var panel in _alliesPanels)
        {
            panel.RemoveAllStatusEffects();
            panel.QueueFree();
        }
        foreach (var panel in _foesPanels)
        {
            panel.RemoveAllStatusEffects();
            panel.QueueFree();
        }

        foreach (var foe in _foes)
            foe.BecameOutOfAction -= OnCharacterBecameOutOfAction;
        foreach (var ally in _allies)
            ally.BecameOutOfAction -= OnCharacterBecameOutOfAction;

        _foes.Clear();
        _allies.Clear();
        
        _foesPanels.Clear();
        _alliesPanels.Clear();

        Hide();
    }
    public override void _Process(double delta)
    {
        if (_isInBattle is false || _isPlayerTurn is true)
            return;

        Dequeue();
    }

    public void SetPlayerTarget(Character target) 
    {
        if (_selectedAction.TargetAllies is true)
        {
            _selectedAction.Play(_activeCharacter, new() { target }, _foes );
        }
        else
        {
            _selectedAction.Play(_activeCharacter, _allies, new() { target });
        }
        _activeCharacter.OnTurnEnd?.Invoke();

        _isPlayerTurn = false;
    }
    public void SetPlayerAction(CharacterAction action)
    {
        if (_isPlayerTurn is false)
            return;

        _selectedAction = action;

        if (_selectedAction.IsUntargetable is true)
        {
            _selectedAction.Play(_activeCharacter, _allies, _foes.Where(x => x.IsOutOfAction is false).ToList());
            _activeCharacter.OnTurnEnd?.Invoke();

            _isPlayerTurn = false;
            return;
        }
        _targetSelectionPanel.StartTargeting(_selectedAction, _alliesPlacement.GetChildren().Select(x => x as CharacterPanel).ToList(),
                                                              _foesPlacement.GetChildren().Select(x => x as CharacterPanel)
                                                              .Where(x => x.Character.IsOutOfAction is false).ToList(), this);
    }
    private void Dequeue()
    {
        _activeCharacter = _turnQueue.DequeueNext();
        if (_activeCharacter.IsOutOfAction is true)
            return;

        if (_allies.Contains(_activeCharacter) is true)
        {
            _isPlayerTurn = true;
            _activeCharacter.OnTurnStart?.Invoke();

            _actionSelectionPanel.StartSelection(_activeCharacter, this);
        }
        else if(_activeCharacter.IsOutOfAction is false)
        {
            _activeCharacter.OnTurnStart?.Invoke();
            _foesPanels.Find(x => x.Character == _activeCharacter).GetChildByType<CharacterActionDecider>().PickFromList(_activeCharacter, _foes, _allies.Where(x => x.IsOutOfAction is false).ToList());
            _activeCharacter.OnTurnEnd?.Invoke();
        }
    }

    private CharacterPanel CreateCharacterPanel(Character character)
    {
        var characterPanel = _blankCharacterPanel.Instantiate() as CharacterPanel;
        characterPanel.SetCharacter(character);

        return characterPanel;
    }
}
