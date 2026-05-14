using UnityEngine;
using UnityEngine.SceneManagement;

// /goto TELEPORT makes the player teleport to a given TELEPORT point in the area.

namespace ATLYSS_AdditionalFastTravel;

public static class GotoCommand
{
    public static void buildChatMessage(string inputMessage,bool useDividers=true)
    {
        string msg = (useDividers ? "---\n" : "") + inputMessage + (useDividers ? "---\n" : "");
        ChatBehaviour._current.New_ChatMessage(msg);
    }
        
    public static void ProcessCommand(string rawCommand)
    {
        string[] splitCmd = rawCommand.Split(' ');

        //Prevent usage of this command if player is in a dungeon to prevent cheesing
        if (SceneData.IsInDungeon())
        {
            buildChatMessage("<color=orange>Cannot use this command while in a dungeon.</color>",false);
            return;
        }

        var scenes = SceneData.GetAvailableScenes();
        var currentScene = scenes.FirstOrDefault(x => x.Value.Path == SceneData.GetCurrentScene());

        var knownGotos = currentScene.Value.Gotos ?? [];
        var spawnPointGotos = SceneData.GetCurrentSceneSpawnPoints();
        var allGotos = knownGotos.Concat(spawnPointGotos).ToDictionary(x => x.Key, x => x.Value, StringComparer.InvariantCultureIgnoreCase);
        
        if (splitCmd.Length == 2)
        {
            string targetLocation = splitCmd[1];

            if (allGotos.TryGetValue(targetLocation, out var targetVector))
            {
                Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._warp,1.0f);
                Player._mainPlayer.gameObject.SetActive(false);
                Player._mainPlayer.transform.position = targetVector;
                Player._mainPlayer.gameObject.SetActive(true);

                buildChatMessage("Moved to " + targetLocation + ".",false);
            }
            else
            {
                buildChatMessage("<color=orange>ERROR</color>: This goto point does not exist for this area.", false);
            }
        }
        else
        {
            if (allGotos.Count > 0)
            {
                string scenesString = string.Join("\n", allGotos.Keys.Select(x => "  " + x).ToArray());
                buildChatMessage("Available goto points for this area:\n" + scenesString + "\nUse <color=orange>/goto [POINT]</color> to goto the given point.");
            }
            else
            {
                buildChatMessage("ERROR: No goto points were found for the current area (" + SceneData.GetCurrentScene() + ")",false);
                buildChatMessage("(Tell the mod developer to add some goto points for this area!)",false);
            }
        }
    }
}