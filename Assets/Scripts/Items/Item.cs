using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool taken = false;

    public Transform ReturnCurrentTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 5f, LayerMask.GetMask("Ground")))
        {
            //Debug.Log(hit.transform.parent.parent.name);
            return hit.transform.parent.parent;
        }
        else
        {
            Debug.Log("No Tile");
            return null;
        }
    }
    public void Destroy()
    {
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }

}
