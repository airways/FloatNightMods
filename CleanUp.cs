using System.Reflection;
using UnityEngine;
using HarmonyLib;
using System.Linq;
using Devdog.General2;
using EasyBuildSystem.Runtimes.Internal.Storage;

public static class CleanUp {
    public const string ModID = "CleanUpMod";
    public const string ModName = "Airways' CleanUp";
    public const string ModShortName = "CleanUp";
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
    
}

[HarmonyPatch(typeof(Player))]
[HarmonyPatch("Awake")]
public class CleanUp_Player_Awake
{
    public static bool Prefix()
    {
        CleanUp.Log("==================================================");
        CleanUp.Log("Cleanup starting area junk");
        
        // Hide junk near spawn point
        UnityEngine.GameObject.Find("Crate_Simple_02").SetActive(false);
        UnityEngine.GameObject.Find("Barrel_01").SetActive(false);
        UnityEngine.GameObject.Find("Barrel_02").SetActive(false);
        UnityEngine.GameObject.Find("P_Char_Simple_01").SetActive(false);
        UnityEngine.GameObject.Find("Roof_Border_Line_03 (1)").SetActive(false);
        UnityEngine.GameObject.Find("Roof_Border_Line_03").SetActive(false);
        UnityEngine.GameObject.Find("Pipe_Bent_01").SetActive(false);
        
        // Change price of a shop item
        //UnityEngine.Object.FindObjectOfType<Devdog.Rucksack.Vendors.ItemVendorCreator>()._itemDefs[0].buyPrice[0].amount = 1
        
        return true;
    }
}
