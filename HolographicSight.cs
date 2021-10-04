/**
 * Settings File: Steam/steamapps/common/Float Night/XuanYe_Data/HolographicSight.ini
 * Format:
 *      [Settings]
 *      Mode=ACOGSight|NoSight|HolographicSight|IronSight
 *
 * Instructions:
 *      Set one value from the list on the Mode line. Example:
 *      
 *      [Settings]
 *      Mode=NoSight
 *
 */
// System
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

// Unity
using UnityEngine;
using HarmonyLib;

// Game
using Devdog.General2;
using EasyBuildSystem.Runtimes.Internal.Storage;

public static class HolographicSight {
    public const string ModID = "HolographicSight";
    public const string ModName = "Airways' HolographicSight";
    public const string ModShortName = "HolographicSight";
    public const string ModVersion = "1.0.0";
    public static string Mode;
    
    public static void Main() {
        Log("==================================================");
        Log("Loading " + ModID + "...");
        Log("Config file: " + IniFile.path);
        
        // Read settings, set any defaults needed, then re-write file for player reference
        Mode = IniFile.Read("Settings", "Mode");
        if(String.IsNullOrEmpty(Mode)) {
            Mode = "NoSight";
            IniFile.Write("Settings", "!Mode", "This value can be one of these: ACOGSight|NoSight|HolographicSight|IronSight");
            IniFile.Write("Settings", "Mode", Mode);
        }
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
    
    private class IniFile : MonoBehaviour
    {
        public static string path = Application.dataPath + "/"  + ModID + ".ini";
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);
        [DllImport("kernel32")]
        public static extern int GetLastError();
     
        public static void Write(string Section, string Key, string Value)
        {
            if(WritePrivateProfileString(Section, Key, Value, path) == 0) {
                Log("IniFile.Write failed, error: " + GetLastError().ToString());
            } else {
                Log("IniFile.Write okay " + Section + "." + Key + "=" + Value);
            }
        }
        public static string Read(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            //int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            if(GetPrivateProfileString(Section, Key, "", temp, 255, path) == 0) {
                Log("IniFile.Read failed, error: " + GetLastError().ToString());
            } else {
                Log("IniFile.Read okay " + Section + "." + Key);
            }
            return temp.ToString();
     
        }
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
        HolographicSight.Log("Mode: " + HolographicSight.Mode);
        
        // Hide ACOG frame and repurpose lense into a "projected" red-dot lense
        GameObject fpo = UnityEngine.GameObject.Find("First Person Objects");
        
        HolographicSight.Log("First Person Objects:");
        Debug.Log(fpo);
        
        ArrayList rifle = HolographicSight.GetAllObjectsOnlyInScene("Rifle");
        
        HolographicSight.Log("Rifle:count="+rifle.Count.ToString());
        
        
        GameObject ACOGSight = null;
        GameObject IronSight = null;
        
        if(rifle.Count == 0) {
            HolographicSight.Log("***** ***** ERROR: Rifle not found, can't patch! ***** *****");
        } else {
            Debug.Log(rifle[0]);
            
            for(int i = 0; i <= rifle.Count; i++) {
                GameObject go = (GameObject)rifle[i];
                //HolographicSight.Log("rifle[].ACOG_Sight_002 ["+i.ToString() + "]:");
                //Debug.Log(go.transform.Find("ACOG_Sight_002"));
                
                //HolographicSight.Log("rifle[].ACOG Sight ["+i.ToString() + "]:");
                Transform ts1 = go.transform.Find("ACOG Sight");
                HolographicSight.Log("***** ACOG Sight = " + ts1?.gameObject?.ToString());
                if(ts1 != null) {
                    ACOGSight = ts1.gameObject;
                }
                
                Transform ts2 = go.transform.Find("M4A1_Sopmod_Iron_Sight");
                HolographicSight.Log("***** M4A1_Sopmod_Iron_Sight = " + ts2?.gameObject?.ToString());
                if(ts2 != null) {
                    IronSight = ts2.gameObject;
                }
                
                if(ACOGSight != null && IronSight != null) break;
            }
            
            HolographicSight.Log("Selecting Mode: " + HolographicSight.Mode);
            
            switch(HolographicSight.Mode)
            {
                case "NoSight":
                    ACOGSight.SetActive(false);
                    break;
                
                case "IronSight":
                    ACOGSight.SetActive(false);
                    IronSight.SetActive(true);
                    break;
                
                case "HolographicSight":
                    
                    ArrayList lense = HolographicSight.GetAllObjectsOnlyInScene("Front_Lens");
                    if(lense.Count == 0) {
                        HolographicSight.Log("ERROR: Could not find Front_Lens, cannot patch!");
                    } else {
                        GameObject lenseCopy = GameObject.Instantiate((GameObject)lense[0]);
                        GameObject dot = GameObject.Instantiate((GameObject)lense[0]);
                        lenseCopy.transform.position = ((GameObject)lense[0]).transform.position;
                        dot.transform.position = ((GameObject)lense[0]).transform.position;
                        
                        dot.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        dot.name = "HolographicSight";
                        dot.transform.SetParent(ACOGSight.transform.GetParent());
                        
                        ACOGSight.SetActive(false);
                        
                        HolographicSight.Log("HolographicSight done!");
                    }
                    break;
                    
                case "":
                    HolographicSight.Log("Mode: ACOG Sight (default)");
                    break;
                    
                case "ACOGSight":
                    HolographicSight.Log("Mode: ACOG Sight (specified)");
                    break;
                
                default:
                    HolographicSight.Log("ERROR: Invalid Mode: " + HolographicSight.Mode);
                    break;
            }
            
        }
        return true;
    }
}
