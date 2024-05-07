using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class TDGroup : MonoBehaviour
{
    [System.Serializable]
    public class Zone
    {
        [HideInInspector]
        public string ZoName;
        public TDZone ZoneGO;
    }
    public List<Zone> Zones;

    [HideInInspector]
    public string zonname = "Zone Name";

    public void Update()
    {
        //Check if list is initialized
        if (Zones == null)
            Zones = new List<Zone>();
        //Check if any zone deleted and remove from list
        else
        {
            foreach (Zone zon in Zones)
            {
                // Remove deleted zones
                if (zon.ZoneGO == null)
                    Zones.Remove(zon);
                // Set name for list element
                zon.ZoName = zon.ZoneGO.name;
            }
        }
    }
    public void OnDo()
    {
        //Create child zone
        GameObject zoneG = new GameObject(zonname);

        //Set as child of zonegroup go
        zoneG.transform.parent = this.transform;

        // Add zone script
        zoneG.AddComponent<TDZone>();

        //Check if list is initialized
        if (Zones == null)
            Zones = new List<Zone>();

        //Add zone to group list
        Zone zonez = new Zone();
        zonez.ZoneGO = zoneG.GetComponent<TDZone>();
        Zones.Add(zonez);
        zonez.ZoneGO.OnDo();
    }
}
