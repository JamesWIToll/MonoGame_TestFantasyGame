using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TestFantasyGame.Source.World.Entities;


namespace TestFantasyGame.Source.Tools;

public class AnimationPlayer{

    private readonly Texture2D _texture;
    private readonly List<Rectangle> _sourceRects = new();
    private readonly int _frames;
    private int _frame;
    private readonly float _frameTime;
    private float _frameTimeLeft;
    private bool _active = true;
    private bool _loop;
    private Func<bool> _callback;
    private Vector2 _scale;

    public AnimationPlayer(Texture2D texture, int framesX, float frameTime, Vector2 scale, bool loop = false, Func<bool> callback = null) {
        _texture = texture;
        _frames = framesX;
        _frameTime = frameTime;
        _frameTimeLeft = _frameTime;
        _scale = scale;
        _loop = loop;
        _callback = callback;

        var frameWidth = _texture.Width/_frames;
        var frameHeight = _texture.Height;

        for(int i=0; i < _frames; i++){
            _sourceRects.Add(new(i*frameWidth, 0, frameWidth, frameHeight));
        }
    }
    

    public void Stop(){ 
        _active = false; 

       
    }

    public void Start() { _active = true; }

    public void Reset() { 
        _frame = 0;
        _frameTimeLeft = _frameTime;
    }

    public void Update(float totalSeconds) {
        if (!_active) return;

        _frameTimeLeft -= totalSeconds;

        if(_frameTimeLeft <= 0){
            _frameTimeLeft += _frameTime;
            if (_frame >= _frames-1 && !_loop){
                Stop();
                //give callback a chance to restart the animation
                if (_callback is not null && _callback()){
                    _active = true;
                    Reset();
                }
            
            }else{ 
                _frame = (_frame+1) % _frames;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 pos, bool flipped){
        spriteBatch.Draw(
                            _texture,
                            pos,
                            _sourceRects[_frame],
                            Color.White,
                            0,
                            new Vector2(_sourceRects[_frame].Width/2, _sourceRects[_frame].Height/2),
                            _scale,
                            flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                            0.1f
                        );
    }

    public Rectangle GetCurrSpriteRect(){
        return _sourceRects[_frame];
    }
}
