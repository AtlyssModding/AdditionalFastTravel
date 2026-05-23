using Mirror;
using UnityEngine;

namespace ATLYSS_AdditionalFastTravel;

public static class SceneData
{
    public struct Goto
    {
        public Goto()
        {
            Name = "";
        }

        public static Goto FromSpawnPoint(string name, string spawnPoint) => new()
        {
            Name = name,
            SpawnPoint = spawnPoint
        };

        public static Goto FromPosition(string name, Vector3 position) => new()
        {
            Name = name,
            Position = position
        };

        public string Name;
        public string? SpawnPoint;
        public Vector3? Position;
    }
    
    public struct KnownSceneData
    {
        public KnownSceneData()
        {
            Name = "";
            Path = "";
            Gotos = [];
        }

        public bool IsCustomMap;
        public string Name;
        public string Path;
        public List<Goto> Gotos; // Predefined for vanilla, generated from spawnpoints for custom maps
    }

    public static string LastNetworkSceneLoaded { get; internal set; } = "";

    public static string GetCurrentScene()
    {
        var scene = "<null>";

        if (NetworkServer.active)
            scene = Player._mainPlayer ? Player._mainPlayer.gameObject.scene.path : "<null>";

        else if (NetworkClient.active)
            scene = LastNetworkSceneLoaded;

        return scene;
    }

    public static bool IsInDungeon()
    {
        if (Player._mainPlayer == null)
            return false;
        
        return Player._mainPlayer._playerMapInstance._zoneType == ZoneType.Dungeon;
    }

    public static List<KnownSceneData> GetAvailableScenes()
    {
        var scenes = new List<KnownSceneData>(SceneDatas);

        foreach (var extraMap in GameManager._current._cachedScriptableMapDatas)
        {
            // Commands do not support spaces right now
            var sanitizedMapName = extraMap.Value._mapLockID.Replace(" ", "");
            
            if (!scenes.Any(x => x.Path == extraMap.Value._subScene) && !scenes.Any(x => x.Name == sanitizedMapName))
            {
                scenes.Add(new KnownSceneData()
                {
                    Name = sanitizedMapName,
                    Path = extraMap.Value._subScene,
                    IsCustomMap = true,
                    Gotos = []
                });
            }
        }

        return scenes;
    }

    public static List<Goto> GetAvailableGotos()
    {
        var scenes = SceneData.GetAvailableScenes();
        var currentScene = scenes.FirstOrDefault(x => x.Path == SceneData.GetCurrentScene());
        return SceneData.UpdateFromMapSpawnPoints(currentScene.Gotos ?? []);
    }

    public static List<Goto> UpdateFromMapSpawnPoints(List<Goto> gotos)
    {
        var spawnPoints = UnityEngine.Object.FindObjectsByType<SpawnPoint>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        var updatedGotos = new List<Goto>(gotos);
        
        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.gameObject.scene.path != GetCurrentScene())
                continue;

            var tag = spawnPoint._spawnPointTag;

            if (string.IsNullOrWhiteSpace(tag))
                continue;

            var existingGotoIndex = updatedGotos.FindIndex(x => x.SpawnPoint == tag);

