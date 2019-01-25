using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour 
{
	[SyncVar]
	private bool _isDead = false;
	public bool isDead
	{
		get { return _isDead; }
		protected set { _isDead = value; }
	}


	[SerializeField]
	private int maxHealth = 100;

	[SyncVar]   // Each change gets pushed out to ALL clients.
	private int currentHealth;

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;

	[SerializeField]
	private GameObject[] disableGameObjectsOnDeath;

	[SerializeField]
	private  GameObject deathEffect;

	[SerializeField]
	private  GameObject spawnEffect;

	private bool firstSetup = true;

	public void SetupPlayer ()
	{
			// Switch Camera
		if (isLocalPlayer)
		{
			GameManage.instance.SetSceneCameraActive(false);
			GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
		}

		CmdBroadCastNewPlayerSetup();
	}

	[Command]
	private void CmdBroadCastNewPlayerSetup()
	{
		RpcSetupPlayerOnAllClients();
	}

	[ClientRpc]
	private void  RpcSetupPlayerOnAllClients ()
	{
		if (firstSetup)
		{
			wasEnabled = new bool [disableOnDeath.Length];
			for (int i = 0; i < wasEnabled.Length; i++)
				wasEnabled[i] = disableOnDeath[i].enabled;

			firstSetup = false;	
		}
		SetDefaults();
	}

	[ClientRpc] // Make sure method is called on all clients
	public void RpcTakeDamage (int _amount)
	{
		if (isDead)
			return;

		currentHealth -= _amount; 
		Debug.Log (transform.name + " now has " + currentHealth);

		if (currentHealth <= 0)
			Die();
	}

	private void Die()
	{
		isDead = true;

		// Disable Componenets.
		for (int i = 0; i < disableOnDeath.Length; i++)
			disableOnDeath[i].enabled = false;

		// Disable GameObjects.
		for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
			disableGameObjectsOnDeath[i].SetActive(false);

		// Disable Collider
		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = true;

		// Spawn Death Effect.
		GameObject _gfxInstance = (GameObject) Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(_gfxInstance, 3f);

		// Switch Camera
		if (isLocalPlayer)
		{
			GameManage.instance.SetSceneCameraActive(true);
			GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
		}

		Debug.Log (transform.name + " is DEAD.");

		StartCoroutine(Respawn());
	}
	// Call respawn method
	private IEnumerator Respawn ()
	{
		yield return new WaitForSeconds(GameManage.instance.matchSettings.respawnTime);

		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;
		
		yield return new WaitForSeconds(0.1f);

		SetupPlayer();

		Debug.Log (transform.name +" respawned.");
		
	}
	

	public void SetDefaults ()
	{
		isDead = false;

		currentHealth = maxHealth;
		
		// Enable components.
		for (int i = 0; i < disableOnDeath.Length; i++)   // Reset to orig state.
			disableOnDeath[i].enabled = wasEnabled[i];

		// Enable GameObjects.
		for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
			disableGameObjectsOnDeath[i].SetActive(true);

		// Enable Collider.
		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = true;
		

		// Create Spawn Effect.
		// Spawn Death Effect.
		GameObject _gfxInstance = (GameObject) Instantiate(spawnEffect, transform.position, Quaternion.identity);
		Destroy(_gfxInstance, 3f);
	}	


	void Update()
	{
		if (!isLocalPlayer)
			return;

		if (Input.GetKeyDown(KeyCode.K))
			RpcTakeDamage(9999);
	}  
}
