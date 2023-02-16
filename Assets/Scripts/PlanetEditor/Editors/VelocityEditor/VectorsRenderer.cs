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
   }
}
