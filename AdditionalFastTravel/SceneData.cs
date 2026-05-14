using UnityEngine;

namespace ATLYSS_AdditionalFastTravel;

public static class SceneData
{
    public struct KnownSceneData
    {
        public KnownSceneData()
        {
            Path = "";
            Gotos = [];
            Spawnpoints = [];
        }
        
        public string Path;
        public List<string> Spawnpoints;
        public Dictionary<string, Vector3> Gotos;
    }

    public static string GetCurrentScene()
    {
        if (Player._mainPlayer == null)
            return "<null>";

        return Player._mainPlayer.gameObject.scene.path;
    }

    private static List<string> dungeonScenes = new List<string>()
    {
        "Assets/Scenes/map_dungeon00_sanctumCatacombs.unity",
        "Assets/Scenes/map_dungeon01_crescentGrove.unity"
    };

    public static bool IsInDungeon()
    {
        return dungeonScenes.Contains(GetCurrentScene());
    }

    public static Dictionary<string, KnownSceneData> GetAvailableScenes()
    {
        var scenes = new Dictionary<string, KnownSceneData>(StringComparer.InvariantCultureIgnoreCase);
        
        foreach (var pair in SceneDatas)
            scenes[pair.Key] = pair.Value;

        foreach (var extraMap in GameManager._current._cachedScriptableMapDatas)
        {
            if (!scenes.Values.Any(x => x.Path == extraMap.Value._subScene) && !scenes.ContainsKey(extraMap.Value._mapLockID))
            {
                scenes[extraMap.Value._mapLockID] = new KnownSceneData()
                {
                    Path = extraMap.Value._subScene,
                    Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase)
                };
            }
        }

