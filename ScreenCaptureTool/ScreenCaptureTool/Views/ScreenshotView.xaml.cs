using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ScreenCaptureTool.Helpers;
using ScreenCaptureTool.Helpers.SelectionHelpers;
using System.Windows.Data;
using ScreenCaptureTool.DataAccess;

namespace ScreenCaptureTool.Views
{
    /// <summary>
    /// Interaction logic for ScreenshotView.xaml
    /// </summary>
    public partial class ScreenshotView : WindowBase
    {
        private Point _selectionStartPos;
        private Point _selectionEndPos;
        private SelectionBoundsResolver _selectionBoundsResolver = new SelectionBoundsResolver();
        private SelectionCursorResolver _selectionCursorResolver = new SelectionCursorResolver();

        private bool _canSelect;
        private bool _canMoveSelection;
        private bool _canResizeSelection;
        private bool _areaSelected;

        public ScreenshotView()
        {
            InitializeComponent();



            SetBinding(Window.WidthProperty, new Binding("ScreenBounds.Width") { Source = DataContext, Mode = BindingMode.TwoWay });
            SetBinding(Window.HeightProperty, new Binding("ScreenBounds.Height") { Source = DataContext, Mode = BindingMode.TwoWay });


            CompositionTarget.Rendering += OnCompositionTargetRendering;
        }

        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            ResolveActionStackPanelLocation();
        }

        public void TakeScreenshot(bool screenCapture)
        {
            if (Visibility == Visibility.Visible)
                return;

            Show();
            ResolveWindowBounds();
        }

        private void OnMainCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Visibility == Visibility.Collapsed || e.ChangedButton != MouseButton.Left)
                return;

            _selectionStartPos = e.GetPosition(MainCanvas);

            _canSelect = !_selectionStartPos.IsInBoundsOf(SelectionOverlay.Rect);
            _canResizeSelection = _areaSelected && _selectionStartPos.IsOnCornerOf(SelectionOverlay.Rect);

            if (_canResizeSelection)
            {
                _selectionStartPos = _selectionStartPos.GetCorner(SelectionOverlay.Rect).GetOppositeCorner(SelectionOverlay.Rect);
            }

            _canMoveSelection = _areaSelected && _selectionStartPos.IsInBoundsOf(SelectionOverlay.Rect);
        }

        private void OnMainCanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (Visibility != Visibility.Visible)
                return;


            Mouse.OverrideCursor = _selectionCursorResolver.GetCursor(e.GetPosition(MainCanvas), SelectionOverlay.Rect);

            if (e.LeftButton == MouseButtonState.Released || WindowContextMenu.IsOpen)
                return;

            _selectionEndPos = e.GetPosition(MainCanvas);

            ActionStackPanel.Visibility = Visibility.Hidden;

            if ((_canSelect || _canResizeSelection) && _selectionStartPos != _selectionEndPos)
            {
                SelectionOverlay.Rect = _selectionBoundsResolver.GetSelectionBounds(_selectionStartPos, _selectionEndPos);
            }
            else if (_canMoveSelection)
            {
                SelectionOverlay.Rect = _selectionBoundsResolver.GetSelectionMoveBounds(_selectionStartPos,
                    _selectionEndPos, SelectionOverlay.Rect);
                _selectionStartPos = _selectionEndPos;
            }
        }

        private void OnMainCanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Visibility == Visibility.Collapsed)
                return;

            _canSelect = false;
            _canMoveSelection = false;
            _canResizeSelection = false;

            if (SelectionOverlay.Rect.Width < 1 || SelectionOverlay.Rect.Width < 1)
                return;

            _areaSelected = true;
            ActionStackPanel.Visibility = Visibility.Visible;

            SelectionOverlay.Rect = _selectionBoundsResolver.ResolveSelectionOverflow(SelectionOverlay.Rect, ImageOverlay.Rect);
            _selectionStartPos = new Point();
            _selectionEndPos = new Point();
        }

        private void OnScreenshotWindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void ResolveActionStackPanelLocation()
        {
            if (ActionStackPanel.Visibility != Visibility.Visible) return;

            const int addition = 25;

            bool notEnoughSpaceRight = MainCanvas.Width - SelectionOverlay.Rect.Right < ActionStackPanel.Width + addition;
            bool notEnoughSpaceLeft = SelectionOverlay.Rect.Left < ActionStackPanel.Width + addition;

            if (notEnoughSpaceLeft && notEnoughSpaceRight)
            {
                Canvas.SetLeft(ActionStackPanel, SelectionOverlay.Rect.Right - ActionStackPanel.Width);
            }
            else if (notEnoughSpaceRight)
            {
                Canvas.SetLeft(ActionStackPanel, SelectionOverlay.Rect.Left - ActionStackPanel.Width - addition);

            }
            else
            {
                Canvas.SetLeft(ActionStackPanel, SelectionOverlay.Rect.Right + addition);

            }

            if (Canvas.GetTop(ActionStackPanel) + ActionStackPanel.Height > MainCanvas.Height)
            {
                Canvas.SetBottom(ActionStackPanel, SelectionOverlay.Rect.Top);
            }
            else if (SelectionOverlay.Rect.Bottom < ActionStackPanel.Height)
            {
                Canvas.SetTop(ActionStackPanel, SelectionOverlay.Rect.Bottom);
            }
            else
            {
                Canvas.SetTop(ActionStackPanel, SelectionOverlay.Rect.Bottom - ActionStackPanel.Height);
            }
        }

        private void ResolveWindowBounds()
        {
            var viewModel = DataContext as IScreenBoundsProvider;

            if (viewModel != null)
            {
                Width = viewModel.ScreenBounds.Width;
                Height = viewModel.ScreenBounds.Height;
                Left = viewModel.ScreenBounds.Left;
                Top = viewModel.ScreenBounds.Top;

                ImageOverlay.Rect = new Rect(0, 0, viewModel.ScreenBounds.Width, viewModel.ScreenBounds.Height);
            }
        }

        protected override void OnWindowBaseClosed(object sender, EventArgs e)
        {
            base.OnWindowBaseClosed(sender, e);
            ResolveWindowBounds();
        }
    }
}
