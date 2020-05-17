using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class well_checker : MonoBehaviour
{
    public int NUM_CONCEPTS = 5;
    private Stack<Block> blocks_inside;

    public Text[] descriptions;
    private Text currentDesc;

    public string[] concepts = { "Variable", "Memoria", "Instrucción", "Secuencia", "Programa" };

    private Dictionary<string, Text> descriptions_to_check;
    private string currentConcept;
    IEnumerator enumerator;

    public GameObject player_reference;

    private int mistakes;
    private float elapsed_time;

    public bool is_review_stage;
    public review_manager reviewer;

    private void Start()
    {
        blocks_inside = new Stack<Block>();
        initDictionary();
        elapsed_time = Time.time;
    }

    private void initDictionary()
    {
        descriptions_to_check = new Dictionary<string, Text>();
        for (int i = 0; i < NUM_CONCEPTS; i++)
        {
            descriptions_to_check.Add(concepts[i], descriptions[i]);
        }
        randomizeDictionary();
    }

    private void randomizeDictionary()
    {
        System.Random rng = new System.Random();
        descriptions_to_check = descriptions_to_check.OrderBy(x => rng.Next())
          .ToDictionary(item => item.Key, item => item.Value);
        enumerator = descriptions_to_check.Keys.GetEnumerator();
        nextConcept();
    }

    private void nextConcept()
    {
        if (currentDesc != null)
        {
            currentDesc.gameObject.SetActive(false);
        }
        enumerator.MoveNext();
        currentConcept = enumerator.Current.ToString();
        descriptions_to_check.TryGetValue(currentConcept, out currentDesc);
        currentDesc.gameObject.SetActive(true);
        print(currentConcept);
    }

    private void ejectBlocks()
    {
        foreach (Block b in blocks_inside)
        {
            //b.transform.Translate(new Vector3(0,2,7));

            foreach (Transform children in player_reference.transform)
            {
                if (children.tag != "guide")
                {
                    Physics.IgnoreCollision(children.GetComponent<BoxCollider>(), b.GetComponent<BoxCollider>(), true);
                }
            }
            b.GetComponent<Rigidbody>().velocity = Vector3.zero;
            b.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 1f, -0.2f) * 3000f);
            StartCoroutine(activateCollision(b));
            print("Moved");
        }
        blocks_inside.Clear();
    }

    IEnumerator check_last_block()
    {
        yield return new WaitForSeconds(1.0f);
        string blockText = blocks_inside.Peek().GetComponent<Variable_block>().variable_written.text;

        if (blockText != currentConcept)
        {
            print("EFE: BlockText es " + blockText + " y mi nombre es: " + currentConcept);
            mistakes += 1;
            ejectBlocks();
            randomizeDictionary();
        }
        else if (blocks_inside.Count < NUM_CONCEPTS)
        {
            print("Todo ok. Cantidad: " + blocks_inside.Count);
            nextConcept();
        }
        else
        {
            if (!is_review_stage)
            {
                int stars = 0;
                if (mistakes <= 1)
                {
                    stars = 3;
                }
                else if (mistakes <= 3)
                {
                    stars = 2;
                }
                else
                {
                    stars = 1;
                }
                elapsed_time = Time.time - elapsed_time;

                scene_manager.checkEndScreen(stars, elapsed_time, mistakes);
            }
            else
            {
                reviewer.nextReview(mistakes);
            }

            print("Se ha acabado el juego y has ganado");
        }
    }

    IEnumerator activateCollision(Block other)
    {
        yield return new WaitForSeconds(2.0f);

        foreach (Transform children in player_reference.transform)
        {
            if (children.tag != "guide")
            {
                Physics.IgnoreCollision(children.GetComponent<BoxCollider>(), other.GetComponent<BoxCollider>(), false);
            }
        }

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