            if (existingGotoIndex != -1)
            {
                updatedGotos[existingGotoIndex] = updatedGotos[existingGotoIndex] with
                {
                    Position = spawnPoint.transform.position
                };
            }
            else
            {
                var name = char.ToUpper(tag[0]) + tag[1..];
                updatedGotos.Add(Goto.FromPosition(name, spawnPoint.transform.position));
            }
        }

        return updatedGotos;
    }

    // Spawn points can be retrieved by searching for SpawnPoint MonoBehaviours in a scene
    // For the time being, only the first known spawn point is used for warps, or "spawnPoint" as a fallback if none are added
    private static readonly List<KnownSceneData> SceneDatas =
    [
        new KnownSceneData()
        {
            Name = "SanctumCatacombs",
            Path = "Assets/Scenes/map_dungeon00_sanctumCatacombs.unity",
            Gotos = [
                Goto.FromSpawnPoint("Spawn", "spawnPoint")
            ]
        },
        new KnownSceneData()
        {
            Name = "CrescentGrove",
            Path = "Assets/Scenes/map_dungeon01_crescentGrove.unity",
            Gotos = [
                Goto.FromSpawnPoint("Spawn", "spawnPoint")
            ],
        },
        new KnownSceneData()
        {
            Name = "Sanctum",
            Path = "Assets/Scenes/00_zone_forest/_zone00_sanctum.unity",
            Gotos =
            [
                Goto.FromSpawnPoint("Spawn", "startPoint"),
                Goto.FromPosition("Shop", new Vector3(200f, 11f, -110f)),
                Goto.FromPosition("Enchanting", new Vector3(310f, 11f, -281f)),
                Goto.FromPosition("Barracks", new Vector3(-160f, 29f, -600f)),
                Goto.FromPosition("Lake", new Vector3(500f, 43f, 90f)),
            ],
        },
        new KnownSceneData()
        {
            Name = "SanctumArena",
            Path = "Assets/Scenes/00_zone_forest/_zone00_sanctumArena.unity",
            Gotos = [
                Goto.FromSpawnPoint("Spawn", "spawnPoint")
            ],
        },
        new KnownSceneData()
        {
            Name = "OuterSanctum",
            Path = "Assets/Scenes/00_zone_forest/_zone00_outerSanctum.unity",
            Gotos = [
                Goto.FromSpawnPoint("Sanctum", "spawnPoint"),
                Goto.FromSpawnPoint("ArcwoodPass", "arcwoodSpawn"),
                Goto.FromSpawnPoint("EffoldTerrace", "terraceSpawn"),
                Goto.FromSpawnPoint("TuulValley", "tuulValleyPoint")
            ],
        },
        new KnownSceneData()
        {
            Name = "ArcwoodPass",
            Path = "Assets/Scenes/00_zone_forest/_zone00_arcwoodPass.unity",
            Gotos =
            [
                Goto.FromPosition("Chapel", new Vector3(-54f, 1f, 1702f)),
                Goto.FromPosition("CatacombsEntrance", new Vector3(298f, 42f, 1819f)),
                Goto.FromPosition("BadgeMerchants", new Vector3(136f, 81f, 2585f)),
                Goto.FromSpawnPoint("OuterSanctum", "spawnPoint"),
                Goto.FromSpawnPoint("Waypoint", "catacombWaypoint"),
                Goto.FromSpawnPoint("DungeonPortal", "fortSpawn"),
                Goto.FromSpawnPoint("DungeonEnd", "endDungeonSpawn"),
                Goto.FromSpawnPoint("CrescentRoad", "keepSpawn"),
            ],
        },
        new KnownSceneData()
        {
            Name = "CatacombsArena",
            Path = "Assets/Scenes/00_zone_forest/_zone00_catacombsArena.unity",
            Gotos = [
                Goto.FromSpawnPoint("Spawn", "spawnPoint")
            ],
        },
        new KnownSceneData()
        {
            Name = "EffoldTerrace",
            Path = "Assets/Scenes/00_zone_forest/_zone00_effoldTerrace.unity",
            Gotos =
            [
                Goto.FromSpawnPoint("Spawn", "startPoint"),
                Goto.FromPosition("CenterArea", new Vector3(20f, 18f, 45f)),
            ],
        },
        new KnownSceneData()
        {
            Name = "CrescentRoad",
            Path = "Assets/Scenes/00_zone_forest/_zone00_crescentRoad.unity",
            Gotos = 
            [
                Goto.FromSpawnPoint("ArcwoodPass", "spawnPoint"),
                Goto.FromSpawnPoint("CrescentKeep", "keepSpawn"),
                Goto.FromSpawnPoint("LuvoraGarden", "gardenPoint"),
            ]
        },
        new KnownSceneData()
        {
            Name = "CrescentKeep",
            Path = "Assets/Scenes/00_zone_forest/_zone00_crescentKeep.unity",
            Gotos =
            [
                Goto.FromPosition("KeepEntrance", new Vector3(-150f, 55f, 235f)),
                Goto.FromPosition("GroveLobby", new Vector3(-1265f, 225f, 575f)),
                Goto.FromSpawnPoint("Spawn", "startPoint"),
                Goto.FromSpawnPoint("Waypoint1", "ckeepWaypoint1"),
                Goto.FromSpawnPoint("Waypoint2", "ckeepWaypoint2"),
                Goto.FromSpawnPoint("GateOfTheMoon", "moonGateSpawn"),
                Goto.FromSpawnPoint("CrescentGrove", "groveEntrance"),
            ],
        },
        new KnownSceneData()
        {
            Name = "LuvoraGarden",
            Path = "Assets/Scenes/00_zone_forest/_zone00_luvoraGarden.unity",
            Gotos = [
                Goto.FromSpawnPoint("Spawn", "startPoint")
            ],
        },
        new KnownSceneData()
        {
            Name = "TuulValley",
            Path = "Assets/Scenes/00_zone_forest/_zone00_tuulValley.unity",
            Gotos = [
                Goto.FromSpawnPoint("Spawn", "spawnPoint"),
                Goto.FromSpawnPoint("TuulEnclave", "enclavePoint")
            ],
        },
        new KnownSceneData()
        {
            Name = "TuulEnclave",
            Path = "Assets/Scenes/00_zone_forest/_zone00_tuulEnclave.unity",
            Gotos =
            [
                Goto.FromSpawnPoint("Spawn", "spawnPoint"),
                Goto.FromSpawnPoint("TuulEnclave", "enclavePoint")
            ]
        },
        new KnownSceneData()
        {
            Name = "BularrFortress",
            Path = "Assets/Scenes/00_zone_forest/_zone00_bularFortress.unity",
            Gotos =
            [
                Goto.FromPosition("AmmagonHut", new Vector3(-92f, 14f, -580f)),
                Goto.FromSpawnPoint("Spawn", "startPoint"),
                Goto.FromSpawnPoint("Fort", "fortSpawn")
            ],
        },
        new KnownSceneData()
        {
            Name = "GateOfTheMoon",
            Path = "Assets/Scenes/00_zone_forest/_zone00_gateOfTheMoon.unity",
            Gotos =
            [
                Goto.FromPosition("RedwoudEntrance", new Vector3(805f, 5f, 820f)),
                Goto.FromSpawnPoint("Spawn", "spawnPoint")
            ]
        },
        new KnownSceneData()
        {
            Name = "WallOfTheStars",
            Path = "Assets/Scenes/00_zone_forest/_zone00_wallOfTheStars.unity",
            Gotos =
            [
                Goto.FromPosition("Merchant", new Vector3(-100f, 12f, -345f)),
                Goto.FromSpawnPoint("Spawn", "spawnPoint"),
                Goto.FromSpawnPoint("Waypoint", "wallStarWaypoint"),
                Goto.FromSpawnPoint("TrialOfTheStars", "trialSpawn")
            ]
        },
        new KnownSceneData()
        {
            Name = "TrialOfTheStars",
            Path = "Assets/Scenes/00_zone_forest/_zone00_trialOfTheStars.unity",
            Gotos =
            [
                Goto.FromPosition("CheckpointHub", new Vector3(30f, 0f, 188f)),
                Goto.FromPosition("Checkpoint1", new Vector3(60f, 448f, 185f)),
                Goto.FromPosition("Checkpoint2", new Vector3(-10f, 878f, 184f)),
                Goto.FromPosition("Checkpoint3", new Vector3(50f, 1130f, 181f)),
                Goto.FromPosition("Summit", new Vector3(-9f, 1507f, 289f)),
                Goto.FromSpawnPoint("Spawn", "spawnPoint")
            ],
        },
        // TODO: Make accessible when area is updated
        // new KnownSceneData()
        // {
        //     Name = "RedWoud",
        //     Path = "Assets/Scenes/00_zone_forest/_zone00_redwoud.unity",
        //     Gotos =
        //     [
        //         Goto.FromSpawnPoint("Spawn", "spawnPoint"),
        //         Goto.FromSpawnPoint("StarWall", "starWallSpawn"),
        //         Goto.FromSpawnPoint("Tree", "treePoint"),
        //         Goto.FromSpawnPoint("Autumn", "autumnPoint"),
        //     ]
        // }
    ];
}