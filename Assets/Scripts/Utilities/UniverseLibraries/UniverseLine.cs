using UnityEngine;


public static class UniverseLine
{
    public static bool Intersect(Vector2 lineStart, Vector2 lineEnd, Vector2 spriteMin, Vector2 spriteMax)
    {
        float denominator = ((spriteMax.y - spriteMin.y) * (lineEnd.x - lineStart.x)) - ((spriteMax.x - spriteMin.x) * (lineEnd.y - lineStart.y));

        if (denominator == 0)
        {
            return false;
        }

        float ua = (((spriteMax.x - spriteMin.x) * (lineStart.y - spriteMin.y)) - ((spriteMax.y - spriteMin.y) * (lineStart.x - spriteMin.x))) / denominator;
        float ub = (((lineEnd.x - lineStart.x) * (lineStart.y - spriteMin.y)) - ((lineEnd.y - lineStart.y) * (lineStart.x - spriteMin.x))) / denominator;

        if (ua is >= 0 and <= 1 && ub is >= 0 and <= 1)
        {
            return true;
        }

        return false;
    }
    
    public static Vector2[] CalculateIntersectionPointsForTexture(Texture2D texture, Vector2 worldPointA, Vector2 worldPointB,
        Vector2 planetPos, float radius)
    {
        // here we are converting world points to texture points
        float slope = (worldPointB.y - worldPointA.y) / (worldPointB.x - worldPointA.x);
        float bias = worldPointA.y - slope * worldPointA.x;
        // 0 = ax^2 + bx + c -> quadratic formula
        float a = 1 + Mathf.Pow(slope, 2);
        float b = -2 * planetPos.x + 2 * slope * (worldPointA.y - slope * worldPointA.x) - 2 * planetPos.y * slope;
        float c = Mathf.Pow(planetPos.x, 2) + Mathf.Pow((worldPointA.y - slope * worldPointA.x), 2) +
            Mathf.Pow(planetPos.y, 2) - 2 *
            planetPos.y * (worldPointA.y - slope * worldPointA.x) - Mathf.Pow(radius, 2);

        float delta = Mathf.Pow(b, 2) - 4 * a * c;
        // line goes thru the circle
        if (delta > 0)
        {
            // world coordinates
            float x1 = (-b - Mathf.Sqrt(delta)) / (2 * a);
            float y1 = slope * x1 + bias;

            float x2 = (-b + Mathf.Sqrt(delta)) / (2 * a);
            float y2 = slope * x2 + bias;

            // texture coordinates
            float diffx = planetPos.x - radius, diffy = planetPos.y - radius;
            float scaler = texture.width / (radius * 2);
            
            float tx1 = (x1 - diffx) * scaler;
            float ty1 = (y1 - diffy) * scaler;
            float tx2 = (x2 - diffx) * scaler;
            float ty2 = (y2 - diffy) * scaler;
            
            return new[]
            {
                new Vector2(tx1, ty1),
                new Vector2(tx2, ty2)
            };
        }

        return null;
    }
}
