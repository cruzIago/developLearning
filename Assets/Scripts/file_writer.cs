using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * This class opens, writes and closes to a file
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
        writer = new StreamWriter(path);
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
