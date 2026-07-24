using Godot;
using System;
using System.ComponentModel;


public partial class CameraManager : Camera2D
{
	[Export]
	public Node2D player;

	private Vector2 offset;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		offset = Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 movePosition = player.Position + offset;
		Position = movePosition;
		
	}
}
