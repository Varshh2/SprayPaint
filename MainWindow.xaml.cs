using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SprayPaints
{
    public partial class MainWindow : Window
    {
        private bool isPainting = false;
        private bool isErasing = false;
        private List<Shape> paintedShapes = new List<Shape>();

        public MainWindow()
        {
            InitializeComponent();

            // Add event handlers for mouse events on the canvas
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseUp += Canvas_MouseUp;
            canvas.MouseMove += Canvas_MouseMove;
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                LoadImage(imagePath);
            }
        }

        private void LoadImage(string imagePath)
        {
            try
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));

                double canvasWidth = canvas.ActualWidth - 2 * 96; // 96 pixels for 1-inch border on each side
                double canvasHeight = canvas.ActualHeight - 2 * 96; // 96 pixels for 1-inch border on each side

                double aspectRatio = bitmapImage.PixelWidth / (double)bitmapImage.PixelHeight;

                double imageWidth = Math.Min(canvasWidth, canvasHeight * aspectRatio);
                double imageHeight = Math.Min(canvasHeight, canvasWidth / aspectRatio);

                Image image = new Image();
                image.Source = bitmapImage;
                image.Stretch = Stretch.Uniform; // Set Stretch property to Uniform

                double leftMargin = (canvasWidth - imageWidth) / 2;
                double topMargin = (canvasHeight - imageHeight) / 2;

                image.Margin = new Thickness(leftMargin + 96, topMargin + 96, 0, 0); // 1-inch border on each side
                image.Width = imageWidth;
                image.Height = imageHeight;

                // Clear existing elements from the canvas
                canvas.Children.Clear();

                // Add the image to the canvas with borders
                canvas.Children.Add(image);

                SelectedImageText.Text = $"Selected Image: {System.IO.Path.GetFileName(imagePath)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Clear the list of painted shapes when a new image is loaded
            paintedShapes.Clear();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isPainting = true;
                isErasing = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                DrawShape(e.GetPosition(canvas));
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isPainting = false;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPainting)
            {
                // Check if the mouse position is inside the image bounds
                if (IsMouseInsideImage(e.GetPosition(canvas)))
                {
                    DrawShape(e.GetPosition(canvas));
                }
            }
        }

        private bool IsMouseInsideImage(Point mousePosition)
        {
            if (canvas.Children.Count > 0 && canvas.Children[0] is Image image)
            {
                double imageLeft = Canvas.GetLeft(image);
                double imageTop = Canvas.GetTop(image);
                double imageRight = imageLeft + image.Width;
                double imageBottom = imageTop + image.Height;

                return mousePosition.X >= imageLeft && mousePosition.X <= imageRight
                    && mousePosition.Y >= imageTop && mousePosition.Y <= imageBottom;
            }

            return false;
        }

        private void DrawShape(Point position)
        {
            Shape shape = isErasing ? CreateEraserShape(position) : CreatePaintShape(position);
            canvas.Children.Add(shape);
            paintedShapes.Add(shape);
        }

        private Ellipse CreatePaintShape(Point position)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = new SolidColorBrush(GetSelectedColor());
            ellipse.Width = brushSizeSlider.Value;
            ellipse.Height = brushSizeSlider.Value;

            Canvas.SetLeft(ellipse, position.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, position.Y - ellipse.Height / 2);

            return ellipse;
        }

        private Rectangle CreateEraserShape(Point position)
        {
            Rectangle eraser = new Rectangle();
            eraser.Fill = Brushes.White; // Set eraser color (in this case, white)
            eraser.Width = brushSizeSlider.Value;
            eraser.Height = brushSizeSlider.Value;

            Canvas.SetLeft(eraser, position.X - eraser.Width / 2);
            Canvas.SetTop(eraser, position.Y - eraser.Height / 2);

            return eraser;
        }

        private Color GetSelectedColor()
        {
            string selectedColor = (colorPicker.SelectedItem as ComboBoxItem)?.Content.ToString();

            switch (selectedColor)
            {
                case "Black":
                    return Colors.Black;
                case "Red":
                    return Colors.Red;
                case "Green":
                    return Colors.Green;
                case "Blue":
                    return Colors.Blue;
                default:
                    return Colors.Black;
            }
        }

        private void UndoPaint()
        {
            if (paintedShapes.Count > 0)
            {
                // Remove the last painted shape from the canvas and the list
                canvas.Children.Remove(paintedShapes[paintedShapes.Count - 1]);
                paintedShapes.RemoveAt(paintedShapes.Count - 1);
            }
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            UndoPaint();
        }

        private void EraserButton_Click(object sender, RoutedEventArgs e)
        {
            // Call the undo method directly on button click
            UndoPaint();

            // Update the button appearance based on the mode
            Button eraserButton = (Button)sender;
            if (isErasing)
            {
                eraserButton.Background = Brushes.Gray; // Set a different background color for the eraser button when active
            }
            else
            {
                eraserButton.Background = (Brush)new BrushConverter().ConvertFromString("#3498db"); // Restore the original background color
            }
        }
    }
}
