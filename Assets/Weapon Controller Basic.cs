using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour, PlayerEric.IPlayerActions
{
    public Transform shootSpawn;
    public bool shooting;
    public GameObject bulletPrefab;
    private PlayerEric playerControls;
    private PlayerEric playerEric;

    void Awake ()
    {
        playerControls = new PlayerEric ();
        playerControls.Player.SetCallbacks (this);
    }

    void OnEnable ()
    {
        playerControls.Player.Enable ();
    }


    void OnDisable ()
    {
        playerControls.Player.Disable ();
    }


    public void InstantiateBullet()
    {
        Instantiate (bulletPrefab, shootSpawn.position, shootSpawn.rotation);
    }

    public void OnMove (InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException ();
    }

    public void OnLook (InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException ();
    }

    public void OnFire (InputAction.CallbackContext context)
    {
        InstantiateBullet();

    }
}
