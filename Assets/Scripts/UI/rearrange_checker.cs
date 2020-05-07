using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class rearrange_checker : MonoBehaviour
{
    Block fixed_block;
    public RectTransform gui_position;
    Text ownText;

    /*
     * Sets the position of the block so it stays in place until player moves it 
     */
    public void fixBlock(Block block)
    {
        block.GetComponent<Block>().pickDown();
        fixed_block = block;
        fixed_block.transform.position = this.transform.position + Vector3.up;
        fixed_block.transform.rotation = this.transform.rotation;
        fixed_block.GetComponent<Rigidbody>().isKinematic = true;
    }

    /*
     * For manager to check blocks and if its right 
     */
    public Block getBlock()
    {
        return fixed_block;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "item" && fixed_block == null)
        {
            fixBlock(other.GetComponent<Block>());
            GetComponent<Renderer>().material.SetColor("_Color",(fixed_block.GetComponent<Reorder_block>().block_color)-new Color(0.5f,0.5f,0.5f,0f));
            fixed_block.GetComponent<Reorder_block>().reorder_text.gameObject.SetActive(true);
            fixed_block.GetComponent<Reorder_block>().reorder_text.transform.position = gui_position.transform.position;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "item")
        {
            if (other.GetComponent<Block>() == fixed_block)
            {
                if (!fixed_block.player.GetComponent<player_controller>().isItemHeld)
                {
                    fixed_block.GetComponent<Rigidbody>().isKinematic = false;
                }
                GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
                fixed_block.GetComponent<Reorder_block>().reorder_text.gameObject.SetActive(false);
                fixed_block = null;
            }
        }
    }

}
