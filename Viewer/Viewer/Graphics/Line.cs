﻿using System.Windows;
using System.Windows.Media;
using LineGeometry = Viewer.Geometry.LineGeometry;

namespace Viewer.Graphics
{
    public sealed class Line : Shape
    {
        /// <summary>
        /// Initializes an instance of <see cref="Line"/> class.
        /// </summary>
        public Line(Point startPoint, Point endPoint)
        {
            Geometry = new LineGeometry(startPoint, endPoint);

            InvalidateVisual();
        }

        protected override void Render(DrawingContext drawingContext)
        {
            var lineGeometry = (LineGeometry) Geometry;

            drawingContext.DrawLine(Pen(), lineGeometry.StartPoint, lineGeometry.EndPoint);
        }

        public override T Write<T>(IWriter<T> writer)
        {
            var lineGeometry = (LineGeometry) Geometry;
            return writer.WriteLine(lineGeometry.StartPoint, lineGeometry.EndPoint, Color, LineStyle);
        }
    }
}