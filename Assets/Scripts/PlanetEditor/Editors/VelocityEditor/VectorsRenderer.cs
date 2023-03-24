using System;
using UnityEngine;

public class VectorsRenderer : MonoBehaviour
{
   public static VectorsRenderer Instance;
   [SerializeField] private LineRenderer[] vectorsRenderer = new LineRenderer[3];
   private bool _shown = false;
   [SerializeField] private PlanetEditor _attachedEditor;

   void Awake() => Instance = this;
   
   public void ShowRenderer(bool shown)
   {
      _shown = shown;
      if (shown) return;
      for (int i = 0; i < vectorsRenderer.Length; i++)
      {
         vectorsRenderer[i].SetPositions(new[] {Vector3.zero, Vector3.zero});
         HideArrow(i);
      }
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
      if (Vector2.Distance(start, start + end) <= 0.2f) HideArrow(index);
      else SetArrowEnd(index, start, start + end);
   }

   void HideArrow(int index)
   {
      Transform arrow = vectorsRenderer[index].transform.GetChild(0);
      arrow.position = new Vector3(10000, 10000, arrow.position.z);
   }
   
   void SetArrowEnd(int index, Vector2 start, Vector2 end)
   { 
      Transform arrow = vectorsRenderer[index].transform.GetChild(0);
      arrow.position = new Vector3(end.x, end.y, arrow.position.z);
      var angle = MathF.Atan((end.y - start.y) / (end.x - start.x)) * 180 / MathF.PI - 90;
      arrow.rotation = Quaternion.Euler(0, 0, angle);
   }
}
