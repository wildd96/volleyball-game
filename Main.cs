using Godot;
using System;

public partial class Main : Node
{
	private MainCharacter _sprite;
	private RigidBody2D _ball;
	private Line2D _line;
	private Vector2 _servePosition = new Vector2(160, 415);
	private float _serveVelocity = 350f;
	private bool _serving = true;
	private Vector2 _angle;
	
	public override void _Ready()
	{
		_sprite = GetNode<MainCharacter>("player");
		_ball = GetNode<RigidBody2D>("ball");
		_line = GetNode<Line2D>("trajLine");
	}

	public override void _Process(double delta)
	{
		UpdateLineOpacity();
		Serve();
		_angle = _sprite.UpdateLine(0f);
		
		if (Input.IsActionJustPressed("hit"))
		{
			Hit();
		}

	}
	
	public void Hit()
	{
		float maxPower = UpdateLineOpacity();
		
		_ball.LinearVelocity = (750f * maxPower) * _angle;
	}
	
	public void Serve()
	{

		if (!_serving)
		{
			return;
		}
		
		_ball.GlobalPosition = _servePosition;
		_serving = false;
		_ball.LinearVelocity = Vector2.Up * _serveVelocity;
	}

	public float UpdateLineOpacity()
	{
		Vector2 ballPos = _ball.GlobalPosition;
		Vector2 spritePos = _sprite.GlobalPosition;
		float distance = ballPos.DistanceTo(spritePos);
		float period = 200f;
		float maxOpacity = 1.5f;
		float minOpacity = 0f;
		float phaseShift = Mathf.Pi/3f;

		float opacity = 100f/(distance*2);

		if(distance > 200)
		{
			opacity = 0;
		}
		
		_line.Modulate = new Color(_line.Modulate.R, _line.Modulate.G, _line.Modulate.B, opacity);
		
		return opacity;
	}
}
