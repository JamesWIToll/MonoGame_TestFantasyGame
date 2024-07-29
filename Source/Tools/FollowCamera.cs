/**
The following Camera class is my version of a similar camera object to the one here -
https://community.monogame.net/t/simple-2d-camera/9135 
by user spool

*/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestFantasyGame.Source.World.Entities;

namespace TestFantasyGame.Source.Tools;

public class FollowCamera{
    public Vector2 Position {get; set;}
    public float Zoom {get {return _zoom;} set{_zoom = Math.Clamp(value, 0.3f, 2f);}}
    public Viewport _viewport;
    private BasicEntity _target;
    private float _zoom;
    public Matrix Transform {get; set;}
    public Rectangle ViewRect {get; set;}
    public FollowCamera(Viewport viewport, BasicEntity entityToFollow){
        _viewport = viewport;
        Position = new Vector2(_viewport.Bounds.Width/2, _viewport.Bounds.Height/2);
        _target = entityToFollow;
        Zoom = 1.0f;
    }

    public void Update(Viewport viewport){
        _viewport = viewport;

        Position = _target.pos;


        var keydownd = (Keyboard.GetState().IsKeyDown(Keys.Down));
        var keydownu = (Keyboard.GetState().IsKeyDown(Keys.Up));

        if (keydownd) { Zoom = Zoom - 0.05f; } else if (keydownu) {Zoom = Zoom + 0.05f;}

        Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0));
        Transform *= Matrix.CreateScale(_zoom);
        Transform *= Matrix.CreateTranslation(new Vector3(viewport.Bounds.Width * 0.5f, viewport.Bounds.Height * 0.5f, 0));

        var inverseTransform = Matrix.Invert(Transform);

        var topLeftPos = Vector2.Transform(Vector2.One, inverseTransform);
        var topRightPos = Vector2.Transform(new Vector2(viewport.Bounds.Width, 0), inverseTransform); 
        var botLeftPos = Vector2.Transform(new Vector2(0, _viewport.Bounds.Height), inverseTransform);

        ViewRect = new Rectangle((int)topLeftPos.X, (int)topLeftPos.Y, (int)topRightPos.X-(int)topLeftPos.X, (int)botLeftPos.Y-(int)topLeftPos.Y);
    }
}
