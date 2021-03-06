using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public ControlSettings settings;
    public Vector3 localRotation;
    private Ray ray;
    private bool isShoot;
    private bool canShoot = true;
    public float shootDelay = 0.080f;
    public LayerMask layerMask;
    private RaycastHit hit;
    private string filePath;
    private float sensOffset = 0.1f;

    private TargetComponent target;
    private GameObject targetObject;

    private void Awake()
    {
        controls = new Controls();
        filePath = Path.Combine(Application.persistentDataPath, "Controls.json");
        Load();
    }


    private void OnEnable()
    {
        aim = controls.Game.Aim;
        shoot = controls.Game.Shoot;
        shoot.Enable();
        aim.Enable();
        shoot.performed += Shoot;
    }

    private void Update()
    {
        MoveMouse();
        transform.localEulerAngles = localRotation;
    }

    private void FixedUpdate()
    {
        if (isShoot)
        {
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (!target)
                {
                    if (hit.transform.gameObject.TryGetComponent<TargetComponent>(out target))
                    {
                        targetObject = hit.transform.gameObject;
                        SendTargetHit();
                    }
                }
                else
                {
                    if (hit.transform.gameObject == targetObject)
                        SendTargetHit();
                }
            };
            isShoot = false;
        }
    }

    private void SendTargetHit()
    {
        target.Hit();
    }

    private void Shoot(InputAction.CallbackContext obj)
    {
        if (canShoot)
        {
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

    private void MoveMouse()
    {
        localRotation.y += aim.ReadValue<Vector2>().x * settings.sensitivity * sensOffset;
        localRotation.x += aim.ReadValue<Vector2>().y * -settings.sensitivity * sensOffset;
        localRotation.y %= 360f;
        localRotation.x = Mathf.Clamp(localRotation.x, -90f, 90f);
    }

    private void OnDisable()
    {
        //aim.performed -= MouseMove;
        shoot.performed -= Shoot;
        shoot.Disable();
        aim.Disable();
    }



    private void Load()
    {
        if (!File.Exists(filePath))
        {
            LoadDefault();
            return;
        }
        var json = File.ReadAllText(filePath);
        settings = JsonUtility.FromJson<ControlSettings>(json);
    }
    private void Save()
    {
        var json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(filePath, json);
    }

    public void LoadDefault()
    {
        settings = new ControlSettings(0.25f);
    }

    public void UpdateSettings(ControlSettings settings)
    {
        this.settings = settings;
        Save();
    }

}
