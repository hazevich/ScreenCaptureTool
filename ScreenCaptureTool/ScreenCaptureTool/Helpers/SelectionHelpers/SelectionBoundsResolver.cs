using System;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenCaptureTool.Helpers.SelectionHelpers
{
    public class SelectionBoundsResolver
    {
        public Rect GetSelectionBounds(Point mouseStartPos, Point mouseEndPos)
        {
            var selectionBounds = new Rect()
            {
                X = Math.Min(mouseStartPos.X, mouseEndPos.X),
                Y = Math.Min(mouseStartPos.Y, mouseEndPos.Y)
            };
            selectionBounds.Width = Math.Max(mouseStartPos.X, mouseEndPos.X) - selectionBounds.X;
            selectionBounds.Height = Math.Max(mouseStartPos.Y, mouseEndPos.Y) - selectionBounds.Y;

            return selectionBounds;
        }

        public Rect GetSelectionResizeBounds(Point mousePoint, Rect selectionRect)
        {
            var corner = mousePoint.GetCorner(selectionRect);

            if (corner.Equals(selectionRect.TopLeft))
                corner = selectionRect.BottomRight;
            else if (corner.Equals(selectionRect.BottomLeft))
                corner = selectionRect.TopRight;
            else if (corner.Equals(selectionRect.TopRight))
                corner = selectionRect.BottomLeft;
            else if (corner.Equals(selectionRect.BottomRight))
                corner = selectionRect.TopLeft;

            return GetSelectionBounds(corner, mousePoint);
        }

        public Rect GetSelectionMoveBounds(Point mouseStartPos, Point mouseEndPos, Rect selectionRect)
        {
            var selectionBounds = new Rect()
            {
                X = selectionRect.X - (mouseStartPos.X - mouseEndPos.X),
                Y = selectionRect.Y - (mouseStartPos.Y - mouseEndPos.Y),
                Width = selectionRect.Width,
                Height = selectionRect.Height
            };

            return selectionBounds;
        }

        public Rect ResolveSelectionOverflow(Rect selectionRect, Rect sourceRect)
        {
            if (selectionRect.X < sourceRect.X)
            {
                selectionRect.Width = selectionRect.Width - Math.Abs(selectionRect.X);
                selectionRect.X = sourceRect.X;
            }
            else if (selectionRect.Right > sourceRect.Width)
            {
                selectionRect.Width = selectionRect.Width - (selectionRect.Right - sourceRect.Width);
            }

            if (selectionRect.Y < sourceRect.Y)
            {
                selectionRect.Height = selectionRect.Height - Math.Abs(selectionRect.Y);
                selectionRect.Y = sourceRect.Y;
            }
            else if (selectionRect.Bottom > sourceRect.Height)
            {
                selectionRect.Height = selectionRect.Height - (selectionRect.Bottom - sourceRect.Height);
            }

            return selectionRect;
        }
    }
}
