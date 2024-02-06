using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPositionConfigurator : MonoBehaviour
{
    [Header("Options config.")]
    [SerializeField]
    float fFirstOptionPositionX = -480f;
    [SerializeField]
    float fFirstOptionPositionY = 350;
    [SerializeField]
    float fOptionSpaceY = 90f;

   [Header("Suboptions Panel config.")]
    [SerializeField]
    GameObject goPanel;
    [SerializeField]
    float fPanelPositionX = 750f;
    [SerializeField]
    float fPanelPositionY = -300f;

    public Vector2 GetPositionForIndex(int iIndex){
        return new Vector2(fFirstOptionPositionX, fFirstOptionPositionY - ((iIndex+1)*fOptionSpaceY));
    }

    public float GetSpaceBetweenOptions(){
        return fOptionSpaceY;
    }

    public void InstantiatePanelForSuboptionsOfOptionI(Transform tOptionParent, int iIndex){
        GameObject goNewPanel = Instantiate(goPanel, tOptionParent);
//            goNewPanel.transform.localPosition = new Vector2(fPanelPositionX, fPanelPositionY + (i*fOptionSpaceY));
            goNewPanel.transform.localPosition = new Vector2(fPanelPositionX, 
            fPanelPositionY + (iIndex*GetSpaceBetweenOptions()));

            goNewPanel.SetActive(false);

    }
}
