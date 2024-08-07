using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SampleData sample = new SampleData();
        sample.name = "Test";
        sample.score = 10f;

        string data = JsonUtility.ToJson(sample);

        string JSON = "{\n\t\"name\": \"Alice\", \n\t\"score\": 90.3\n}";
        SampleData sample2 = JsonUtility.FromJson<SampleData>(JSON);

        Debug.Log($"Deserialized {sample2.name} - Score : {sample2.score}");
    }
}

[System.Serializable]
public class SampleData
{
    public string name;
    public float score;
}