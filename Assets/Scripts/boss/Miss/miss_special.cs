using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miss_special : MonoBehaviour
{
    public Transform drop_terminal;
    public GameObject terminal;
    void Start() {
        //LeanTween.rotate(gameObject, new Vector3(0, 0, 0), 1.0f);
    }
    public void DropTerminal() {
        terminal.transform.parent = null;
        terminal.transform.position = drop_terminal.position;
    }
}
