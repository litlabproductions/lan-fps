using UnityEngine;
using UnityEngine.Networking;

	/*
		All players check if they hit somthing locally

		**IF THEY DO: Send over ID out of what they hit and desired damage amount to be dealt.

		Calls server --> Server finds player by ID (Game Manage component funtion)
		Puts damage passed into takeDamage() function of player found by server above.

		Unity recognizes that this sync var for currentHealth has changed,
		then sends the new info out to all the clients.
	 */
[RequireComponent (typeof (WeaponManager))]
public class PlayerShoot : NetworkBehaviour 
{
	private const string PLAYER_TAG = "Player";

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	private PlayerWeapon currentWeapon;
	private WeaponManager weaponManager;

	void Start ()
	{
		if (cam == null)
		{
			Debug.LogError("PlayerShoot: No camera referenced!");
			this.enabled = false;
		}

		weaponManager = GetComponent<WeaponManager>();
	}

	void Update ()
	{
		currentWeapon = weaponManager.GetCurrentWeapon();

		if (currentWeapon.fireRate <= 0f)
		{
			if (Input.GetButtonDown("Fire1"))
				Shoot();
		} 
		else
		{
			if (Input.GetButtonDown("Fire1"))
				InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
			else if (Input.GetButtonUp ("Fire1"))
				CancelInvoke("Shoot");
		}
	}

	[Command]
	void CmdOnShoot ()   // Called on sewrver when somone shoots.
	{
		RpcDoShootEffect();	
	}

	[ClientRpc]     // Called on on client to produce shooting effects
	void RpcDoShootEffect()
	{
		weaponManager.GetCurrentGraphics().muzzleFlash.Play();
	}

	[Command]    // Called on server when we hit somthing 
	void CmdOnHit(Vector3 _pos, Vector3 _normal)
	{
		RpcDoHitEffect (_pos, _normal);
	}

	[ClientRpc]     // Called on on client, here we spawn in effects/
	void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
	{
		GameObject _hitEffect = (GameObject) Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
		Destroy(_hitEffect, 2f);
	}


	[Client]  // Never called on server.
	void Shoot ()
	{
		if (!isLocalPlayer)
			return;

			// Shooting, call OnShoot method on server.
		CmdOnShoot(); 

		RaycastHit _hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask) )
		{
			if (_hit.collider.tag == PLAYER_TAG)
			{	
				CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
			}

				// We hit somthing call onHit to the server
			CmdOnHit(_hit.point, _hit.normal);
		}
	}

	[Command]
	void CmdPlayerShot (string _playerID, int _damage)
	{
		Debug.Log(_playerID + " has been shot.");

		Player _player = GameManage.GetPlayer(_playerID);
		_player.RpcTakeDamage(_damage);
		//GameObject.Find(_ID);
	}
}