// #name Airways' CleanUp
// #author airways
// #desc Removes all the junk from the starting area. More mods at http://airways.mm.st/FloatNightMods/
// #proc_filter XuanYe.exe

using System.Reflection;
using UnityEngine;
//using Harmony;
using HarmonyLib;
using System.Linq;
using Devdog.General2;
using EasyBuildSystem.Runtimes.Internal.Storage;

public static class CleanUp {
    public const string ModID = "st.mm.airways.CleanUpMod";
    public const string ModName = "Airways' CleanUp";
    public const string ModShortName = "CleanUp";
    public const string ModVersion = "1.0.0";
    private static Harmony harmony;
    
    public static void Main() {
        Log("==================================================");
        Log("Loading...");
        
        CleanUp.harmony = new Harmony(ModID);
        
        var assembly = Assembly.GetExecutingAssembly();
        Debug.Log(assembly);
        CleanUp.harmony.PatchAll(assembly);
        
        Log("Patching done");

        if (!Harmony.HasAnyPatches(ModID)) {
            Log("ERROR: We are not marked as having patches!");
        }

        var dict = Harmony.VersionInfo(out var myVersion);
        Log("My version: " + myVersion);
        foreach (var entry in dict)
        {
            var id = entry.Key;
            var version = entry.Value;
            Log("Mod " + id + " uses Harmony version " + version);
        }
        
        var original = typeof(BuildStorage).GetMethod("LoadDataFile");

        /* List ALL patched methods
        Log("==================================================");
        Log("All patched methods:");
        var originalMethods = Harmony.GetAllPatchedMethods();
        foreach (var method in originalMethods) {
            Log(":"+method);
        }
        // */
        
        //* List OUR patched methods
        Log("==================================================");
        Log("This mod's patched methods:");
        var myOriginalMethods = harmony.GetPatchedMethods();
        foreach (var method in myOriginalMethods) {
            Log(":"+method);
        }
        // */
        
        // retrieve all patches
        var patches = Harmony.GetPatchInfo(original);
        if (patches is null) {
            Log("ERROR: No patches were loaded!");
        } else {
            Log("All owners: " + patches.Owners);
            foreach (var patch in patches.Prefixes)
            {
                Log("index: " + patch.index);
                Log("owner: " + patch.owner);
                Log("patch method: " + patch.PatchMethod);
                Log("priority: " + patch.priority);
                Log("before: " + patch.before);
                Log("after: " + patch.after);
            }
        }
        
        Log("==================================================");
    }

    public static void Unload() {
        // Unload and unpatch everything before reloading the script
        CleanUp.harmony.UnpatchAll(ModID);
    }
    
    public static void Log(string message)
    {
        Debug.Log("Mod::" + ModShortName + "::" + message);
    }
    
}

[HarmonyPatch(typeof(BuildStorage))]
[HarmonyPatch("LoadDataFile")]
public class CleanUpHook
{
    public static void Postfix()
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
    }
}


