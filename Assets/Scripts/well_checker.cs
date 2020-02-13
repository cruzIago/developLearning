using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class well_checker : MonoBehaviour
{
    private List<GameObject> blocks_inside;

    public Text[] descriptions_to_check;

    private void Start()
    {
        blocks_inside = new List<GameObject>();
    }

    IEnumerator check_blocks()
    {
        yield return new WaitForSeconds(1.0f);
        foreach (GameObject b in blocks_inside)
        {
            //b.transform.Translate(new Vector3(0,2,7));
            b.GetComponent<Rigidbody>().velocity = Vector3.zero;
            b.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 1f, -0.2f) * 2000f);
            StartCoroutine(activateCollision(b));
            print("Moved");
        }
        blocks_inside.Clear();
    }

    IEnumerator activateCollision(GameObject other)
    {
        yield return new WaitForSeconds(2.0f);
        Physics.IgnoreCollision(other.GetComponent<Collider>(), this.GetComponent<Collider>(), false);
        print("Active collision");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("block"))
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
            blocks_inside.Add(collision.gameObject);
            StartCoroutine(check_blocks());
        }
    }
}
