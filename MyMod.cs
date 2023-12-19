using SonsSdk;
using TheForest;
using UnityEngine;

namespace MyMod;

public class MyMod : SonsMod
{
    public MyMod()
    {
        // Nothing here yet. We can initialize custom callbacks here.
    }

    protected override void OnInitializeMod()
    {
        // Do your early mod initialization which doesn't involve game or sdk references here
        Config.Init();
    }

    protected override void OnSdkInitialized()
    {
        // Do your mod initialization which involves game or sdk references here
        // This is for stuff like UI creation, event registration etc.
        MyModUi.Create();
        GlobalInput.RegisterKey(KeyCode.Z, CloneKelvin()); // Register the Z key and assign it to the CloneKelvin() Action.
    }

    protected override void OnGameStart()
    {
        // This function is executed when the player joins a game right before he gains control of the character.
    }

    public static Action CloneKelvin()
    {
        return () =>
        {
            DebugConsole.TryRunDynamicCommand("addcharacter", "robby 1", DebugConsole.Instance); // Spawn a clone of Robby (AKA Kelvin) using console command
        };
    }
}