﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

/**
 * Manages the whole description level
 */
public class well_checker : MonoBehaviour
{
    public int NUM_CONCEPTS = 5;
    private Stack<Block> blocks_inside; //So order is known

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

    public Image background_texts;
    public Image background_console;

    private void Start()
    {
        blocks_inside = new Stack<Block>();
        initDictionary();
        elapsed_time = Time.time;
        checkStyle();
    }

    void checkStyle()
    {
        if (GameObject.Find("game_manager"))
        {
            GameObject.Find("game_manager").GetComponent<game_manager>().changeStyle(background_texts, background_console);
        }
    }

    /*
     * Get the concepts and descriptions and add them to a dictionary
     */
    private void initDictionary()
    {
        descriptions_to_check = new Dictionary<string, Text>();
        for (int i = 0; i < NUM_CONCEPTS; i++)
        {
            descriptions_to_check.Add(concepts[i], descriptions[i]);
        }
        randomizeDictionary();
    }

    /*
     * Randomize descriptions order 
     */
    private void randomizeDictionary()
    {
        System.Random rng = new System.Random();
        descriptions_to_check = descriptions_to_check.OrderBy(x => rng.Next())
          .ToDictionary(item => item.Key, item => item.Value);
        enumerator = descriptions_to_check.Keys.GetEnumerator();
        nextConcept();
    }

    /*
     * If player success 
     */
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
    }

    /*
     * If player fails 
     */
    private void ejectBlocks()
    {
        foreach (Block b in blocks_inside)
        {

            foreach (Transform children in player_reference.transform)
            {
                if (children.tag != "guide")
                {
                    if (children.GetComponent<BoxCollider>() != null && b.GetComponent<BoxCollider>() != null)
                    {
                        Physics.IgnoreCollision(children.GetComponent<BoxCollider>(), b.GetComponent<BoxCollider>(), true);
                    }
                }
            }
            b.GetComponent<Rigidbody>().velocity = Vector3.zero;
            b.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 1f, -0.2f) * 3000f);
            StartCoroutine(activateCollision(b));
        }
        blocks_inside.Clear();
    }

    IEnumerator check_last_block()
    {
        yield return new WaitForSeconds(1.0f);
        string blockText = blocks_inside.Peek().GetComponent<Variable_block>().variable_written.text;

        if (blockText != currentConcept)
        {
            mistakes += 1;
            ejectBlocks();
            randomizeDictionary();
        }
        else if (blocks_inside.Count < NUM_CONCEPTS)
        {
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
            
        }
    }

    IEnumerator activateCollision(Block other)
    {
        yield return new WaitForSeconds(2.0f);

        foreach (Transform children in player_reference.transform)
        {
            if (children.tag != "guide")
            {
                if (children.GetComponent<BoxCollider>() != null && other.GetComponent<BoxCollider>() != null)
                {
                    Physics.IgnoreCollision(children.GetComponent<BoxCollider>(), other.GetComponent<BoxCollider>(), false);
                }
            }
        }

        Physics.IgnoreCollision(other.GetComponent<Collider>(), this.GetComponent<Collider>(), false);
        
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
