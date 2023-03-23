using System.Drawing;
using UnityEngine;

public class VectorsRenderer : MonoBehaviour
{
   [SerializeField] private LineRenderer[] vectorsRenderer = new LineRenderer[3];
   private bool _shown = false;
   [SerializeField] private PlanetEditor _attachedEditor;

   public void ShowRenderer(bool shown)
   {
      _shown = shown;
      if (shown) return;
      foreach (var t in vectorsRenderer) t.SetPositions( new [] { Vector3.zero, Vector3.zero });
   }

   public void UpdateVectors()
   { 
      if (!_shown || !_attachedEditor.EditorBase.Shown) return;

      float[] values = 
      {
         _attachedEditor.EditorBase.CurrentPlanet.CurrentVelocity.x,
         _attachedEditor.EditorBase.CurrentPlanet.CurrentVelocity.y,
      };

      for (int i = 0; i < vectorsRenderer.Length; i++)
      {
         Vector2 start = _attachedEditor.EditorBase.CurrentPlanet.CurrentPosition;
         Vector2 end = new (values[0], values[1]);

         switch (i)
         {
            // x, clear y to get only x
            case 0:
               end -= new Vector2(0, end.y);
               break;
            // y, clear x to get only y
            case 1:
               end -= new Vector2(end.x, 0);
               break;
         }

         UpdateVector(i, start, end);
      }
   }
   
   void UpdateVector(int index, Vector2 start, Vector2 end)
   {
      vectorsRenderer[index].SetPosition(0, start);
      vectorsRenderer[index].SetPosition(1, start + end);

      if (Vector2.Distance(start, end) <= .5f)
      {
         // Clear arrow
         return;
      }
      if (Vector2.Distance(start, end) > 0.5f)
      {
         UniverseMath.GetLinearFunctionParams(start, end, out float a, out float b);
         DrawLine(start + (end - start)*.8f, a);
         
      }
   }

   void DrawLine(Vector3 point, float fa, float size = 10f)
   {
      float a = -1 / fa;
      float b = point.y - point.x * a;
      
      // quadratic function
      float qa = 1 + a;
      float qb = -2 * (point.x + b + point.y);
      float qc = Mathf.Pow(point.x, 2) + Mathf.Pow(b, 2) + Mathf.Pow(point.y, 2) + Mathf.Pow(size, 2) - 2 * point.y * b;

      float delta = Mathf.Pow(qb, 2) - 4 * qa * qc;
      float sd = Mathf.Sqrt(delta);

      float x1 = (-qb - sd) / (2 * qa);
      float y1 = a * x1 + b;
      
      float x2 = (-qb - sd) / (2 * qa);
      float y2 = a * x2 + b;
      
      LogPoints(x1, y1);
      LogPoints(x2, y2);
   }

   void LogPoints(float x, float y) => Debug.Log($"({x}, {y})");
   
}
