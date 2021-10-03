using System.Reflection;
using UnityEngine;
using HarmonyLib;
using System.Linq;
using Devdog.Rucksack;
using Devdog.Rucksack.Items;
using Devdog.General2;
using EasyBuildSystem.Runtimes.Internal.Storage;

public static class MeatIsFood {
    public const string ModID = "MeatIsFoodMod";
    public const string ModName = "Airways' MeatIsFood";
    public const string ModShortName = "MeatIsFood";
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

[HarmonyPatch(typeof(MeatItemInstance))]
[HarmonyPatch("DoUse")]
public class MeatIsFood_MeatItemInstance_DoUse
{
    public static bool Prefix(Result<ItemUsedResult> __result, Character character, ItemContext useContext)
    {
        MeatIsFood.Log("==================================================");
        MeatIsFood.Log("Meat is food");
        
        if (!PlayerPropertiesLogic.Instance.IsVendorOpen())
		{
			PlayerPropertiesLogic.Instance.RecoverPlayerHealth(0, 25);
			PlayerPropertiesLogic.Instance.EatingFoodAudioPlay();
			__result = new ItemUsedResult(useContext.useAmount, true, 0f);
		}
        
        // Stop original method from running
		return false;
    }
    
}
