using UnityEngine;
using UnityEngine.UI;

namespace ATC.Operator.MapView {
    // Make sure this component is on a GameObject with a RectTransform
    [RequireComponent(typeof(RectTransform))]
    public class SimpleUILineRenderer : Graphic {
        [Header("Line Properties")]
        [SerializeField]
        private Vector2 _point1 = Vector2.zero;
        public Vector2 Point1 {
            get { return _point1; }
            set {
                if (_point1 != value) {
                    _point1 = value;
                    SetVerticesDirty(); // Mark for redraw when points change
                }
            }
        }

        [SerializeField]
        private Vector2 _point2 = Vector2.zero;
        public Vector2 Point2 {
            get { return _point2; }
            set {
                if (_point2 != value) {
                    _point2 = value;
                    SetVerticesDirty(); // Mark for redraw when points change
                }
            }
        }

        [SerializeField]
        [Range(1, 10)] // You can adjust max thickness in Inspector
        private float _thickness = 2f;
        public float Thickness {
            get { return _thickness; }
            set {
                // Ensure thickness is positive
                if (value < 0.1f) value = 0.1f;

                if (Mathf.Abs(_thickness - value) > 0.001f) // Use epsilon for float comparison
                {
                    _thickness = value;
                    SetVerticesDirty(); // Mark for redraw when thickness changes
                }
            }
        }

        // Override the color property from Graphic to also mark for redraw
        public override Color color {
            get { return base.color; }
            set {
                if (base.color != value) {
                    base.color = value;
                    SetVerticesDirty(); // Mark for redraw when color changes
                }
            }
        }

        // This is called by Unity UI system when the mesh needs to be rebuilt
        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear(); // Clear any existing vertices and triangles

            // If the points are the same, draw nothing or a single point (depending on desired behavior)
            if (Point1 == Point2) {
                // Optionally, draw a small circle or nothing. For "less calculative", drawing nothing is best.
                return;
            }

            // Calculate direction and perpendicular vector for thickness
            Vector2 lineDirection = (Point2 - Point1).normalized;
            Vector2 perpendicular = new Vector2(-lineDirection.y, lineDirection.x) * (Thickness / 2f);

            // Calculate the four corners of the line segment (a quad)
            Vector2 p1_offset = Point1 + perpendicular;
            Vector2 p2_offset = Point1 - perpendicular;
            Vector2 p3_offset = Point2 + perpendicular;
            Vector2 p4_offset = Point2 - perpendicular;

            // Add the vertices to the VertexHelper
            // (0) p1_offset (top-left) --- (2) p3_offset (top-right)
            //     |                          |
            // (1) p2_offset (bottom-left) --- (3) p4_offset (bottom-right)
            vh.AddVert(p1_offset, color, new Vector2(0, 1)); // UVs are usually 0,0-1,1 for UI. Not critical for solid color.
            vh.AddVert(p2_offset, color, new Vector2(0, 0));
            vh.AddVert(p3_offset, color, new Vector2(1, 1));
            vh.AddVert(p4_offset, color, new Vector2(1, 0));

            // Add the two triangles that form the quad
            // Triangle 1: 0, 1, 2
            vh.AddTriangle(0, 1, 2);
            // Triangle 2: 2, 1, 3
            vh.AddTriangle(2, 1, 3);
        }

        // You might want to provide methods to easily set both points at once
        public void SetPoints(Vector2 p1, Vector2 p2) {
            if (_point1 != p1 || _point2 != p2) {
                _point1 = p1;
                _point2 = p2;
                SetVerticesDirty(); // Mark for redraw
            }
        }

        // If you need to set line color dynamically
        public void SetLineColor(Color newColor) {
            color = newColor; // This will automatically call SetVerticesDirty due to our override
        }

        // If you need to set line thickness dynamically
        public void SetLineThickness(float newThickness) {
            Thickness = newThickness; // This will automatically call SetVerticesDirty
        }
    }
}