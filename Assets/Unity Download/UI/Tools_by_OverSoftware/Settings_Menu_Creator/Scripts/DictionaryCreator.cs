using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryCreator : MonoBehaviour
{
    private Dictionary<string, int> dicOptions = new Dictionary<string, int>();

    public void CreateDictionary(string[] aMenuOptions){
        for (int i = 0; i < aMenuOptions.Length; i++){
            dicOptions.Add(aMenuOptions[i],i);
        }
    }

    public int GetIndexFromValue(string sValue){
        return dicOptions[sValue];
    }

}
