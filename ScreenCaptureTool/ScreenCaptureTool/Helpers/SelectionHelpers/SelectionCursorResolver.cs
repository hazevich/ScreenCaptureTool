using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScreenCaptureTool.Helpers.SelectionHelpers
{
    public class SelectionCursorResolver
    {
        public Cursor GetCursor(Point mousePoint, Rect movableOject)
        {
            if (mousePoint.IsOnCornerOf(movableOject))
            {
                var angle = mousePoint.GetCorner(movableOject);

                if (angle.Equals(movableOject.TopLeft))
                    return Cursors.SizeNWSE;
                else if (angle.Equals(movableOject.BottomLeft))
                    return Cursors.SizeNESW;
                else if (angle.Equals(movableOject.TopRight))
                    return Cursors.SizeNESW;
                else if (angle.Equals(movableOject.BottomRight))
                    return Cursors.SizeNWSE;
            }

            return mousePoint.IsInBoundsOf(movableOject) ? Cursors.SizeAll : Cursors.Arrow;
        }
    }
}
