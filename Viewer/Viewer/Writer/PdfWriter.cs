﻿using System;
using System.Windows;
using System.Windows.Media;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace Viewer.Writer
{
    public sealed class PdfWriter : IWriter<PdfWriter>
    {
        private readonly PdfCanvas m_canvas;
        private readonly PdfDocument m_pdfDocument;

        private readonly double m_scale;
        private readonly double m_dx;
        private readonly double m_dy;

        public PdfWriter(string filename, Size pageSize, Rect drawingBounds)
        {
            double ymax = drawingBounds.Bottom;
            double ymin = drawingBounds.Top;
            double xmax = drawingBounds.Right;
            double xmin = drawingBounds.Left;

            m_scale = Math.Min(pageSize.Width / (xmax - xmin), pageSize.Height / (ymax - ymin));
            m_dx = -xmin;
            m_dy = -ymin;

            m_pdfDocument = new PdfDocument(new iText.Kernel.Pdf.PdfWriter(filename));
            m_canvas = new PdfCanvas(m_pdfDocument.AddNewPage(new PageSize((float) pageSize.Width, (float) pageSize.Height)));
        }

        public PdfWriter WriteLine(Point a, Point b, Color color, DashStyle dashStyle)
        {
            SetDashStyle(dashStyle);
            SetColor(color, false);
            WriteLineGeometry(a, b);

            return this;
        }

        public PdfWriter WriteCircle(Point center, double radius, Color color, DashStyle dashStyle, bool filled)
        {
            SetDashStyle(dashStyle);
            SetColor(color, filled);
            WriteCircleGeometry(center, radius, filled);

            return this;
        }

        public PdfWriter WriteTriangle(Point a, Point b, Point c, Color color, DashStyle dashStyle, bool filled)
        {
            SetDashStyle(dashStyle);
            SetColor(color, filled);
            WriteTriangleGeometry(a, b, c, filled);

            return this;
        }

        public PdfWriter WriteRectangle(Point a, Point b, Color color, DashStyle dashStyle, bool filled)
        {
            SetDashStyle(dashStyle);
            SetColor(color, filled);
            WriteRectangleGeometry(a, b, filled);

            return this;
        }

        public PdfWriter WriteShapes(Shape[] shapes)
        {
            foreach (Shape shape in shapes) shape.Write(this);

            return this;
        }

        public void Close()
        {
            m_pdfDocument.Close();
        }

        private void WriteLineGeometry(Point a, Point b)
        {
            m_canvas
                .MoveTo(Tx(a.X), Ty(a.Y))
                .LineTo(Tx(b.X), Ty(b.Y))
                .ClosePathStroke();
        }

        private void WriteCircleGeometry(Point center, double radius, bool filled)
        {
            m_canvas.Circle(Tx(center.X), Ty(center.Y), radius * m_scale);

            if (filled)
                m_canvas.ClosePathFillStroke();
            else
                m_canvas.ClosePathStroke();
        }

        private void WriteTriangleGeometry(Point a, Point b, Point c, bool filled)
        {
            m_canvas
                .MoveTo(Tx(a.X), Ty(a.Y))
                .LineTo(Tx(b.X), Ty(b.Y))
                .LineTo(Tx(c.X), Ty(c.Y))
                .LineTo(Tx(a.X), Ty(a.Y));

            if (filled)
                m_canvas.ClosePathFillStroke();
            else
                m_canvas.ClosePathStroke();
        }

        private void WriteRectangleGeometry(Point a, Point b, bool filled)
        {
            var c = new Point(b.X, a.Y);
            var d = new Point(a.X , b.Y);

            m_canvas
                .MoveTo(Tx(a.X), Ty(a.Y))
                .LineTo(Tx(c.X), Ty(c.Y))
                .LineTo(Tx(b.X), Ty(b.Y))
                .LineTo(Tx(d.X), Ty(d.Y))
                .LineTo(Tx(a.X), Ty(a.Y));

            if (filled)
                m_canvas.ClosePathFillStroke();
            else
                m_canvas.ClosePathStroke();
        }

        private void SetColor(Color color, bool filled)
        {
            m_canvas.SetStrokeColor(new DeviceRgb(color.R, color.G, color.B));

            if (filled) m_canvas.SetFillColor(new DeviceRgb(color.R, color.G, color.B));
        }

        private void SetDashStyle(DashStyle dashStyle)
        {
            float[] floats = new float[dashStyle.Dashes.Count];
            for (int i = 0; i < dashStyle.Dashes.Count; i++) floats[i] = (float) dashStyle.Dashes[i];

            m_canvas.SetLineDash(floats, 0f);
        }

        private double Tx(double x)
        {
            return (x + m_dx) * m_scale;
        }

        private double Ty(double y)
        {
            return (y + m_dy) * m_scale;
        }
    }
}