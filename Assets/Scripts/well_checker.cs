using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class well_checker : MonoBehaviour
{
    private Stack<Block> blocks_inside;
    public string askedName;

    private void Start()
    {
        blocks_inside = new Stack<Block>();
    }

    private void ejectBlocks()
    {
        foreach (Block b in blocks_inside)
        {
            //b.transform.Translate(new Vector3(0,2,7));
            b.GetComponent<Rigidbody>().velocity = Vector3.zero;
            b.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 1f, -0.2f) * 2000f);
            StartCoroutine(activateCollision(b.gameObject));
            print("Moved");
        }
        blocks_inside.Clear();
    }

    IEnumerator check_last_block()
    {
        yield return new WaitForSeconds(1.0f);
        string blockText = blocks_inside.Peek().GetComponent<Variable_block>().getVariableName();
        if (blockText != askedName)
        {
            print("EFE: BlockText es " + blockText + " y mi nombre es: " + askedName);
            ejectBlocks();
        }
        else
        {
            print("Todo ok");
            print("Remaining blocks: " + blocks_inside.Count);
        }

        //print("Actually not doing nothing lmao");
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
            blocks_inside.Push(collision.gameObject.GetComponent<Block>());
            StartCoroutine(check_last_block());
        }
    }
}
