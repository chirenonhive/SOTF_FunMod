using RedLoader;
using SonsSdk;
using SonsSdk.Attributes;
using TheForest;
using TheForest.Utils;
using UnityEngine;
using WindowsInput;

namespace MyMod;

public class MyMod : SonsMod
{
    private bool KeypadMinusKeyDown = false;
    private float KeypadMinusHoldTime = 0f;
    private bool AutoRun = false;
    public Rigidbody rb;

    public MyMod()
    {
        // Nothing here yet. We can initialize custom callbacks here.
        OnWorldUpdatedCallback = OnWorldUpdate;
    }

    protected override void OnInitializeMod()
    {
        // Do your early mod initialization which doesn't involve game or sdk references here
        Config.Init();
        RLog.Msg("INITIALIZED!");
    }

    protected override void OnSdkInitialized()
    {
        // Do your mod initialization which involves game or sdk references here
        // This is for stuff like UI creation, event registration etc.
        MyModUi.Create();
        RLog.Msg("SDK INITIALIZED!");
    }

    protected override void OnGameStart()
    {
        // This function is executed when the player joins a game right before he gains control of the character.
    }
    public void OnWorldUpdate()
    {
        if (Sons.Input.InputSystem.GetKeyDown(KeyCode.KeypadMultiply))
        {
            float MaxHP = LocalPlayer.Vitals.GetMaxHealth();
            LocalPlayer.Vitals.SetHealth(MaxHP);
            LocalPlayer.Vitals.SetRest(100);
            LocalPlayer.Vitals.SetHydration(100);
            LocalPlayer.Vitals.SetFullness(100);
            SonsTools.ShowMessage("HEALED!");
            RLog.Msg("HEALED!");
        }

        // Clone kelvin when slash key is held for 3 seconds
        if (Sons.Input.InputSystem.GetKeyDown(KeyCode.KeypadMinus))
        {
            KeypadMinusKeyDown = true;
        }
        if (Sons.Input.InputSystem.GetKeyUp(KeyCode.KeypadMinus))
        {
            KeypadMinusKeyDown = false;
        }
        if (KeypadMinusKeyDown)
        {
            KeypadMinusHoldTime += Time.deltaTime;
            if (KeypadMinusHoldTime >= 3f)
            {
                CloneKelvin();
                KeypadMinusKeyDown = false;
                KeypadMinusHoldTime = 0f;
            }
        }

        // Auto run
        if(Sons.Input.InputSystem.GetKeyDown(KeyCode.Numlock))
        {
            if (!AutoRun)
            {
                AutoRun = true;
                var inputSimulator = new InputSimulator();
                inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                SonsTools.ShowMessage("AUTORUN: ON!"); // Show a message in the bottom left corner of the screen
                RLog.Msg("AUTORUN: ON!");
            }
            else
            {
                AutoRun = false;
                var inputSimulator = new InputSimulator();
                inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                SonsTools.ShowMessage("AUTORUN: OFF!"); // Show a message in the bottom left corner of the screen
                RLog.Msg("AUTORUN: OFF!");
            }
        }

        if (AutoRun)
        {
            // If player presses any movement key, disable autorun
            if (Sons.Input.InputSystem.GetKeyDown(KeyCode.A) || Sons.Input.InputSystem.GetKeyDown(KeyCode.S) || Sons.Input.InputSystem.GetKeyDown(KeyCode.D) || Sons.Input.InputSystem.GetKeyUp(KeyCode.W))
            { 
                AutoRun = false;
                var inputSimulator = new InputSimulator();
                inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                SonsTools.ShowMessage("AUTORUN: OFF!"); // Show a message in the bottom left corner of the screen
                RLog.Msg("AUTORUN: OFF!");
            }
        }
    }

    [DebugCommand("clonekelvin")]
    public void CloneKelvin()
    {
        DebugConsole.TryRunDynamicCommand("addcharacter", "robby 1", DebugConsole.Instance); // Spawn a clone of Robby (AKA Kelvin) using console command
        SonsTools.ShowMessage("KELVIN CLONED!"); // Show a message in the bottom left corner of the screen
        RLog.Msg("KELVIN CLONED!");
    }
}