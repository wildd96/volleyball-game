using Godot;
using System;

public partial class Main : Node
{
	private CharacterBody2D _sprite;
	private RigidBody2D _ball;
	private Line2D _line;
	public override void _Ready()
	{
		foreach (Node child in GetChildren())
		{
			GD.Print(child.GetType());
		}
		_sprite = GetNode<CharacterBody2D>("player");
		_ball = GetNode<RigidBody2D>("ball");
		_line = GetNode<Line2D>("trajLine");
		
		if (_sprite == null){
			GD.Print("Sprite is null");			
		} else {
			GD.Print("Not null sprite");
		}

		if (_line == null){
			GD.Print("Line is null");
		} else {
			GD.Print("Not null line");
		}

		if (_ball == null){
			GD.Print("Ball is null");
		} else {
			GD.Print("Not null ball");
		}

	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateLineOpacity();
	}

	public void UpdateLineOpacity()
	{
		float distance = _line.GlobalPosition.DistanceTo(_sprite.GlobalPosition);
		float opacity = 10f - Mathf.Clamp(distance/100f, 0f, 1f);
		GD.Print(distance);
		GD.Print(opacity);

	}
}
