// using System;
// using System.Drawing;
// using System.IO;
// using System.Windows.Forms;
using System.ComponentModel;

namespace MyProject.UI.Controls
{
    public class BuildingControl : Panel
    {
        private Image _baseFloorImage;
        private Image _normalFloorImage;
        private Image _topFloorImage;
        private int _floorCount = 3; // Default: base + 1 normal + top

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FloorCount
        {
            get => _floorCount;
            set
            {
                // Ensure 2-10 floors (base + 0-8 normal + top)
                _floorCount = Math.Clamp(value, 2, 10);
                CalculateSize();
                Invalidate(); // Redraw the control
            }
        }

        public BuildingControl()
        {
            // Load images
            string basePath = Path.Combine(Application.StartupPath, "resources", "edificio");
            _baseFloorImage = Image.FromFile(Path.Combine(basePath, "piso-base.png"));
            _normalFloorImage = Image.FromFile(Path.Combine(basePath, "piso-normal.png"));
            _topFloorImage = Image.FromFile(Path.Combine(basePath, "piso-superior.png"));

            DoubleBuffered = true; // Smoother drawing
            CalculateSize();
        }

        private void CalculateSize()
        {
            // Calculate size based on images and floor count
            int width = Math.Max(_baseFloorImage.Width,
                Math.Max(_normalFloorImage.Width, _topFloorImage.Width));

            // Height = base + normal floors + top
            int normalFloorsCount = _floorCount - 2; // subtract base and top
            int height = _baseFloorImage.Height +
                         (normalFloorsCount * _normalFloorImage.Height) +
                         _topFloorImage.Height;

            Size = new Size(width, height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            int normalFloorsCount = _floorCount - 2; // subtract base and top
            int y = 0;

            // Draw top floor (draw from top to bottom)
            g.DrawImage(_topFloorImage, (Width - _topFloorImage.Width) / 2, y);
            y += _topFloorImage.Height;

            // Draw normal floors
            for (int i = 0; i < normalFloorsCount; i++)
            {
                g.DrawImage(_normalFloorImage, (Width - _normalFloorImage.Width) / 2, y);
                y += _normalFloorImage.Height;
            }

            // Draw base floor
            g.DrawImage(_baseFloorImage, (Width - _baseFloorImage.Width) / 2, y);
        }
    }
}