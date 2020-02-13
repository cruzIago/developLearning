using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checker : MonoBehaviour
{

    private Stack<Block> blockStack;
    public string currentName;

    // Start is called before the first frame update
    void Start()
    {
        blockStack = new Stack<Block>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addBlock(Block b)
    {
        blockStack.Push(b);
    }

    public void removeLast()
    {
        Block ejectedBlock = blockStack.Pop();
        print(ejectedBlock.textGuide.GetComponentInChildren<Text>().text + " was poped from the stack");
        print("Remaining blocks: " + blockStack.Count);
    }

    public void removeAll()
    {
        while (blockStack.Count != 0)
        {
            removeLast();
        }
    }

    public bool isEmpty()
    {
        return blockStack.Count == 0;
    }

    public void checkLastBlock()
    {
        string blockText = blockStack.Peek().textGuide.GetComponentInChildren<Text>().text;
        if (blockText != currentName)
        {
            print("EFE: BlockText es " + blockText + " y mi nombre es: " + currentName);
            removeAll();
        }
        else
        {
            print("Todo ok");
            print("Remaining blocks: " + blockStack.Count);
        }
    }
}
