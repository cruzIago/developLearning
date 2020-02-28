using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class file_writer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void writeFile()
    {
        string path = "log.txt";
        string message = "Hello";

        //Write message
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(message);
        writer.Close();
    }
}
