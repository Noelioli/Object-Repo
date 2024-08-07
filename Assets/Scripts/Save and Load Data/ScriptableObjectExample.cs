using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSample", menuName = "CircuitStream/ Create Sample ScriptableObject")]
public class ScriptableObjectExample : ScriptableObject
{
    public string objectName;
    public string score;
    public Vector2 startPosition;
}
