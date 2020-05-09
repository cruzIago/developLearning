using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * This class writes on the file to send some information 
 */
public class file_writer
{
    private StreamWriter writer;
    public string path;

    public file_writer()
    {
        openStream();
    }

    private void openStream()
    {
        path = "logFile.txt";
        writer = new StreamWriter(path, true);
    }

    public void writeOnFile(string line)
    {
        writer.WriteLine(line);
    }

    public void closeStream()
    {
        writer.Close();
    }
}
