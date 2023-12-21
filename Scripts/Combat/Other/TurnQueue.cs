using Godot;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class TurnQueue : Node
{
    private Queue<Character> _characters = new();
    public void SetCharactersForQueue(List<Character> characters)
    {
        _characters = new (characters.OrderByDescending(x => x.Initiative));
    }
    public Character DequeueNext()
    {
        if ( _characters.Count is 0) return null;

        var character = _characters.Dequeue();
        _characters.Enqueue(character);

        return character;
    }
    public void RemoveCharacter(Character character)
    {
        _characters = new (_characters.Where(x => x != character));
    }
}