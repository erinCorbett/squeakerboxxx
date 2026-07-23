using Godot;
using System;



public partial class Player : RigidBody2D
{
	[Export]
	public float  Movespeed = 5.0f;

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

	private bool isGrounded, isWalled;
	
	private bool isFacingRight = true;


	 //  [Header("Ground and Wall Detection Settings")]
	//public Transform groundCheck, wallCheck; // Empty GameObject to check ground
	public float groundCheckRadius = 0.2f; // Radius for ground detection
	public float wallCheckRadius = 0.2f;

	public float fallRespawnHeight = -50.0f;

	
	private float jumpBufferCounter, coyoteTimeCounter;
	
	private int jumpCount;
	
	public override void _Ready() {
		initialPos = new Vector2(this.GlobalPosition.X, this.GlobalPosition.Y);
	}

	public override void _Process(double delta)
	{
		// Called every frame. Delta is time since the last frame.
		// Update game logic here.

		//GD.Print(jumpCount + ", " + isGrounded);
		//HandleVariableJump(delta);
		HandleInput(delta);
		CheckForHeight();
		if(isGrounded)
			jumpCount = 0;
	}


	public override void _PhysicsProcess(double delta)
	{
		if(LinearVelocity.Y != 0)
			CheckGroundStatus();
		HandleMovement(delta);
		HandleVariableJump(delta);
		
	}

	
	private void HandleInput(double delta)
	{
		//Coyote Time!
		if (!isGrounded)
		{           
			coyoteTimeCounter -= (float)delta;

			//Checks to see if Coyote Time has run out before adding to JumpCount
			if(coyoteTimeCounter<=0&&jumpCount==0)
			{
				jumpCount++;
			}
			
		} else if(isGrounded)
		{
			//Reset Coyote Time and Jump Count
			coyoteTimeCounter = coyoteTime;
			jumpCount = 0;
		}

		//Get the so-called "Jump" Input
		 if (Input.IsActionJustPressed("ui_accept"))
		 {
			//Checks that the jump buffer is empty and then Start JumpBufferCounter
			if(jumpBufferCounter == 0) {
			  jumpBufferCounter = jumpBufferTime;
			  Jump();
			}
		 } else
		{
			if(jumpBufferCounter>0f)
				jumpBufferCounter -= (float)delta;
				else
					jumpBufferCounter = 0;
		}
		//Flips Character
		HandleCharacterFlip();
	}

	private void HandleCharacterFlip()
	{
		if (moveInput.X > 0 && !isFacingRight)
		{
			Flip();
		}
		else if (moveInput.X < 0 && isFacingRight)
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

	private void HandleMovement(double delta)
	{
		

		//Get Horizontal Axis
		 moveInput = new Vector2(Input.GetAxis("ui_left", "ui_right"), 0);

		//Accelerates/Decelerate movement
		float targetSpeed = moveInput.X *  Movespeed;
		float speedDifference = targetSpeed - LinearVelocity.X;
		float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Acceleration : Deceleration;
		float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelRate, 0.9f) * Mathf.Sign(speedDifference);

		//Add Horizontal Movement
		ApplyForce(movement * Vector2.Right);
		

		// Clamp the velocity
		LinearVelocity = new Vector2(Mathf.Clamp(LinearVelocity.X, - Movespeed,  Movespeed), LinearVelocity.Y);
		
		 
	}

	private void CheckForHeight()
	{
		
	}
	private void Jump()
	{
		
		//Checks for jumpBuffer and CoyoteTimer, OR if you have any jumps left and if so then LEAP princess! 
		// if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f || jumpCount < maxJumpCount)
		if (jumpCount < maxJumpCount || jumpCount < maxJumpCount)
		{	
			
			jumpCount++;
			LinearVelocity = new Vector2(LinearVelocity.X, -jumpForce);
		}

	}

	private void CheckGroundStatus()
	{
	// 	isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
		isGrounded = groundCheck.OverlapsBody(ground);
	}


	public Vector2 VariableJumpVelocity()
	{
		return new Vector2(0,0);
	}

	private void HandleVariableJump(double delta)
	{
		
		
		if (LinearVelocity.Y < 0 && !Input.IsActionPressed("ui_accept"))
		{
			LinearVelocity += Vector2.Up * 980 * (lowJumpMultiplier - 1) * (float)delta*-1;
		
		}
		else if (LinearVelocity.Y > 0)
		{
			LinearVelocity += Vector2.Up * 980 * (fallMultiplier - 1) * (float)delta*-1;
		}
		
	}
}
