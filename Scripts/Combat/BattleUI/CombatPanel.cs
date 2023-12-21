using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CombatPanel : Control
{
    [Export] PackedScene _blankCharacterPanel;

    [Export] Control _foesPlacement;
    [Export] Control _alliesPlacement;

    [Export] private TurnQueue _turnQueue;

    [Export] ActionSelectionPanel _actionSelectionPanel;
    [Export] TargetSelectionPanel _targetSelectionPanel;

    public Action<Character[]> CombatEnded;
    public Action CombatLost;



    private Character _activeCharacter;
    private CharacterAction _selectedAction;


    private List<CharacterPanel> _foes = new();
    private List<CharacterPanel> _allies = new();

    private List<CharacterPanel> _livingAllies => _allies.Where(x => x.Character.IsDead is false).ToList();
    private List<CharacterPanel> _livingFoes => _foes.Where(x => x.Character.IsDead is false).ToList();
    private CharacterPanel _activeCharacterPanel => _allies.Concat(_foes).Where(x => x.Character == _activeCharacter).FirstOrDefault();


    public void StartCombat(Character[] allies, Character[] foes)
    {
        foreach (var ally in allies)
        {
            var panel = CreateCharacterPanel(ally);
            _alliesPlacement.AddChild(panel);
            _allies.Add(panel);
            
        }
        foreach (var foe in foes)
        {
            var panel = CreateCharacterPanel(foe);
            _foesPlacement.AddChild(panel);
            _foes.Add(panel);

            panel.AddChild(foe.ActionDecider.Instantiate());
            panel.AnimatedSprite.FlipH = true;
        }

        _turnQueue.SetCharactersForQueue(allies.Concat(foes).ToList());

        NextTurn();
    }

    public void SetPlayerAction(CharacterAction action)
    {
        _selectedAction = action;

        if (_selectedAction.IsUntargetable is true)
        {
            PlayAction(action, _activeCharacterPanel, _allies, _livingFoes);
            return;
        }
        _targetSelectionPanel.StartTargeting(_selectedAction, _allies, _livingFoes, this);
    }
    public void SetPlayerTarget(CharacterPanel target)
    {
        if (_selectedAction.TargetAllies is true)
        {
            PlayAction(_selectedAction, _activeCharacterPanel, new() { target }, _foes);
        }
        else
        {
            PlayAction(_selectedAction, _activeCharacterPanel, _allies, new() { target });
        }
    }

    private void NextTurn()
    {
        _activeCharacter = _turnQueue.DequeueNext();

        _activeCharacterPanel.OnStartOfTurn?.Invoke();

        if (_allies.Contains(_activeCharacterPanel))
        {
            _actionSelectionPanel.StartSelection(_activeCharacter, this);
        }
        else
        {
            var action = _activeCharacterPanel.ActionDecider.PickFromList(_activeCharacterPanel, _foes, _allies);
            PlayAction(action, _activeCharacterPanel, _foes, _allies);
        }
    }

    private void OnCharacterAnimationFinished(Character character)
    {
        if (character == _activeCharacter)
            NextTurn();
    }
    private void OnCharacterDeath(Character character)
    {
        bool hasCombatEnded = !_livingAllies.Any() || !_livingFoes.Any();

        if (hasCombatEnded is true)
        {
            if (_livingAllies.Any() is false)
            {
                CombatLost?.Invoke();
            }
            else 
            {
                CombatEnded?.Invoke(_allies.Select(x => x.Character).ToArray());
            }

            foreach (var ally in _allies)
            {
                ally.QueueFree();
            }
            foreach (var foe in _foes)
            {
                foe.QueueFree();
            }
        }
        else
        {
            _turnQueue.RemoveCharacter(character);
        }
    }
    
    private CharacterPanel CreateCharacterPanel(Character character)
    {
        var panel = _blankCharacterPanel.Instantiate() as CharacterPanel;
        panel.SetCharacter(character);

        character.OnDeath += OnCharacterDeath;

        return panel;
    }
    private void PlayAction(CharacterAction action, CharacterPanel caster, List<CharacterPanel> allies, List<CharacterPanel> foes)
    {
        action.Play(caster, allies, foes);
        caster.PlayAnimation(action.AnimationType);
        caster.OnEndOfTurn?.Invoke();

        if (action.AnimationType is not AnimationType.block && action.AnimationType is not AnimationType.idle)
        {
            WaitTillAnimStop(_activeCharacterPanel);
        }
        else
        {
            NextTurn();
        }
    }

    private async void WaitTillAnimStop(CharacterPanel characterPanel)
    {
        await ToSignal(characterPanel.AnimatedSprite, "animation_finished");
        OnCharacterAnimationFinished(characterPanel.Character);
    }

}