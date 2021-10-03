using System.Reflection;
using UnityEngine;
using HarmonyLib;
using System.Linq;
using System.Collections;
using Devdog.General2;
using EasyBuildSystem.Runtimes.Internal.Storage;

public static class HolographicSight {
    public const string ModID = "HolographicSight";
    public const string ModName = "Airways' HolographicSight";
    public const string ModShortName = "HolographicSight";
    public const string ModVersion = "1.0.0";
    
    public static void Main() {
        Log("==================================================");
        Log("Loading " + ModID + "...");
    }

    public static void Unload() {
    }
    
    public static void Log(string message)
    {
        Debug.Log("Mod::" + ModShortName + "::" + message);
    }
    
    public static ArrayList GetAllObjectsOnlyInScene(string name = null)
    {
        ArrayList objectsInScene = new ArrayList();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (!(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)) {
                if(name == null || go.name == name)
                    objectsInScene.Add(go);
            }
        }

        return objectsInScene;
    }
    
}

[HarmonyPatch(typeof(Player))]
[HarmonyPatch("Awake")]
public class HolographicSight_Player_Awake
{
    public static bool Prefix()
    {
        HolographicSight.Log("==================================================");
        HolographicSight.Log("HolographicSight patching...");
        
        // Hide ACOG frame and repurpose lense into a "projected" red-dot lense
        GameObject fpo = UnityEngine.GameObject.Find("First Person Objects");
        
        HolographicSight.Log("First Person Objects:");
        Debug.Log(fpo);
        
        HolographicSight.Log("Rifle:");
        ArrayList rifle = HolographicSight.GetAllObjectsOnlyInScene("Rifle");
        Debug.Log(rifle.Count);
        if(rifle.Count == 0) {
            HolographicSight.Log("ERROR: Rifle not found, can't patch!");
        } else {
            Debug.Log(rifle[0]);
            
            for(int i = 0; i <= rifle.Count; i++) {
                GameObject go = (GameObject)rifle[i];
                HolographicSight.Log("rifle[].ACOG_Sight_002 ["+i.ToString() + "]:");
                Debug.Log(go.transform.Find("ACOG_Sight_002"));
                
                HolographicSight.Log("rifle[].ACOG Sight ["+i.ToString() + "]:");
                Transform ts = go.transform.Find("ACOG Sight");
                Debug.Log(ts);
                if(ts != null) {
                    ts.gameObject.SetActive(false);
                }
            }
            
            //HolographicSight.Log("==================================================");
            //HolographicSight.Log("Activate rifle");
            //UnityEngine.GameObject.Find("Rifle").SetActive(true);
            //Debug.Log(fpo.transform.Find("ACOG_Sight_002"));
            fpo.transform.Find("ACOG_Sight_002").gameObject.SetActive(false);
            
            //GameObject lense = UnityEngine.GameObject.Find("Front_Lens");
            //GameObject dot = GameObject.Instantiate(UnityEngine.GameObject.Find("Front_Lens"));
            //dot.transform.position = lense.transform.position;
            //lense.SetActive(false);
            //dot.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //dot.name = "HolographicSight";
            //dot.transform.SetParent(lense.transform);
            
            //HolographicSight.Log("HolographicSight done!");
        }
        return true;
    }
}
/*
[HarmonyPatch(typeof(BuildStorage))]
[HarmonyPatch("SaveStorageFile")]
public class HolographicSight_BuildStorage_SaveStorageFile
{
    public static void Prefix()
    {
        HolographicSight.Log("==================================================");
        HolographicSight.Log("HolographicSight unpatching for save...");
        
        // Hide ACOG frame and repurpose lense into a "projected" red-dot lense
        GameObject fpo = UnityEngine.GameObject.Find("First Person Objects");
        
        HolographicSight.Log("First Person Objects:");
        Debug.Log(fpo);
        
        HolographicSight.Log("Rifle:");
        ArrayList rifle = HolographicSight.GetAllObjectsOnlyInScene("Rifle");
        Debug.Log(rifle.Count);
        if(rifle.Count == 0) {
            HolographicSight.Log("ERRPR: Rifle not found, can't patch!");
        } else {
            Debug.Log(rifle[0]);
            
            for(int i = 0; i <= rifle.Count; i++) {
                GameObject go = (GameObject)rifle[i];
                HolographicSight.Log("rifle[].ACOG_Sight_002 ["+i.ToString() + "]:");
                Debug.Log(go.transform.Find("ACOG_Sight_002"));
                
                HolographicSight.Log("rifle[].ACOG Sight ["+i.ToString() + "]:");
                Transform ts = go.transform.Find("ACOG Sight");
                Debug.Log(ts);
                if(ts != null) {
                    ts.gameObject.SetActive(true);
                }
            }
        }
    }
    
    public static void Postfix()
    {
        // Repatch after save
        HolographicSight_BuildStorage_LoadDataFile.Prefix();
    }
}
*/
