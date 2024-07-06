using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TestFantasyGame.Source.Tools;
using TestFantasyGame.Source.World.Entities;
using System.Linq;
using System;


namespace TestFantasyGame.Source.World;

public class WorldObj {

    public TerrainManager terrainManager;
    public Player player;
    public List<BasicEntity> interactableEntities = new();


    private GraphicsDeviceManager _graphics;
    public WorldObj(GraphicsDeviceManager graphics){
        _graphics = graphics;
    }

    public virtual void LoadContent(ContentManager content){
        terrainManager = new ProceduralTerrainManager(130,200);
        terrainManager.LoadContent(content); 

        var spawnPos = terrainManager.GetPlayerSpawnPosition(); 

        player = new Player(new Vector2(spawnPos.X, spawnPos.Y));
        interactableEntities.Add(player);
        for (int i = 0; i < 10; i++){
            interactableEntities.Add(new Enemy(new Vector2(spawnPos.X-(20*i), spawnPos.Y-(20*i)), player));
        }

        interactableEntities.ForEach((BasicEntity entity) => entity.LoadContent(content));
    }

    public virtual void Update(GameTime gameTime){

        interactableEntities.ForEach((BasicEntity entity) => entity.Update(gameTime, interactableEntities));
        interactableEntities.Sort((BasicEntity one, BasicEntity two) => 
            two.Dead && !one.Dead ? 1 : 0
        );
        terrainManager.Update(this);
    }

    public virtual void Draw(SpriteBatch spriteBatch, FollowCamera camera){


        terrainManager.Draw(spriteBatch, camera);
        interactableEntities.ForEach((BasicEntity entity) => entity.Draw(spriteBatch));
    }

}
