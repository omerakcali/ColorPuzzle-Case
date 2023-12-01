using System.Collections;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class LevelDataImporter : ScriptableObject
{
    public int LevelId;
    
    private List<string> _loadedLines;
    
    [Button]
    public void ImportLevelData()
    {
        LoadCsvData();

        var firstLine = _loadedLines[0].Split(',');

        var columnCount = int.Parse(firstLine[0]);

        Vector2Int playerStartPosition = new Vector2Int(int.Parse(firstLine[1]), int.Parse(firstLine[2]));

        int playerStartColor = int.Parse(firstLine[3]);
        
        LevelData instance = ScriptableObject.CreateInstance<LevelData>();

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
        
        string path = $"Assets/Levels/Level_{LevelId}.asset";
        
        AssetDatabase.CreateAsset(instance,path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = instance;
    }

    private void LoadCsvData()
    {
        var csv =Resources.Load<TextAsset>("LevelData");
        var reader = new StringReader(csv.text);

        _loadedLines = new();

        while (reader.Peek() != -1)
        {
            _loadedLines.Add(reader.ReadLine());
        }
    }
}
