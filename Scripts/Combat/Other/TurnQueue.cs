using Godot;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class TurnQueue : Node
{
    private Queue<Character> _characters = new();
    private List<Character> _playedCharacters = new();
    public void SetCharactersForQueue(List<Character> characters)
    {
        _characters = new (characters.OrderByDescending(x => x.Initiative));
    }
    public Character DequeueNext()
    {
        if ( _characters.Count is 0) return null;

        var character = _characters.Dequeue();
        _playedCharacters.Add(character);

        if (_characters.Count is 0)
        {
            _characters = new(_playedCharacters);
        }

        return character;
    }
    public void RemoveCharacter(Character character)
    {
        _characters = new (_characters.Where(x => x != character));
        _playedCharacters = new (_playedCharacters.Where(x => x != character));
    }
    public void AddCharacter(Character character)
    {
        _playedCharacters.Add(character);
    }
}