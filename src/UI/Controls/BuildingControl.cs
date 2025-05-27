using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;

namespace MyProject.UI.Controls
{
    public class BuildingControl : Panel
    {
        private Image _baseFloorImage;
        private Image _normalFloorImage;
        private Image _topFloorImage;
        private int _floorCount = 3; // Default: base + 1 normal + top
        private float _scaleFactor = 0.50f; // factor del redimensionamiento

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FloorCount
        {
            get => _floorCount;
            set
            {
                _floorCount = Math.Clamp(value, 2, 10);
                CalculateSize();
                Invalidate();
            }
        }

        [DefaultValue(0.75f)]
        public float ScaleFactor
        {
            get => _scaleFactor;
            set
            {
                _scaleFactor = Math.Max(0.1f, Math.Min(2.0f, value));
                LoadAndScaleImages();
                CalculateSize();
                Invalidate();
            }
        }

        private Image _originalBaseFloorImage;
        private Image _originalNormalFloorImage;
        private Image _originalTopFloorImage;

        public BuildingControl()
        {
            // Load original images
            string basePath = Path.Combine(Application.StartupPath, "resources", "edificio");
            _originalBaseFloorImage = Image.FromFile(Path.Combine(basePath, "piso-base.png"));
            _originalNormalFloorImage = Image.FromFile(Path.Combine(basePath, "piso-normal.png"));
            _originalTopFloorImage = Image.FromFile(Path.Combine(basePath, "piso-superior.png"));

            LoadAndScaleImages();

            DoubleBuffered = true;
            CalculateSize();
        }

        private void LoadAndScaleImages()
        {
            _baseFloorImage = ResizeImage(_originalBaseFloorImage, _scaleFactor);
            _normalFloorImage = ResizeImage(_originalNormalFloorImage, _scaleFactor);
            _topFloorImage = ResizeImage(_originalTopFloorImage, _scaleFactor);
        }

        private Image ResizeImage(Image img, float scale)
        {
            int width = (int)(img.Width * scale);
            int height = (int)(img.Height * scale);
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, width, height);
            }
            return bmp;
        }

        private void CalculateSize()
        {
            int width = Math.Max(_baseFloorImage.Width,
                Math.Max(_normalFloorImage.Width, _topFloorImage.Width));

            int normalFloorsCount = _floorCount - 2;
            int height = _baseFloorImage.Height +
                         (normalFloorsCount * _normalFloorImage.Height) +
                         _topFloorImage.Height;

            Size = new Size(width, height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            int normalFloorsCount = _floorCount - 2;
            int y = 0;

            g.DrawImage(_topFloorImage, (Width - _topFloorImage.Width) / 2, y);
            y += _topFloorImage.Height;

            for (int i = 0; i < normalFloorsCount; i++)
            {
                g.DrawImage(_normalFloorImage, (Width - _normalFloorImage.Width) / 2, y);
                y += _normalFloorImage.Height;
            }

            g.DrawImage(_baseFloorImage, (Width - _baseFloorImage.Width) / 2, y);
        }
    }
}