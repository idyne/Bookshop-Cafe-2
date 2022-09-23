using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
public class FateMenu : MonoBehaviour
{
    // Add a menu item named "Do Something" to Fate in the menu bar.
    [MenuItem("Fate/Delete Player Data")]
    static void DeletePlayerData()
    {
        string path = Application.persistentDataPath + "/PlayerData.fate";
        if (File.Exists(path))
            File.Delete(path);
    }

    // Validated menu item.
    // Add a menu item named "Log Selected Transform Name" to Fate in the menu bar.
    // We use a second function to validate the menu item
    // so it will only be enabled if we have a transform selected.
    /*[MenuItem("Fate/Log Selected Transform Name")]
    static void LogSelectedTransformName()
    {
        Debug.Log("Selected Transform is on " + Selection.activeTransform.gameObject.name + ".");
    }*/

    // Validate the menu item defined by the function above.
    // The menu item will be disabled if this function returns false.
    /*[MenuItem("Fate/Log Selected Transform Name", true)]
    static bool ValidateLogSelectedTransformName()
    {
        // Return false if no transform is selected.
        return Selection.activeTransform != null;
    }*/

    // Add a menu item named "Do Something with a Shortcut Key" to Fate in the menu bar
    // and give it a shortcut (ctrl-g on Windows, cmd-g on macOS).
    [MenuItem("Fate/Take Screenshot %&s")]
    static void TakeScreenshot()
    {
        string folderPath = Directory.GetCurrentDirectory() + "/Screenshots/";

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var screenshotName =
                                "Screenshot_" +
                                System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") +
                                ".png";
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName));
        Debug.Log(folderPath + screenshotName);
    }

    // Add a menu item called "Double Mass" to a Rigidbody's context menu.
    /*[MenuItem("CONTEXT/Rigidbody/Double Mass")]
    static void DoubleMass(MenuCommand command)
    {
        Rigidbody body = (Rigidbody)command.context;
        body.mass = body.mass * 2;
        Debug.Log("Doubled Rigidbody's Mass to " + body.mass + " from Context Menu.");
    }*/

    // Add a menu item to create custom GameObjects.
    // Priority 1 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarchy context menus.
    [MenuItem("GameObject/Fate/Object Pooler", false, 0)]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject variableForPrefab = Resources.Load("Fate Games/Prefabs/ObjectPooler", typeof(GameObject)) as GameObject;
        GameObject go = PrefabUtility.InstantiatePrefab(variableForPrefab) as GameObject;
        go.name = "Object Pooler";
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}