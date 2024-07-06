using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestFantasyGame.Source.World;
using TestFantasyGame.Source.Tools;
using System;

namespace TestFantasyGame.Source;

public class TestFantasyGameObj : Game
{

    public static GraphicsDevice graphicsDevice {get; set;}
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteBatch _uiBatch;

    private MouseState _mouseState;

    private WorldObj world;

    private bool _lockMouse = false;

    private FollowCamera camera;

    public SpriteFont uiFont;

    private SimpleFps _fpsCounter = new SimpleFps();


    public TestFantasyGameObj()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferHeight = 832;
        _graphics.PreferredBackBufferWidth = 1280;


        IsMouseVisible = true;

        world = new WorldObj(_graphics);

    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _uiBatch = new SpriteBatch(GraphicsDevice);

        world.LoadContent(Content);

        uiFont = Content.Load<SpriteFont>("2d\\Fonts\\UIFont");

    }

    protected override void Update(GameTime gameTime)
    {
        _fpsCounter.Update(gameTime);

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            _lockMouse = false;

        _mouseState = Mouse.GetState(); 

        if (camera is null ){
            camera = new FollowCamera(GraphicsDevice.Viewport, world.player);
        } else {
            camera.Update(GraphicsDevice.Viewport);
        }


        if(_lockMouse){
            
            
            var x = Math.Clamp(_mouseState.X, 0, _graphics.PreferredBackBufferWidth);
            var y = Math.Clamp(_mouseState.Y, 0, _graphics.PreferredBackBufferHeight);
            
            Mouse.SetPosition(x,y);
        
        }

        world.Update(gameTime);


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,samplerState: SamplerState.PointClamp, transformMatrix: camera.Transform);

        world.Draw(_spriteBatch, camera);
        RectangleSprite.DrawRectangle(_spriteBatch, camera.ViewRect, Color.Red, 10);


        _spriteBatch.End();

        _uiBatch.Begin();

            _uiBatch.DrawString(uiFont, "HP: " + world.player.HP, Vector2.One, Color.DarkRed );
            _fpsCounter.DrawFps(_uiBatch,uiFont, new Vector2(1, 20), Color.Black);


        _uiBatch.End();

        base.Draw(gameTime);
    }
}
