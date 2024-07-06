using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestFantasyGame.Source.Tools;
using System;
using System.Net.Security;
using Microsoft.VisualBasic;


namespace TestFantasyGame.Source.World.Entities;

public class Enemy : BasicEntity {

    private Player currentPlayer;

    public Enemy(Vector2 position, Player player) : base(position) {
        currentPlayer = player;

        Speed = 1;
    }

    public override void LoadContent(ContentManager content) {
        base.LoadContent(content);

        AddAnim("Idle", "2d\\Orc\\Orc-Idle", 6,0.25f, new Vector2(3,3), true);
        AddAnim("Attack", "2d\\Orc\\Orc-Attack01", 6,0.1f, new Vector2(3,3), false, EnemyAttack);
        AddAnim("Death", "2d\\Orc\\Orc-Death", 4,0.25f, new Vector2(3,3), false);
        AddAnim("Walk", "2d\\Orc\\Orc-Walk", 8,0.25f, new Vector2(3,3), true);

    }

    public override void Death(){
        SwitchAnim("Death");
    }

    public bool EnemyAttack(){
        if(IsInHitRect(currentPlayer)){
            DealDamage(currentPlayer);
        }
        SwitchAnim("Idle");
        return false;
    }

    public override void BehaviorControls(){

        if (_currentAnim != "Death"){

            if (currentPlayer.pos.X < pos.X){
                Move.ExecuteCommand(DirectionEnum.LEFT);
            } else if (currentPlayer.pos.X > pos.X){
                Move.ExecuteCommand(DirectionEnum.RIGHT);
            }

            if(currentPlayer.pos.Y < pos.Y){
                Move.ExecuteCommand(DirectionEnum.UP);
            } else if (currentPlayer.pos.Y > pos.Y){
                Move.ExecuteCommand(DirectionEnum.DOWN);
            }

            if (IsCollidingDown(currentPlayer) || IsCollidingLeft(currentPlayer) ||
                IsCollidingRight(currentPlayer) || IsCollidingUp(currentPlayer)){
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
