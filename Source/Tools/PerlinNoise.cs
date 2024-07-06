/**
The code below is my adapted version of perlin noise following this guide - https://rtouti.github.io/graphics/perlin-noise-algorithm
*/
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace TestFantasyGame.Source.Tools;
public class PerlinNoise {
    public int WidthPixels {get; private set;}
    public int HeightPixels {get; private set;}
    public Color[,] pixels;
    private List<int> permutation;
     

    public PerlinNoise(int width, int height){
        WidthPixels = width;
        HeightPixels = height;
        pixels = new Color[width,height];
        GenerateNoise();
    }
    public void GenerateNoise(){
        MakePermutation();
        for(int x = 0; x < WidthPixels; x++){
            for(int y = 0; y < HeightPixels; y++){
                float n = Noise2d(x*0.01f, y*0.01f);
                n+=1.0f;
                n/=2.0f;

                int c = (int)Math.Round(255f*n);
                pixels[x,y] = new Color(c,c,c);
            }
        }
    }

    public void MakePermutation(){

        permutation = new List<int>();
        for(var i = 0; i < 256; i++){
            permutation.Add(i);
        }

        permutation = permutation.OrderBy(x => Random.Shared.Next()).ToList();

        permutation.AddRange(permutation);
    }
    
    private static Vector2 GetConstVect (int v) => (v % 7) switch{ 0 => new Vector2(1f,1f), 
                                                                   1 => new Vector2(1f,0f), 
                                                                   2 => new Vector2(0f,0f),
                                                                   3 => new Vector2(0f,1f),
                                                                   4 => new Vector2(-1f,0),
                                                                   5 => new Vector2(-1f,-1f),
                                                                   6 => new Vector2(-1f,1f), 
                                                                   _ => new Vector2(1f, -1f)};


    private static float Lerp (float v1, float v2, float t) => v1 + t*(v2-v1);
    private static float Fade (float t) => 6*t*t*t*t*t - 15*t*t*t*t + 10*t*t*t;

    public float  Noise2d(float x, float y){
        int X = (int)Math.Floor(x) % 255;
        int Y = (int)Math.Floor(y) % 255;
        float xf = x - (float)Math.Floor(x);
        float yf = y - (float)Math.Floor(y);

        var topRight = new Vector2(xf-1f, yf-1f);
        var topLeft = new Vector2(xf, yf-1f);
        var bottomRight = new Vector2(xf-1f, yf);
        var bottomLeft = new Vector2(xf, yf);

        var valueTopRight = permutation[permutation[X+1]+Y+1];
	    var valueTopLeft = permutation[permutation[X]+Y+1];
	    var valueBottomRight = permutation[permutation[X+1]+Y];
	    var valueBottomLeft = permutation[permutation[X]+Y];


        var dotTopRight = Vector2.Dot(topRight, GetConstVect(valueTopRight));
        var dotTopLeft = Vector2.Dot(topLeft, GetConstVect(valueTopLeft));
        var dotBottomRight = Vector2.Dot(bottomRight, GetConstVect(valueBottomRight));
        var dotBottomLeft = Vector2.Dot(bottomLeft, GetConstVect(valueBottomLeft));

        var u = Fade(xf);
        var v = Fade(yf);

        return Lerp(Lerp(dotBottomLeft, dotTopLeft, v), Lerp(dotBottomRight, dotTopRight, v), u);        
    }



}
