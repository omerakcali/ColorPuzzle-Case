using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class LevelDataImporter : ScriptableObject
{
    private List<string> _loadedLines;

    [SerializeField] private List<TextAsset> LevelTextAssets;

    [Button]
    private void CollectLevelDataAssets()
    {
        var levels = Resources.LoadAll<TextAsset>("Levels");
        LevelTextAssets = levels.ToList();
    }

    [Button]
    private void ImportLevels()
    {
        for (var i = 0; i < LevelTextAssets.Count; i++)
        {
            var csv = LevelTextAssets[i];
            LoadCsvData(csv);
            ImportLevelData(i+1);
        }
    }
    
    private void ImportLevelData(int levelIndex)
    {
        var firstLine = _loadedLines[0].Split(',');

        var columnCount = firstLine.Length;

        Vector2Int playerStartPosition = new Vector2Int(int.Parse(firstLine[1]), int.Parse(firstLine[2]));

        int playerStartColor = int.Parse(firstLine[3]);
        
        
        string path = $"Assets/Levels/Level_{levelIndex}.asset";

        var loadedAsset = AssetDatabase.LoadAssetAtPath<LevelData>(path);
        
        LevelData instance = loadedAsset != null ? loadedAsset :CreateInstance<LevelData>();
        if(loadedAsset == null) 
            AssetDatabase.CreateAsset(instance,path);

        instance.ColumnCount = columnCount;
        instance.PlayerPosition = playerStartPosition;
        instance.PlayerStartColor = playerStartColor;
        instance.Tiles = new();
        for (int i = _loadedLines.Count - 1; i >= 1; i--)
        {
            var line = _loadedLines[i];
            var lineElements = line.Split(',');
            for (int j = 0; j < lineElements.Length; j++)
            {
                var element = lineElements[j];
                var tileData = new LevelTileData();
                tileData.TilePickupColor = -1;
                switch (element)
                {
                    case "w":
                        tileData.TileType = TileType.Wall;
                        break;
                    case "e":
                        tileData.TileType = TileType.Empty;
                        break;
                    case "":
                        tileData.TileType = TileType.Floor;
                        break;
                    default:
                        tileData.TileType = TileType.Floor;
                        tileData.TilePickupColor = int.Parse(element);
                        break;
                }
                instance.Tiles.Add(tileData);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = instance;
    }

    private void LoadCsvData(TextAsset csv)
    {
        var reader = new StringReader(csv.text);

        _loadedLines = new();

        while (reader.Peek() != -1)
        {
            _loadedLines.Add(reader.ReadLine());
        }
    }
}
