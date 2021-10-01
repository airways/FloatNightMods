/*
 * This script loads all of the other mod scripts that have registered Harmony patches.
 */
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using System.Linq;
using Devdog.Rucksack;
using Devdog.Rucksack.Items;
using Devdog.General2;
using EasyBuildSystem.Runtimes.Internal.Storage;

public static class ZAirwaysLoader {
    public const string ModID = "ZAirwaysLoader";
    public const string ModName = "Airways' Loader";
    public const string ModShortName = "ZAirwaysLoader";
    public const string ModVersion = "1.0.0";
    private static Harmony harmony;
    
    public static void Main() {
        Log("==================================================");
        Log("ZAirwaysLoader starting");
        Log("Note: If you do not see a section below titled 'ZAirwaysLoader Patched Methods' and a 'Loading finished!' message then it is very likely one of your scripts has an incorrect return type or parameter type, or has some other error that is preventing loading from finishing. We can't currently detect when this happens so if you have problems and do not see that section below, double check all your scripts!");
        
        harmony = new Harmony(ModID);
        Log("Got harmony instance");
        var assembly = Assembly.GetExecutingAssembly();
        Log("Got assembly instance");
        harmony.PatchAll(assembly);
        Log("Patching returned");
        
        Log("==================================================");
        
        if (!Harmony.HasAnyPatches(ModID)) {
            Log("ERROR: We are not marked as having patches!");
        } else {
            Log("Patches loaded!");
        }

        Log("==================================================");
        
        var dict = Harmony.VersionInfo(out var myVersion);
        Log("My version: " + myVersion);
        foreach (var entry in dict)
        {
            var id = entry.Key;
            var version = entry.Value;
            Log("Mod " + id + " uses Harmony version " + version);
        }
        
        Log("==================================================");
        Log("ZAirwaysLoader Patched Methods:");
        var myOriginalMethods = harmony.GetPatchedMethods().ToList();
        if(myOriginalMethods.Count == 0) {
            Log("==================================================");
            Log("SERIOUS WARNING: No patches found");
        } else {
            Log($"{myOriginalMethods.Count} patches found:");
            foreach (var method in myOriginalMethods) {
                Log(":"+method);
            }
        }
        
        Log("==================================================");
        Log("Loading finished!");
        Log("==================================================");
    }

    public static void Unload() {
        // Unload and unpatch everything before reloading the script
        harmony.UnpatchAll(ModID);
    }
    
    public static void Log(string message)
    {
        Debug.Log("Mod::" + ModShortName + "::" + message);
    }
    
}
