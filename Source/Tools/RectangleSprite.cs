
/** 

The following code was stolen from -> https://stackoverflow.com/questions/13893959/how-to-draw-the-border-of-a-square
by stackoverflow user https://stackoverflow.com/users/1020861/neeko


this is only to be used for testing purposes




*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestFantasyGame.Source.Tools;
class RectangleSprite
{
    static Texture2D _pointTexture;
    public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
    {
        if (_pointTexture == null)
        {
            _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _pointTexture.SetData<Color>(new Color[]{Color.White});
        }

        spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
        spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
        spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
        spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
    }     
}
