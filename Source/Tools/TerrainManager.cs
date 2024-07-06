using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TestFantasyGame.Source.World;
using TestFantasyGame.Source.World.Entities;

namespace TestFantasyGame.Source.Tools;

public abstract class TerrainManager{


    public Dictionary<string, List<TerrainTexture>> _terrainTypes;

    protected TerrainTile[,] _tileGrid;

    protected int GridWidth {get; set;}
    protected int GridHeight {get; set;}

    protected float _tileSize = 64f;
    protected ContentManager _contentManager;

    public Vector2 origin;

    public TerrainManager (int height, int width){
        GridWidth = width;
        GridHeight = height;
        _tileGrid = new TerrainTile[width,height];
        _terrainTypes = new Dictionary<string, List<TerrainTexture>>();
        origin = Vector2.Zero;

    }

    public void AddTerrainType(string type){
        _terrainTypes.Add(type, new List<TerrainTexture>());
    }
    public void AddTerrainType(string type, List<TerrainTexture> terrainTypes){
        _terrainTypes.Add(type, terrainTypes);
    }

    public void AddTileToTerrainType(string type, string tileTexture, bool isHazard){
        _terrainTypes[type].Add(new TerrainTexture(_contentManager.Load<Texture2D>(tileTexture), isHazard));
    }


    public virtual void LoadContent(ContentManager content){
        _contentManager = content;
    }

    public void Update(WorldObj world){
        IterateTilesAndDo((TerrainTile tile) => {tile.Update(world); return true;});
    }
    public void Draw(SpriteBatch spriteBatch, FollowCamera camera){
        IterateTilesAndDo((TerrainTile tile) => {if(IsInCameraView(tile._position.X, tile._position.Y, camera)) {tile.Draw(spriteBatch);} return true;}); 
    }

    public bool IsInCameraView(float xPos, float yPos, FollowCamera camera){
        return  xPos <= camera.Position.X+camera.ViewRect.Width/2+_tileSize &&
                xPos >= camera.Position.X-camera.ViewRect.Width/2-_tileSize*2 &&
                yPos <= camera.Position.Y+camera.ViewRect.Height/2+_tileSize &&
                yPos >= camera.Position.Y-camera.ViewRect.Height/2-_tileSize*2;
    }

    public bool IterateTilesAndDo(Func<TerrainTile, bool> func){
        var success = true;
        for (int i=0; i < GridWidth; i++){
            for(int j=0; j < GridHeight; j++){
                var tile = _tileGrid[i,j];
                success = func(tile) && success ? true :  false;
            }
        }
        return success;
    }

    public abstract void GenerateGrid();    

    public abstract Vector2 GetPlayerSpawnPosition();

    public struct TerrainTexture {

        public TerrainTexture(Texture2D texture, bool isHazard){
            Texture = texture;
            IsHazard = isHazard;
        }
        public Texture2D Texture {get; init;}
        public bool IsHazard {get; init;}
    }


    public abstract class TerrainTile {
        public  readonly Vector2 _position = Vector2.Zero;
        public TerrainTexture TileTexture {get; set;}
        protected TerrainManager _manager;
        protected Rectangle collisionRect;

        public TerrainTile(Vector2 position, string terrainTypeKey, TerrainManager manager){
            _position = position + manager.origin;
            _manager = manager;
            collisionRect = new Rectangle((int)_position.X, (int)_position.Y, (int)manager._tileSize, (int)_manager._tileSize);
            ChangeTexture(terrainTypeKey);
        }

        public void Update(WorldObj world){
            world.interactableEntities.ForEach((BasicEntity entity) => {
                if (collisionRect.Contains(entity.pos) && TileTexture.IsHazard){
                    entity.TakeDamage(10);
                }
            });
        }  

        public void Draw(SpriteBatch spriteBatch){

            spriteBatch.Draw(TileTexture.Texture, 
                            _position,
                            null,
                            Color.White,
                            0f,
                            Vector2.Zero,
                            Vector2.One,
                            SpriteEffects.None,
                            0f);

            //RectangleSprite.DrawRectangle(spriteBatch, collisionRect, Color.Red, 10);
        }

        public abstract void ChangeTexture(string TerrainTypeKey);

    }

}
