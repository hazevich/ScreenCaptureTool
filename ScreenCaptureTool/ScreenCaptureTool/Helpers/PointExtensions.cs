using System;
using System.Windows;

namespace ScreenCaptureTool.Helpers
{
    public static class PointExtensions
    {
        private const int EdgeOverflow = 5;

        public static bool IsOnLeftEdgeOf(this Point mousePoint, Rect rect)
        {
            return mousePoint.X > rect.Left - EdgeOverflow && mousePoint.X < rect.Left + EdgeOverflow;
        }

        public static bool IsOnRightEdgeOf(this Point mousePoint, Rect rect)
        {
            return mousePoint.X > rect.Right - EdgeOverflow && mousePoint.X < rect.Right + EdgeOverflow;
        }

        public static bool IsOnTopEdgeOf(this Point mousePoint, Rect rect)
        {
            return mousePoint.Y > rect.Top - EdgeOverflow && mousePoint.Y < rect.Top + EdgeOverflow;
        }

        public static bool IsOnBottomEdgeOf(this Point mousePoint, Rect rect)
        {
            return mousePoint.Y > rect.Bottom - EdgeOverflow && mousePoint.Y < rect.Bottom + EdgeOverflow;
        }

        public static bool IsInBoundsOf(this Point source, Rect rect)
        {
            return (source.X > rect.Left - EdgeOverflow &&
                source.X < rect.Right + EdgeOverflow &&
                source.Y > rect.Top - EdgeOverflow &&
                source.Y < rect.Bottom + EdgeOverflow) &&
                !source.IsOnCornerOf(rect);
        }

        public static bool IsOnCornerOf(this Point mousePoint, Rect rect)
        {
            bool isOnEdgeByXAxes = mousePoint.IsOnLeftEdgeOf(rect) || mousePoint.IsOnRightEdgeOf(rect);
            bool isOnEdgeByYAxes = mousePoint.IsOnBottomEdgeOf(rect) || mousePoint.IsOnTopEdgeOf(rect);

            return isOnEdgeByXAxes && isOnEdgeByYAxes;
        }

        public static Point GetOppositeCorner(this Point sourcePoint, Rect sourceRect)
        {
            if (sourcePoint.Equals(sourceRect.TopLeft))
                return sourceRect.BottomRight;
            else if (sourcePoint.Equals(sourceRect.BottomLeft))
                return sourceRect.TopRight;
            else if (sourcePoint.Equals(sourceRect.TopRight))
                return sourceRect.BottomLeft;
            else if (sourcePoint.Equals(sourceRect.BottomRight))
                return sourceRect.TopLeft;

            throw new ArgumentException("sourcePoint is not located in any corner of sourceRect");
        }

        public static Point GetCorner(this Point sourcePoint, Rect sourceRect)
        {
            if (sourcePoint.IsOnTopEdgeOf(sourceRect) && sourcePoint.IsOnLeftEdgeOf(sourceRect))
                return sourceRect.TopLeft;
            else if (sourcePoint.IsOnBottomEdgeOf(sourceRect) && sourcePoint.IsOnLeftEdgeOf(sourceRect))
                return sourceRect.BottomLeft;
            else if (sourcePoint.IsOnTopEdgeOf(sourceRect) && sourcePoint.IsOnRightEdgeOf(sourceRect))
                return sourceRect.TopRight;
            else if (sourcePoint.IsOnBottomEdgeOf(sourceRect) && sourcePoint.IsOnRightEdgeOf(sourceRect))
                return sourceRect.BottomRight;

            throw new ArgumentException("sourcePoint is not located in any corner of sourceRect");
        }
    }
}
