using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestFantasyGame.Source.Tools;

namespace TestFantasyGame.Source;

public class ProceduralTerrainManager : TerrainManager{

    Random rand;
    PerlinNoise waterNoise;

    public ProceduralTerrainManager(int height, int width) : base(height, width){
        waterNoise = new PerlinNoise(width, height);
    }

    public override void LoadContent(ContentManager content){
        base.LoadContent(content);
        AddTerrainType("Grass");
        AddTileToTerrainType("Grass", "2d\\Tiles\\taiga", false);
        AddTileToTerrainType("Grass", "2d\\Tiles\\tundra", false);
        AddTileToTerrainType("Grass", "2d\\Tiles\\taidra", false);
        AddTerrainType("Sand");
        AddTileToTerrainType("Sand", "2d\\Tiles\\sand", false);
        AddTileToTerrainType("Sand", "2d\\Tiles\\sand orange", false);
        AddTileToTerrainType("Sand", "2d\\Tiles\\sand white", false);
        AddTerrainType("Water");
        AddTileToTerrainType("Water", "2d\\Tiles\\water", true);
        AddTerrainType("Shallows");
        AddTileToTerrainType("Shallows", "2d\\Tiles\\shallowwater", false);
        GenerateGrid();

    }

    public override void GenerateGrid()
    {
        rand = new Random();
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                
                var key = "Grass";
                if (waterNoise.pixels[i,j].R > 256/1.65 ){
                    key = "Water";
                } else if  (waterNoise.pixels[i,j].R > 256/1.7){
                    key = "Shallows";
                } else if (waterNoise.pixels[i,j].R > 256/1.8){
                    key = "Sand";
                }

                _tileGrid[i,j] = new ProceduralTerrainTile(new Vector2(i*_tileSize, j*_tileSize), key, this);
            }   
        }
    }

    public override Vector2 GetPlayerSpawnPosition(){
        var nonHazardTiles = new List<TerrainTile>();
        for (int i = 0; i < GridWidth; i ++){
            for(int j = 0; j < GridHeight; j++){
                if (!_tileGrid[i,j].TileTexture.IsHazard){
                    nonHazardTiles.Add(_tileGrid[i,j]);
                }
            }
        }
        var idx = rand.Next(nonHazardTiles.Count());
        return nonHazardTiles[idx]._position;
    }

    public class ProceduralTerrainTile : TerrainTile {

        
        protected Random rand;
        public ProceduralTerrainTile(Vector2 position, string terrainType, ProceduralTerrainManager terrainManager) : base(position, terrainType, terrainManager){
        }

        public override void ChangeTexture(string terrainTypeKey)
        {
            rand = new Random();

            List<TerrainTexture> terrainTiles = _manager._terrainTypes[terrainTypeKey];
            int idx = rand.Next(maxValue: terrainTiles.Count());
            TileTexture = terrainTiles[idx];
        }
    }

}
