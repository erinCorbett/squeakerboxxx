using Godot;
using System;

public partial class PlayerControllerBody2D : CharacterBody2D
{
	[Export]
	public float  moveSpeed = 5.0f;

	[Export]
	public float Acceleration = 100f;
	[Export]
	public float Deceleration = 100f;


	[Export]
	public float jumpForce;

	[Export]
	public float fallMultiplier = 2.5f;
	
	[Export]
	public float lowJumpMultiplier = 2f;

	[Export]
	public Area2D groundCheck;

	[Export]
	public Sprite2D playerSprite;
	


	[Export]
	public int maxJumpCount = 2;

	[Export]
	public TileMapLayer ground;


	public float jumpBufferTime = 0.2f;
	public float coyoteTime = 0.2f;

	private Vector2 moveInput;
	
	private Vector2 initialPos;
	
	private bool isFacingRight = true;

	[Export]
	public float fallRespawnHeight = 2000.0f;

	
	private float coyoteTimeCounter;
	
	private int jumpCount;


	//START 
	public override void _Ready() {
		Input.MouseMode = Input.MouseModeEnum.Captured;

		initialPos = new Vector2(GlobalPosition.X, GlobalPosition.Y);
	}

	//UPDATE
	public override void _Process(double delta)
	{
		
		CheckForHeight();
	}


	//PHYSICS LOOP // FIXED UPDATE
	public override void _PhysicsProcess(double delta)
	{

		bool isGrounded = IsOnFloor();

		// //Coyote Time!
		if (!isGrounded)
		{           
			coyoteTimeCounter -= (float)delta;

			
		} else if(isGrounded)
		{
			//Reset Coyote Time and Jump Count
			coyoteTimeCounter = coyoteTime;
			jumpCount = 0;
		}
		

		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!isGrounded)
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && jumpCount<maxJumpCount)
		{
			if(isGrounded && jumpCount==0 || !isGrounded && jumpCount>0 || coyoteTimeCounter>0 && jumpCount==0) {
				jumpCount++;
				velocity.Y = -jumpForce;
			}
		}

		// Get the input direction and handle the movement/deceleration.
		float direction = Input.GetAxis("left", "right");
		if (direction != 0.0)
		{
			velocity.X = direction *moveSpeed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0,moveSpeed);
		}

		
		if (Velocity.Y < 0 && !Input.IsActionPressed("jump"))
		{
			velocity += Vector2.Up * 980 * (lowJumpMultiplier - 1) * (float)delta*-1;
		
		}
		else if (Velocity.Y > 0)
		{
			velocity += Vector2.Up * 980 * (fallMultiplier - 1) * (float)delta*-1;
		}

		Velocity = velocity;
		
		HandleCharacterFlip();
		MoveAndSlide();

	}

	//HANDLE CHARACTER FLIP
	private void HandleCharacterFlip()
	{
		
		if (Velocity.X > 0 && !isFacingRight)
		{
			Flip();
		}
		else if (Velocity.X < 0 && isFacingRight)
		{
			Flip();
		}
	}

	private void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector2 scaler = playerSprite.Scale;

		
		scaler.X *= -1;
		playerSprite.Scale = new Vector2(scaler.X, scaler.Y);
	}


	private void CheckForHeight()
	{
		if(GlobalPosition.Y>fallRespawnHeight)
		{
			GlobalPosition = initialPos;
		}
		
	}


}
