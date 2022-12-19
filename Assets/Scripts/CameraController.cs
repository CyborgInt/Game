using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform arms;
    [SerializeField] private Transform body;

    private PlayerStats stats;

    private float xRot;

    private void Start()
    {
        GetReferences();
        LockCursor();
    }

    private void Update()
    {
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        /// Ã≈“Œƒ Œ“—À≈∆»¬¿ﬁŸ»… ƒ¬»∆≈Õ»≈ Ã€ÿ» ƒÀﬂ ”œ–¿¬ÀÕ≈Õ»ﬂ  ¿Ã≈–Œ…

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90);

        if (!stats.IsDead())
        {
            arms.localRotation = Quaternion.Euler(new Vector3(xRot, 0, 0));
            body.Rotate(new Vector3(0, mouseX, 0));
        }
        else if (Cursor.lockState == CursorLockMode.Locked)
            UnlockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void GetReferences()
    {
        stats = GetComponentInParent<PlayerStats>();
    }
}