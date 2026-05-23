namespace ATLYSS_AdditionalFastTravel;

public static class FastTravelActions
{
    public static void ListWarps()
    {
        var scenes = SceneData.GetAvailableScenes();
        string msg = string.Join("\n", scenes.Select(x => "  " + x.Name));

        if (string.IsNullOrWhiteSpace(msg))
            msg = "No maps available! Tell the mod developer about this!";
        
        Utils.ChatMsg("Available warps:\n" + msg);
    }

    public static void ListGotos()
    {
        var gotos = SceneData.GetAvailableGotos();
        string msg = string.Join("\n", gotos.Select(x => "  " + x.Name));

        if (string.IsNullOrWhiteSpace(msg))
            msg = "No gotos available! Tell the mod developer about this!";
        
        Utils.ChatMsg("Available goto points for this area:\n" + msg);
    }

    public static void WarpAndGoto(string sceneName, string locationName)
    {
        var warpResult = WarpToScene(sceneName, "");

        if (warpResult == WarpResult.AlreadyHere)
        {
            Utils.ChatMsg($"Already in the given map!");
            GoToLocation(locationName);
        }
        else if (warpResult == WarpResult.Warped)
        {
            Main.StoredGoto = locationName;
            Main.StoredGotoNotBefore = DateTime.Now + TimeSpan.FromSeconds(2);
        }
    }
    
    public enum WarpResult
    {
        Warped,
        AlreadyHere,
        MultipleMatches,
        NoMatches,
        InDungeon
    }

    public static WarpResult WarpToScene(string sceneName, string difficulty)
    {
        if (SceneData.IsInDungeon())
        {
            Utils.ChatMsg("<color=orange>Cannot use this command while in a dungeon.</color>");
            return WarpResult.InDungeon;
        }

        var scenes = SceneData.GetAvailableScenes();
        var matches = Utils.FindClosestMatch(sceneName, scenes, scene => scene.Name);

        if (matches.Count == 0)
        {
            Utils.ChatMsg($"<color=orange>ERROR</color>: No matches found for map {sceneName}. Use /warp to list all maps.");
            return WarpResult.NoMatches;
        }
        
        if (matches.Count > 1)
        {
            string msg = string.Join("\n", matches.Select(x => "  " + x.Name));
            Utils.ChatMsg($"<color=orange>ERROR</color>: Found multiple matches for map {sceneName}:\n{msg}\nTry to use a more specific name or use /warp to list all maps.");
            return WarpResult.MultipleMatches;
        }
                
        if (SceneData.GetCurrentScene() == matches[0].Path)
        {
            return WarpResult.AlreadyHere;
        }

        var zoneDifficulty = ZoneDifficulty.NORMAL;

        if (difficulty.Equals("HARD", StringComparison.InvariantCultureIgnoreCase))
            zoneDifficulty = ZoneDifficulty.HARD;

        else if (difficulty.Equals("EASY", StringComparison.InvariantCultureIgnoreCase))
            zoneDifficulty = ZoneDifficulty.HARD;
        
        var spawnTag = matches[0].Gotos.FirstOrDefault(x => x.SpawnPoint != null).SpawnPoint ?? "spawnPoint";
        
        Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._portalInteract, 1.0f);
        Player._mainPlayer.Cmd_SceneTransport(matches[0].Path, spawnTag, zoneDifficulty);

        return WarpResult.Warped;
    }
    
    public enum GotoResult
    {
        Warped,
        MultipleMatches,
        NoMatches,
        InDungeon,
        UnknownPosition
    }
    
    public static GotoResult GoToLocation(string locationName)
    {
        //Prevent usage of this command if player is in a dungeon to prevent cheesing
        if (SceneData.IsInDungeon())
        {
            Utils.ChatMsg("<color=orange>Cannot use this command while in a dungeon.</color>");
            return GotoResult.InDungeon;
        }

        var gotos = SceneData.GetAvailableGotos();
        var matches = Utils.FindClosestMatch(locationName, gotos, go => go.Name);
        
        if (matches.Count == 0)
        {
            Utils.ChatMsg($"<color=orange>ERROR</color>: No matches found for goto {locationName}. Use /goto to list all locations.");
            return GotoResult.NoMatches;
        }
        
        if (matches.Count > 1)
        {
            string msg = string.Join("\n", matches.Select(x => "  " + x.Name));
            Utils.ChatMsg($"<color=orange>ERROR</color>: Found multiple matches for goto {locationName}:\n{msg}\nTry to use a more specific name or use /goto to list all locations.");
            return GotoResult.MultipleMatches;
        }

        var position = matches[0].Position;
                
        if (position == null)
        {
            Utils.ChatMsg($"<color=orange>ERROR</color>: Couldn't retrieve position for {matches[0].Name}, please report this to the mod developer!");
            return GotoResult.UnknownPosition;
        }
        
        Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._warp, 1.0f);
                    
        Player._mainPlayer._pMove._playerController.enabled = false;
        Player._mainPlayer.transform.SetPositionAndRotation(position.Value, Player._mainPlayer.transform.rotation);
        CameraFunction._current.CameraReset();
        Player._mainPlayer._pMove._playerController.enabled = true;

        Utils.ChatMsg($"Moved to {matches[0].Name}.");
        return GotoResult.Warped;
    }
}