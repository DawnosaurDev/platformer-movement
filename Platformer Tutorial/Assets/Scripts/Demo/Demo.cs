using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    public DemoData[] options;

    private void Start()
    {
        #region Setup
        foreach (DemoData data in options)
        {
            if (data.currentObj < 0 || data.currentObj >= data.objects.Length)
                data.currentObj = 0;

            for (int i = 0; i < data.objects.Length; i++)
            {
                if (i == data.currentObj)
                    SetObject(data, i);
                else
                    data.objects[i].SetActive(false);
            }
        }
        #endregion
    }

    private void Update()
    {
        #region Inputs
        foreach (DemoData data in options)
        {
            if (Input.GetKeyDown(data.changeObjKey))
                NextObject(data);
        }
        #endregion
    }

    private void NextObject(DemoData data)
    {
        if (data.currentObj < data.objects.Length - 1)
            SetObject(data, data.currentObj + 1);
        else
            SetObject(data, 0);
    }

    private void SetObject(DemoData data, int newObj)
    {
        data.objects[data.currentObj].SetActive(false);
        data.currentObj = newObj;
        data.objects[data.currentObj].SetActive(true);
    }

    [System.Serializable]
    public class DemoData
    {
        [SerializeField] private string name;

        public GameObject[] objects;
        public int currentObj;
        public KeyCode changeObjKey;
    }
}
