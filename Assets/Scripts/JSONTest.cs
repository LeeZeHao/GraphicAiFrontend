using System.Collections.Generic;
using UnityEngine;

public class JSONTest : MonoBehaviour
{
    void Start() {
        TestObject testObject = new TestObject();
        testObject.strings = new List<string>{"Volvo", "BMW", "Ford", "Mazda"};

        string json = JsonUtility.ToJson(testObject);
        Debug.Log(json);

        TestObject extractedTestObject = JsonUtility.FromJson<TestObject>(json);
        List<string> extractedStrings = extractedTestObject.strings;
        Debug.Log(extractedStrings.Count);
        Debug.Log(extractedStrings[1]);
    }

    private class TestObject {
        public List<string> strings;
    }

    // OK so storing string arrays works
}
