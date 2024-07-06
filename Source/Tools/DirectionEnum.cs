using Microsoft.Xna.Framework;

namespace TestFantasyGame.Source.Tools;


public enum DirectionEnum {
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NONE


}

public class DirVectorFactory {
    public static Vector2 GetDirVector(DirectionEnum dir){
        return dir switch {
            DirectionEnum.UP => new Vector2(0,-1),
            DirectionEnum.DOWN => new Vector2(0,1),
            DirectionEnum.LEFT => new Vector2(-1,0),
            DirectionEnum.RIGHT => new Vector2(1,0),
            DirectionEnum.NONE => Vector2.Zero,
            _ => Vector2.Zero
        };
    }
}
