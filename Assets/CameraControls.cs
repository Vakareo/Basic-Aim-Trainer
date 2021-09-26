using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    private Controls controls;
    private InputAction aim;
    private InputAction shoot;
    public float sensitivity = 1;
    public Vector3 localRotation;
    private Ray ray;
    private bool isShoot;
    private bool canShoot = true;
    public float shootDelay = 0.080f;
    public LayerMask layerMask;
    private RaycastHit hit;

    private void Awake()
    {
        controls = new Controls();
    }


    private void OnEnable()
    {
        aim = controls.Game.Aim;
        shoot = controls.Game.Shoot;
        shoot.Enable();
        aim.Enable();
        shoot.performed += Shoot;
        aim.performed += MouseMove;

    }

    private void Update()
    {
        transform.localEulerAngles = localRotation;
    }

    private void FixedUpdate()
    {
        if (isShoot)
        {
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {

            };
            isShoot = false;
        }
    }

    private void Shoot(InputAction.CallbackContext obj)
    {
        if (canShoot)
        {
            Debug.Log("shoot");
            ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            isShoot = true;
            StartCoroutine(DelayNextShot());
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(hit.point, 0.15f);
    }

    IEnumerator DelayNextShot()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    private void MouseMove(InputAction.CallbackContext obj)
    {
        localRotation.y += obj.ReadValue<Vector2>().x * sensitivity;
        localRotation.x += obj.ReadValue<Vector2>().y * -sensitivity;
        localRotation.y %= 360f;
        localRotation.x = Mathf.Clamp(localRotation.x, -90f, 90f);
    }

    private void OnDisable()
    {
        aim.performed -= MouseMove;
        shoot.performed -= Shoot;
        shoot.Disable();
        aim.Disable();
    }
}
