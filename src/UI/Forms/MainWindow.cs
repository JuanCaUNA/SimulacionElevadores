// not uset libraries:
// using System;
// using System.Drawing;
// using System.Windows.Forms;

using MyProject.UI.Controls;

namespace MyProject.UI.Forms
{
    public class MainForm : Form
    {
        private BuildingControl buildingControl = null!;
        private NumericUpDown floorCountUpDown = null!;
        private Label floorCountLabel = null!;
        private Panel controlPanel = null!;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Simulador de Edificio";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel for controls (bottom of form)
            controlPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };

            // Floor count controls
            floorCountLabel = new Label
            {
                Text = "Número de pisos:",
                Location = new Point(20, 15),
                AutoSize = true
            };
            controlPanel.Controls.Add(floorCountLabel);

            floorCountUpDown = new NumericUpDown
            {
                Minimum = 2,
                Maximum = 10,
                Value = 3,
                Location = new Point(120, 13),
                Width = 60
            };
            floorCountUpDown.ValueChanged += FloorCountUpDown_ValueChanged;
            controlPanel.Controls.Add(floorCountUpDown);

            // Building visualization control (centered in form)
            buildingControl = new BuildingControl
            {
                FloorCount = (int)floorCountUpDown.Value
            };

            // Add controls to form
            this.Controls.Add(buildingControl);
            this.Controls.Add(controlPanel);

            // Center the building control
            buildingControl.Location = new Point(
                (this.ClientSize.Width - buildingControl.Width) / 2,
                (this.ClientSize.Height - buildingControl.Height - controlPanel.Height) / 2
            );

            // Handle form resize to keep building centered
            this.Resize += MainForm_Resize;
        }

        private void FloorCountUpDown_ValueChanged(object? sender, EventArgs e)
        {
            buildingControl.FloorCount = (int)floorCountUpDown.Value;
            CenterBuildingControl();
        }

        private void MainForm_Resize(object? sender, EventArgs e)
        {
            CenterBuildingControl();
        }

        private void CenterBuildingControl()
        {
            buildingControl.Location = new Point(
                (this.ClientSize.Width - buildingControl.Width) / 2,
                (this.ClientSize.Height - buildingControl.Height - controlPanel.Height) / 2
            );
        }
    }
}