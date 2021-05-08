using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerBottom : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rigidbodyOfObjectInDrawer;
            collision.gameObject.TryGetComponent<Rigidbody>(out rigidbodyOfObjectInDrawer);

            //remove the rigidbody
            Destroy(rigidbodyOfObjectInDrawer);

            //Parent the object to the drawer body (sibling of the bottom)
            transform.SetParent(collision.gameObject.transform.parent);
        }
    }
}
