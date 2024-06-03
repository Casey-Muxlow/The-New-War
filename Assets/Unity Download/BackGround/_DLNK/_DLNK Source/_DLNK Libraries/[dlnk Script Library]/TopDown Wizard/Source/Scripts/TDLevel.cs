using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TDLevel : MonoBehaviour
{
    [System.Serializable]
    public class ZoneGroup
    {
        [HideInInspector]
        public string GruName;
        public TDGroup ZGroup;
    }
    public List<ZoneGroup> ZoneGroups;

    [HideInInspector]
    public string zongrup = "New Zone Group";

    public void OnDo()
    {
        //Create gameobject for new zone
        GameObject groupGO = new GameObject(zongrup);

        //Set as child of this object
        groupGO.transform.parent = this.transform;

        //Add zone script
        groupGO.AddComponent<TDGroup>();

        //Check if list is initialized
        if (ZoneGroups == null)
            ZoneGroups = new List<ZoneGroup>();

        //Include in zone list
        ZoneGroup group = new ZoneGroup();
        group.ZGroup = groupGO.GetComponent<TDGroup>();
        ZoneGroups.Add(group);

        //Ejecutar "crear grupo de zonas"
        group.ZGroup.OnDo();
    }
    public void Update()
    {
        //Check if list is initialized
        if (ZoneGroups == null)
            ZoneGroups = new List<ZoneGroup>();
        else
        {
            foreach (ZoneGroup zong in ZoneGroups)
            {
                //Check if any group deleted and remove from list
                if (zong.ZGroup == null)
                    ZoneGroups.Remove(zong);
                //Set group element name
                zong.GruName = zong.ZGroup.name;
            }
        }
    }

}
