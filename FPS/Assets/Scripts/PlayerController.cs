using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour 
{
	   // Show up in inspector
	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 3f;

	[SerializeField]
	private float thrusterForce = 1000f;

	[SerializeField]
	private float thrusterFuelBurnSpeed = 1f;

	[SerializeField]
	private float thrusterFuelRegenSpeed = 0.3f;

	private float thrusterFuelAmount = 1f;

	public float GetThrusterFuelAmount()
	{
		return thrusterFuelAmount;
	}

	[SerializeField]
	private LayerMask enviornmentMask;

	[Header("Sprint settings")]

	[SerializeField]
	private float jointSpring = 20f;
	[SerializeField]
	private float jointMaxForce = 40f;

	// Component caching
	private Animator animator;
	private PlayerMotor motor;
	private ConfigurableJoint joint;

	void Start()
	{
		motor = GetComponent<PlayerMotor>();
		joint = GetComponent<ConfigurableJoint>();
		animator = GetComponent<Animator>();

		SetJointSettings(jointSpring);
	}

	void Update() 
	{
		// Set target Position for sprint so physics act correctly above objects
		RaycastHit _hit;
		if (Physics.Raycast (transform.position, Vector3.down, out _hit, 100f, enviornmentMask))
			joint.targetPosition = new Vector3 (0f, -_hit.point.y, 0f);
		else
			joint.targetPosition = new Vector3 (0f, 0f, 0f);

		// Movment calc as a 3D vector.
		float _xMov = Input.GetAxis("Horizontal");
		float _zMov = Input.GetAxis("Vertical");	

		Vector3 _movHorizontal = transform.right * _xMov;   // (1,0,0)
		Vector3 _movVertical = transform.forward * _zMov;   // (0,0,1)

		// normalized garauntees a vector with value 1.
		Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

		// Animate movement
		animator.SetFloat("ForwardVelocity", _zMov);
	
		// Apply Movement

		motor.Move(_velocity);

		// Calc rotation as 3D Vector (Turning around)
		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3 (0f, _yRot, 0f) * lookSensitivity;	

		// Apply Rotation
		motor.Rotate(_rotation);


				// Calc camera rotation as 3D Vector (Turning around)
		float _xRot = Input.GetAxisRaw("Mouse Y");

		float _cameraRotationX = _xRot * lookSensitivity;	

		// Apply Rotation
		motor.RotateCamera(_cameraRotationX);

		// Calc thruster force based on player input.
		Vector3 _thrusterForce = Vector3.zero;
		if (Input.GetButton ("Jump") && thrusterFuelAmount > 0f)
		{
			thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

			if (thrusterFuelAmount >= 0.01f)
			{
				_thrusterForce = Vector3.up * thrusterForce;
				SetJointSettings(0f);
			}
		}
		else
		{
			thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;

			SetJointSettings(jointSpring);
		
		}	

		thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);
		
			//Apply thruster force.
		motor.ApplyThruster(_thrusterForce);
	}

	private void SetJointSettings (float _jointSpring)
	{
		joint.yDrive = new JointDrive {
			positionSpring = _jointSpring, 
			maximumForce = jointMaxForce 
		};
	}
}
