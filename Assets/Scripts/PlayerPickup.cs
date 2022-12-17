using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private float pickupRange;
    [SerializeField] private LayerMask pickupLayer;

    private Camera cam;
    private Inventory inventory;

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        /// лернд бшгшбючыхи ондмърхе нпсфхъ
        /// я онлныч цемепюжхх ксвю мюопюбкемхъ, нопедекъер назейр мюбедемхъ
        /// дкъ ецн ондмърхъ, х днаюбкъер ецн б хмбемрюпэ

        if(Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
            {
                Debug.Log("Hit: " + hit.transform.name);
                Weapon newItem = hit.transform.GetComponent<ItemObject>().item as Weapon;
                inventory.AddItem(newItem);
                Destroy(hit.transform.gameObject);
            }
        }
    }

    private void GetReferences()
    {
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
    }
}
