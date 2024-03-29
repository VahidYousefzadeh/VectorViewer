﻿using System.Linq;
using System.Windows;
using System.Windows.Media;
using Viewer.Geometry;

namespace Viewer.Graphics
{
    public abstract class Polygon : Shape
    {
        private bool m_filled;

        public bool Filled
        {
            get => m_filled;
            set
            {
                m_filled = value;
                m_isDirty = true;
            }
        }

        protected Polygon(params Point[] corners)
        {
            Geometry = new PolygonGeometry(corners);

            InvalidateVisual();
        }

        protected override void Render(DrawingContext drawingContext)
        {
            if (drawingContext == null) return;

            var polygonGeometry = (PolygonGeometry) Geometry;
            Point[] corners = polygonGeometry.Edges.Select(o => o.StartPoint).ToArray();

            var streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(corners[0], true, true);
                geometryContext.PolyLineTo(
                    new PointCollection(corners.Skip(1)),
                    true,
                    true);

                drawingContext.DrawGeometry(m_filled ? Brush() : null, Pen(), streamGeometry);
            }
        }
    }
}