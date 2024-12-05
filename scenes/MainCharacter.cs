using Godot;
using System;

public partial class MainCharacter : CharacterBody2D
{
	[Export]
	
	public float Speed = 300.0f;
	public float JumpVelocity = -400.0f;
	public float Gravity = 980.0f;
	public int LinePoints = 40;

	public float maxDistance = 100f;

	private AnimatedSprite2D _sprite;
	private RigidBody2D _ball;
	private Line2D _line;
	private bool _isFacingLeft = false;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("Sprite2D");
		_ball = GetParent().GetNode<RigidBody2D>("ball");
		_line = GetParent().GetNode<Line2D>("trajLine");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		UpdateMovement(ref velocity, delta);
		UpdateAnimation(velocity);
		UpdateFacing(velocity);
		UpdateLineOpacity();

		Velocity = velocity;
		MoveAndSlide();
		
		UpdateLine((float)delta);
	}

	private void UpdateMovement(ref Vector2 velocity, double delta)
	{
		if (!IsOnFloor())
		{
			velocity.Y += Gravity * (float)delta;
		}

		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		velocity.X = direction.X != 0 ? direction.X * Speed : Mathf.MoveToward(velocity.X, 0, 20);
	}

	private void UpdateAnimation(Vector2 velocity)
	{
		string animation = "default";
		if (!IsOnFloor())
			animation = "jumping";
		else if (Mathf.Abs(velocity.X) > 1)
			animation = "running";

		_sprite.Play(animation);
	}

	private void UpdateFacing(Vector2 velocity)
	{
		if (velocity.X != 0)
		{
			_isFacingLeft = velocity.X < 0;
			_sprite.FlipH = _isFacingLeft;
		}
	}

	private void UpdateLine(float delta)
	{
		
		if (_line == null || _ball == null)
		{
			GD.PrintErr($"Line is null: {_line == null}, Ball is null: {_ball == null}");
			return;
		}
		
		_line.ClearPoints();
		//_line.AddPoint(GlobalPosition);

		Vector2 playerPosition = GlobalPosition;
		Vector2 ballPos = _ball.GlobalPosition;
		//Vector2 ballVelocity = _ball.LinearVelocity;
		Vector2 direction = (ballPos - playerPosition).Normalized();
		
		Vector2 extendedPoint = ballPos + direction * 100;
		
		_line.AddPoint(ballPos);
		_line.AddPoint(extendedPoint);
	}

	private void UpdateLineOpacity()
	{
		float distance = _line.GlobalPosition.DistanceTo(GlobalPosition);
		float opacity = 10f - Mathf.Clamp(distance / maxDistance, 0f, 1f);

		// GD.Print(Mathf.Clamp(distance / maxDistance, 0f, 1f));
		// GD.Print(distance/maxDistance);

		// Color currentColor = _line.DefaultColor;
		// currentColor.A = opacity;
		// _line.DefaultColor = currentColor;
	}
}
