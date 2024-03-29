﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using Viewer.Graphics;

namespace Viewer.Writer
{
    public sealed class XmlWriter : IWriter<XElement>
    {
        private readonly IFormatProvider m_formatProvider;

        public XmlWriter(IFormatProvider formatProvider)
        {
            m_formatProvider = formatProvider;
        }

        public XElement WriteLine(Point startPoint, Point endPoint, Color color, DashStyle dashStyle)
        {
            return new XElement(
                "line",
                WriteLineGeometry(startPoint, endPoint),
                WriteColor(color),
                WriteDashStyle(dashStyle));
        }

        public XElement WriteCircle(Point center, double radius, Color color, DashStyle dashStyle, bool filled)
        {
            return new XElement(
                "circle",
                WriteCircleGeometry(center, radius),
                WriteFilled(filled),
                WriteColor(color),
                WriteDashStyle(dashStyle));
        }

        public XElement WriteTriangle(Point firstCorner, Point secondCorner, Point thirdCorner, Color color, DashStyle dashStyle, bool filled)
        {
            return new XElement(
                "triangle",
                WriteTriangleGeometry(firstCorner, secondCorner, thirdCorner),
                WriteFilled(filled),
                WriteColor(color),
                WriteDashStyle(dashStyle));
        }

        public XElement WriteRectangle(Point firstCorner, Point secondCorner, Color color, DashStyle dashStyle, bool filled)
        {
            return new XElement(
                "rectangle",
                WriteLineGeometry(firstCorner, secondCorner),
                WriteFilled(filled),
                WriteColor(color),
                WriteDashStyle(dashStyle));
        }

        public void WriteShapes(string fileName, Shape[] shapes)
        {
            var xml = new XElement("root", shapes.Select(o => o.Write(this)));
            xml.Save(fileName);
        }

        private XElement[] WriteLineGeometry(Point a, Point b)
        {
            return new[]
            {
                new XElement("a", $"{a.X.ToString(m_formatProvider)}; {a.Y.ToString(m_formatProvider)}"),
                new XElement("b", $"{b.X.ToString(m_formatProvider)}; {b.Y.ToString(m_formatProvider)}")
            };
        }

        private XElement[] WriteCircleGeometry(Point center, double radius)
        {
            return new[]
            {
                new XElement("center", $"{center.X.ToString(m_formatProvider)}; {center.Y.ToString(m_formatProvider)}"),
                new XElement("radius", $"{radius.ToString(m_formatProvider)}")
            };
        }

        private XElement[] WriteTriangleGeometry(Point a, Point b, Point c)
        {
            return new[]
            {
                new XElement("a", $"{a.X.ToString(m_formatProvider)}; {a.Y.ToString(m_formatProvider)}"),
                new XElement("b", $"{b.X.ToString(m_formatProvider)}; {b.Y.ToString(m_formatProvider)}"),
                new XElement("c", $"{c.X.ToString(m_formatProvider)}; {c.Y.ToString(m_formatProvider)}")
            };
        }

        private static XElement WriteColor(Color color)
        {
            return new XElement("color", $"{color.A}; {color.R}; {color.G}; {color.B}");
        }

        private static XElement WriteDashStyle(DashStyle dashStyle)
        {
            return new XElement("lineType", $"{dashStyle.AsString()}");
        }

        private static XElement WriteFilled(bool filled)
        {
            string boolString = filled.ToString().ToLower();
            return new XElement("filled", $"{boolString}");
        }
    }
}