        return scenes;
    }

    public static Dictionary<string, Vector3> GetCurrentSceneSpawnPoints()
    {
        if (Player._mainPlayer == null)
            return [];

        var spawnPoints = UnityEngine.Object.FindObjectsByType<SpawnPoint>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        var positions = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase);

        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.gameObject.scene != Player._mainPlayer.gameObject.scene)
                continue;

            var tag = spawnPoint._spawnPointTag;

            if (string.IsNullOrWhiteSpace(tag))
                continue;

            var name = char.ToUpper(tag[0]) + tag[1..];

            positions[name] = spawnPoint.transform.position;
        }

        return positions;
    }

    // Spawn points can be retrieved by searching for SpawnPoint MonoBehaviours in a scene
    // For the time being, only the first known spawn point is used for warps, or "spawnPoint" as a fallback if none are added
    private static readonly Dictionary<string, KnownSceneData> SceneDatas = new Dictionary<string, KnownSceneData>(StringComparer.InvariantCultureIgnoreCase)
    {
        ["SanctumCatacombs"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/map_dungeon00_sanctumCatacombs.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase),
            Spawnpoints = [
                "spawnPoint"
            ],
        },
        ["CrescentGrove"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/map_dungeon01_crescentGrove.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase),
            Spawnpoints = [
                "spawnPoint"
            ],
        },
        ["Sanctum"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_sanctum.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase)
            {
                ["Spawn"] = new Vector3(8.4f, 7.3f, -16.1f),
                ["Shop"] = new Vector3(200f, 11f, -110f),
                ["Enchanting"] = new Vector3(310f, 11f, -281f),
                ["Barracks"] = new Vector3(-160f, 29f, -600f),
                ["Lake"] = new Vector3(500f, 43f, 90f),
            },
            Spawnpoints = [
                "startPoint"
            ],
        },
        ["SanctumArena"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_sanctumArena.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase),
            Spawnpoints = [
                "spawnPoint"
            ],
        },
        ["OuterSanctum"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_outerSanctum.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase),
            Spawnpoints = [
                "spawnPoint",
                "terraceSpawn",
                "arcwoodSpawn",
                "tuulValleyPoint"
            ],
        },
        ["EffoldTerrace"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_effoldTerrace.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase)
            {
                ["CenterArea"] = new Vector3(20f, 18f, 45f),
            },
            Spawnpoints = [
                "startPoint"
            ]
        },
        ["ArcwoodPass"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_arcwoodPass.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase)
            {
                ["CatacombsEntrance"] = new Vector3(298f, 42f, 1819f),
                ["CatacombsMerchants"] = new Vector3(-54f, 1f, 1702f),
                ["CatacombsBadgeMerchants"] = new Vector3(136f, 81f, 2585f),
                ["FishingPond"] = new Vector3(453f, 48f, 1770f),
            },
            Spawnpoints = [
                "spawnPoint",
                "keepSpawn",
                "catacombWaypoint",
                "endDungeonSpawn",
                "fortSpawn"
            ]
        },
        ["CatacombsArena"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_catacombsArena.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase),
            Spawnpoints = [
                "spawnPoint"
            ],
        },
        ["CrescentRoad"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_crescentRoad.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase),
            Spawnpoints = [
                "spawnPoint",
                "keepSpawn",
                "gardenPoint"
            ]
        },
        ["CrescentKeep"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_crescentKeep.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase)
            {
                ["KeepEntrance"] = new Vector3(-150f, 55f, 235f),
                ["GroveDungeonLobby"] = new Vector3(-1265f, 225f, 575f),
                ["GroveDungeonBadgeMerchants"] = new Vector3(-1265f, 225f, 655f),
            },
            Spawnpoints = [
                "startPoint",
                "ckeepWaypoint1",
                "moonGateSpawn",
                "groveEntrance",
                "ckeepWaypoint2"
            ]
        },
        ["LuvoraGarden"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_luvoraGarden.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase),
            Spawnpoints = [
                "startPoint"
            ]
        },
        ["TuulValley"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_tuulValley.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase),
            Spawnpoints = [
                "spawnPoint",
                "enclavePoint"
            ]
        },
        ["TuulEnclave"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_tuulEnclave.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase),
            Spawnpoints = [
                "spawnPoint",
                "enclavePoint"
            ]
        },
        ["BularrFortress"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_bularFortress.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase)
            {
                ["AmmagonHut"] = new Vector3(-92f, 14f, -580f),
            },
            Spawnpoints = [
                "startPoint",
                "fortSpawn"
            ]
        },
        ["WallOfTheStars"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_wallOfTheStars.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase)
            {
                ["Merchant"] = new Vector3(-100f, 12f, -345f),
            },
            Spawnpoints = [
                "spawnPoint",
                "wallStarWaypoint",
                "trialSpawn"
            ]
        },
        ["TrialOfTheStars"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_trialOfTheStars.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase)
            {
                ["CheckpointHub"] = new Vector3(30f, 0f, 188f),
                ["Checkpoint1"] = new Vector3(60f, 448f, 185f),
                ["Checkpoint2"] = new Vector3(-10f, 878f, 184f),
                ["Checkpoint3"] = new Vector3(50f, 1130f, 181f),
                ["Summit"] = new Vector3(-9f, 1507f, 289f),
            },
            Spawnpoints = [
                "spawnPoint"
            ]
        },
        ["GateOfTheMoon"] = new KnownSceneData()
        {
            Path = "Assets/Scenes/00_zone_forest/_zone00_gateOfTheMoon.unity",
            Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase)
            {
                ["RedwoudEntrance"] = new Vector3(805f, 5f, 820f),
            },
            Spawnpoints = [
                "spawnPoint"
            ]
        },
        // TODO: Make accessible when area is updated
        // ["RedWoud"] = new KnownSceneData()
        // {
        //     Path = "Assets/Scenes/00_zone_forest/_zone00_redwoud.unity",
        //     Gotos = new Dictionary<string, Vector3>(StringComparer.InvariantCultureIgnoreCase),
        //     Spawnpoints = [
        //         "spawnPoint",
        //         "starWallSpawn",
        //         "treePoint",
        //         "autumnPoint"
        //     ]
        // },
    };
}