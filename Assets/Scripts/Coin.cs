using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Coin : Area2D
{
	// Called when the node enters the scene tree for the first time.
	
	[Export]
	public AnimatedSprite2D sprite;
	[Export]
	public CollisionShape2D collider;

	[Export]
	public CpuParticles2D coinBurst;

	
	private float coinTimer, coinReturnCounter;
	public override void _Ready()
	{

		sprite.Play("default");
		coinTimer = 1.0f;
		coinReturnCounter = 0.0f;
		spawnBurst = true;
	}

	bool spawnBurst;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		
		if(HasOverlappingBodies() && spawnBurst == true)
		{
			coinReturnCounter = coinTimer;
			coinBurst.Emitting = true;
		}

		if(coinReturnCounter>0) {
			spawnBurst = false;
			coinReturnCounter -= (float)delta;
			sprite.Visible = false;
		}
		else {
			spawnBurst = true;
			coinReturnCounter = 0;
			sprite.Visible = true;
		}


		
	}

	private void OnAreaEntered(Area2D body)
	{
		coinReturnCounter = coinTimer;
	}
}
