using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TestFantasyGame.Source.Tools;


namespace TestFantasyGame.Source.World.Entities;

public abstract class BasicEntity{

    #region fields
    public List<BasicEntity> worldEntities = new();
    public Vector2 pos;
    public Vector2 size = Vector2.Zero;
    protected DirectionEnum lastDir = DirectionEnum.NONE;
    protected Dictionary<string, AnimationPlayer> _anims;
    protected string _currentAnim = "";
    private bool collidingR, collidingL, collidingU, collidingD = false;
    private Rectangle _spriteSourceRect = new Rectangle();
    private ContentManager _content;
    private int _hp = 6;

    #endregion

    #region properties
    public int MaxHP {get; set;} = 6;
    public int HP {get 
                    { 
                        return _hp;
                    }
                   protected set 
                   {
                        _hp = Math.Clamp(value, 0, MaxHP);
                        if (_hp == 0){
                            Death();
                        }
                   }}
    public int Stamina {get; protected set;} = 6;
    protected int MinDam {get; set;} = 0;
    protected int MaxDam {get; set;} = 6;
    protected float Speed {get; set;} = 2;
    public bool Dead {get { return _hp <= 0;}}

    public Command<bool, BasicEntity> Attack {get;  set;}
    public Command<bool, DirectionEnum> Move {get; set;}
    protected bool Flipped { get; set; } = false;
    public Rectangle CollisionRectange { get { return !Dead ? new Rectangle((int)pos.X-(_spriteSourceRect.Width/4 ), 
                                                                       (int)pos.Y-(_spriteSourceRect.Height/4 ), 
                                                                       _spriteSourceRect.Width/3,
                                                                       _spriteSourceRect.Height/2) : Rectangle.Empty;}

                                            set { _spriteSourceRect = value; } }
    
    public Rectangle HitRectangle { get { return new Rectangle(     CollisionRectange.Location.X-_spriteSourceRect.Width/4, 
                                                                       CollisionRectange.Location.Y, 
                                                                       _spriteSourceRect.Width,
                                                                       _spriteSourceRect.Height/2);}}

    #endregion

    #region Core Methods
    public BasicEntity(Vector2 position){
        pos = position;
        _anims = new();
        
        Attack = new Command<bool, BasicEntity>(DealDamage);
        Move = new Command<bool, DirectionEnum>(MoveDir);


    }

    public virtual void LoadContent(ContentManager content){
        _content= content;

    }

    public virtual void Update(GameTime gameTime, List<BasicEntity> entities){

        lastDir = DirectionEnum.NONE;

        collidingL = false; 
        collidingD = false; 
        collidingR = false; 
        collidingU = false;
        
        foreach(BasicEntity entity in entities){
            collidingL = collidingL ? collidingL : IsCollidingLeft(entity);
            collidingR = collidingR ? collidingR : IsCollidingRight(entity);
            collidingU = collidingU ? collidingU : IsCollidingUp(entity);
            collidingD = collidingD ? collidingD : IsCollidingDown(entity);
        }
        worldEntities = entities;

        BehaviorControls();  
        if (_anims.TryGetValue(_currentAnim, out AnimationPlayer animPlayer)){
            animPlayer.Update((float)gameTime.ElapsedGameTime.TotalSeconds); 
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch){
        //RectangleSprite.DrawRectangle(spriteBatch,CollisionRectange, Color.Blue, 2);
        //RectangleSprite.DrawRectangle(spriteBatch,HitRectangle, Color.Red, 2);
        if (_anims.TryGetValue(_currentAnim, out AnimationPlayer animPlayer)){
            animPlayer.Draw(spriteBatch, pos, Flipped);
            CollisionRectange = animPlayer.GetCurrSpriteRect();
        }
    }
    #endregion

    #region Animation Managment
    public virtual void AddAnim(string animName, string spriteSheetPath, int framesX, float frameTime, Vector2 scale, bool loop=false, Func<bool> callback=null){
        _anims.Add(animName, new AnimationPlayer(_content.Load<Texture2D>(spriteSheetPath), framesX, frameTime, scale, loop, callback));
    }

    protected virtual void SwitchAnim(string animKey){

        if (_currentAnim != animKey){
            if (_anims.TryGetValue(animKey, out AnimationPlayer currPlayer)) {
                if (_anims.TryGetValue(_currentAnim, out AnimationPlayer oldPlayer)){
                    oldPlayer.Stop();
                    oldPlayer.Reset();
                }
                currPlayer.Reset();
                currPlayer.Start();
                _currentAnim = animKey;
            }
        }
        
    }
    #endregion

    #region Attack & Damage
    public virtual void TakeDamage(int amt){
        HP = HP - amt;
    }

    protected virtual bool DealDamage(BasicEntity entity){
        Random rand = new Random();
        try{
            if (entity is not null){
                entity.TakeDamage(rand.Next(MinDam,MaxDam+1));
                return true;
            }
            throw new Exception();
            
        }catch {
            return false;
        }
    }
    #endregion

    #region Movement & Collision
    protected virtual bool MoveDir(DirectionEnum dir){
        var dirVector = DirVectorFactory.GetDirVector(dir);
        try{
            
            lastDir = dir;

            bool colliding = false;

            switch (lastDir) {
                case DirectionEnum.DOWN:
                    colliding = collidingD;
                    break;
                case DirectionEnum.UP:
                    colliding = collidingU;
                    break;
                case DirectionEnum.LEFT:
                    colliding = collidingL;
                    Flipped = true;
                    break;
                case DirectionEnum.RIGHT:
                    colliding = collidingR;
                    Flipped = false;
                    break;
                default:
                    throw new Exception();
            }

            if(!colliding){
                Vector2 vec = dirVector*Speed;
                vec.X = Math.Clamp(vec.X, -Speed, Speed);
                vec.Y = Math.Clamp(vec.Y, -Speed, Speed);
                pos += vec;
                return true;
            }

            return false;   

        }catch{
            return false;
        }
    }

    protected bool IsCollidingLeft(BasicEntity entity) {
        return  entity != this && 
                CollisionRectange.Intersects(entity.CollisionRectange) && 
                CollisionRectange.Location.X > entity.CollisionRectange.Location.X;
    }
    protected bool IsCollidingRight(BasicEntity entity) {
        return  entity != this && 
                CollisionRectange.Intersects(entity.CollisionRectange) && 
                CollisionRectange.Location.X < entity.CollisionRectange.Location.X;
    }

    protected bool IsCollidingDown(BasicEntity entity) {
        return  entity != this && 
                CollisionRectange.Intersects(entity.CollisionRectange) && 
                CollisionRectange.Location.Y < entity.CollisionRectange.Location.Y;
    }
    protected bool IsCollidingUp(BasicEntity entity) {
        return  entity != this && 
                CollisionRectange.Intersects(entity.CollisionRectange) && 
                CollisionRectange.Location.Y > entity.CollisionRectange.Location.Y;
    }

    protected bool IsInHitRect(BasicEntity entity){
        return entity != this && entity.CollisionRectange.Intersects(HitRectangle);
    }
    #endregion

    #region abstract behaviors
    public abstract void BehaviorControls();
    public abstract void Death();
    #endregion

}

