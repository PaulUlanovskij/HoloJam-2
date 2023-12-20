using Godot;
using System;

[GlobalClass]
public partial class CharacterSheet : Resource
{
    [Export] public string Name { get; set; }
    [Export] public int MaxHP { get; set; }
    [Export] public int MaxSP { get; set; }
    [Export] public int Voice { get; set; }
    [Export] public float Def { get; set; }
    [Export] public float Res { get; set; }
    [Export] public int Initiative { get; set; }
    [Export] public Texture2D Portrait { get; set; }
    [Export] public PackedScene ActionDecider { get; set; }
    [Export] public CharacterAction[] Actions { get; set; }
}
