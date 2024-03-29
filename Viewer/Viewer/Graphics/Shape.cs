﻿using System.Windows.Media;
using Viewer.Geometry;

namespace Viewer.Graphics
{
    public abstract class Shape : DrawingVisual
    {
        protected bool m_isDirty { get; set; } = true;

        /// <summary>
        /// Caches the pen to improve performance.
        /// </summary>
        private Pen m_pen;

        private double m_thickness = 1d;
        private Color m_color = Colors.White;
        private DashStyle m_lineStyle = DashStyles.Solid;

        public ShapeGeometry Geometry { get; protected set; }

        public DashStyle LineStyle
        {
            get => m_lineStyle;
            set
            {
                m_lineStyle = value;
                m_isDirty = true;
            }
        }

        public Color Color
        {
            get => m_color;
            set
            {
                m_color = value;
                m_isDirty = true;
            }
        }

        public double Thickness
        {
            get => m_thickness;
            set
            {
                m_thickness = value;
                m_isDirty = true;
            }
        }

        public void InvalidateVisual()
        {
            if (!m_isDirty) return;

            using (DrawingContext drawingContext = RenderOpen()) Render(drawingContext);

            m_isDirty = false;
        }

        protected abstract void Render(DrawingContext drawingContext);

        protected Pen Pen()
        {
            if (m_isDirty || m_pen == null)
                m_pen = new Pen(Brush(), m_thickness) {DashStyle = DashStyle()}.AsFrozen();

            return m_pen;
        }

        protected Brush Brush()
        {
            return new SolidColorBrush(m_color).AsFrozen();
        }

        private DashStyle DashStyle()
        {
            return m_lineStyle.AsFrozen();
        }

        public override string ToString()
        {
            return $"Type: \t\t {GetType().Name} \n" +
                   $"Color: \t\t {m_color} \n" +
                   $"LineType: \t\t {m_lineStyle.AsString().ToUpper()} \n" +
                   Geometry;
        }

        public abstract T Write<T>(IWriter<T> writer);
    }
}