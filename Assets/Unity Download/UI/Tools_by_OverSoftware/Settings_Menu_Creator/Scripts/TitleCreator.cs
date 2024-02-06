using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleCreator : MonoBehaviour
{
    [Header("Title Options")]
    [SerializeField]
    string sTitle;
    [SerializeField]
    GameObject goTitle;
    [SerializeField]
    float fTitlePositionX = -480f;
    [SerializeField]
    float fTitlePositionY = 400f;
    [SerializeField]    
    GameObject goCanvasMenu;

    public void BuildTitle(){
        GameObject goNewTitle = Instantiate(goTitle, goCanvasMenu.transform);
        goNewTitle.transform.localPosition = new Vector2 (fTitlePositionX, fTitlePositionY);
        goNewTitle.GetComponent<Text>().text = sTitle;
    }
}
