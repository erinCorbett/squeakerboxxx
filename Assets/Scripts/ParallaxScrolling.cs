using Godot;
using System;

public partial class ParallaxScrolling : Sprite2D
{
	// Called when the node enters the scene tree for the first time.

	[Export]
	public Camera2D mainCamera;

	Vector2 initPos;
	public override void _Ready()
	{
		initPos = GlobalPosition;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalPosition = mainCamera.GlobalPosition + initPos;
		
	}
}
