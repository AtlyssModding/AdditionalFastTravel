namespace ATLYSS_AdditionalFastTravel;

//Note:
//In singleplayer, Player gameObjects are in the Scene of the current loaded level.
//But in multiplayer, Player gameObjects are put in the 01_RootScene Scene.

// /warp AREA makes the player warp to the default spawn point of the given AREA.

public static class WarpCommand
{
    public static void buildChatMessage(string inputMessage, bool useDividers = true)
    {
        string msg = ((useDividers ? "---\n" : "") + inputMessage + (useDividers ? "\n---" : ""));
        ChatBehaviour._current.New_ChatMessage(msg);
    }

    public static void ProcessCommand(string rawCommand)
    {
        string[] splitCmd = rawCommand.Split(' ');

        //Prevent usage of this command if player is in a dungeon to prevent cheesing
        if (SceneData.IsInDungeon())
        {
            buildChatMessage("<color=orange>Cannot use this command while in a dungeon.</color>", false);
            return;
        }

        var scenes = SceneData.GetAvailableScenes();

        if (splitCmd.Length == 2)
        {
            if (splitCmd[1] == "*")
            {
                string scenesString = string.Join("\n", scenes.Keys.ToArray());
                buildChatMessage("Available areas:\n" + scenesString + "\nUse <color=orange>/warp [Area name]</color> to warp to the given area.");
            }
            else
            {
                if (scenes.TryGetValue(splitCmd[1], out var data))
                {
                    //Check if the player is in the scene. If not, warp them to the default spawn point of the target area.
                    if (SceneData.GetCurrentScene() == data.Path)
                    {
                        buildChatMessage("<color=orange>You are already in the target area.</color>", false);
                    }
                    else
                    {
                        //TP the player to the default spawn point for the current loaded scene.
                        //Spawns seem to be either called spawnPoint, startPoint or respawnPoint.
                        Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._portalInteract, 1.0f);

                        var spawnTag = data.Spawnpoints.FirstOrDefault() ?? "spawnPoint";
                        
                        Player._mainPlayer.Cmd_SceneTransport(data.Path, spawnTag, ZoneDifficulty.NORMAL);
                    }
                }
                else
                {
                    buildChatMessage("<color=orange>ERROR</color>: Given area does not exist", false);
                }

                return;
            }
        }

        if (splitCmd.Length != 2)
        {
            buildChatMessage("Usage: <color=orange>/warp</color> [AREA] \n(Use <color=orange>/warp *</color> to view warpable areas)");
        }
    }
}