using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string NPCname;

    [TextArea(1, 20)]
    public string[] sentences;
}
