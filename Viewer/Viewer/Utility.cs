﻿using System;
using System.Windows;
using System.Windows.Media;

namespace Viewer
{
    public static class Utility
    {
        private const string Dash = "dash";
        private const string Dot = "dot";
        private const string DashDot = "dashDot";
        private const string Solid = "solid";

        public static string AsString(this DashStyle dashStyle)
        {
            if (dashStyle == DashStyles.DashDot)
                return DashDot;
            if (dashStyle == DashStyles.Dash)
                return Dash;
            if (dashStyle == DashStyles.Dot)
                return Dot;
            if (dashStyle == DashStyles.Solid)
                return Solid;

            return Solid;
        }

        public static DashStyle AsDashStyle(this string dashStyle)
        {
            switch (dashStyle)
            {
                case DashDot:
                    return DashStyles.DashDot;
                case Dash:
                    return DashStyles.Dash;
                case Dot:
                    return DashStyles.Dot;
                case Solid:
                    return DashStyles.Solid;
                default:
                    return DashStyles.Solid;
            }
        }

        public static Brush RandomBrush(Random random)
        {
            Color color = Color.FromArgb(
                (byte)random.Next(130, 256),
                (byte)random.Next(130, 256),
                (byte)random.Next(130, 256),
                (byte)random.Next(130, 256));

            var brush = new SolidColorBrush(color);
            return brush.AsFrozen();

        }

        public static T AsFrozen<T>(this T freezable) where T : Freezable
        {
            if (freezable.CanFreeze)
                freezable.Freeze();

            return freezable;
        }
    }
}