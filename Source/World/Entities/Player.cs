using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestFantasyGame.Source.Tools;


namespace TestFantasyGame.Source.World.Entities;

public class Player : BasicEntity {

    public Player(Vector2 position) : base(position) {
        MaxHP = 20;
        HP = 20;
        Speed = 10;
    }

    public override void LoadContent(ContentManager content) {
        base.LoadContent(content);

        AddAnim("Idle", "2d\\Soldier\\Soldier-Idle", 6,0.25f, new Vector2(3,3), true);
        AddAnim("Attack", "2d\\Soldier\\Soldier-Attack01", 6,0.1f, new Vector2(3,3), false, PlayerAttack);
        AddAnim("Death", "2d\\Soldier\\Soldier-Death", 4,0.25f, new Vector2(3,3), false);
        AddAnim("Walk", "2d\\Soldier\\Soldier-Walk", 8,0.25f, new Vector2(3,3), true);

    }

    public override void Death(){
        SwitchAnim("Death");
    }

    public bool PlayerAttack(){
        foreach(BasicEntity entity in worldEntities){
            if (IsInHitRect(entity)){
                DealDamage(entity);
            }
        }
        SwitchAnim("Idle");
        return false;
    }

    public override void BehaviorControls(){
        KeyboardState keyState = Keyboard.GetState();


        if (_currentAnim != "Death"){

            if (keyState.IsKeyDown(Keys.W)){
                Move.ExecuteCommand(DirectionEnum.UP);
            }
            if (keyState.IsKeyDown(Keys.S)){
                Move.ExecuteCommand(DirectionEnum.DOWN);
            }
            if (keyState.IsKeyDown(Keys.D)){
                Move.ExecuteCommand(DirectionEnum.RIGHT);
            }
            if (keyState.IsKeyDown(Keys.A)){
                Move.ExecuteCommand(DirectionEnum.LEFT);
            }

            if (keyState.IsKeyDown(Keys.J)){
                SwitchAnim("Attack");
            }

            if (_currentAnim != "Attack"){
                if (lastDir == DirectionEnum.NONE){
                    SwitchAnim("Idle");
                } else {
                    SwitchAnim("Walk");
                }
            }
            
        }
        

    }


    

}
