using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidMovement : MonoBehaviour
{
    [Header("Classes")]
    public CharacterController2D controller;
    ShootScript shootScript;

    [Header("Float")]
    public float dashSpeed = 80;
    public float runSpeed = 1f;
    public float dashCoolDown = 0;
    public float shootCounter;
    public float horizontalMove = 0f;
    public float verticalMove = 0f;

    [Header("Vectors")]
    Vector3 lastPosition;
    Vector2 movement;

    [Header("Bools")]
	public bool isDashing;
	public bool crouch = false;


    private void Start()
	{
		shootScript = GetComponent<ShootScript>();
	}

	void Update()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;

		movement = new Vector2(horizontalMove,verticalMove);
		movement.Normalize();
	}
	
	void FixedUpdate()
	{
		transform.Translate(movement * 5 * Time.deltaTime);
	}


}
