using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GeoCalc.Controls;
using GeoCalc.Helpers;
using GeoCalc.Shapes;

namespace GeoCalc
{
    public partial class MainForm : Form
    {
        private List<Shape2D> _shapes2D;
        private List<Shape3D> _shapes3D;
        private Shape _currentShape;
        private int _selectedCalculationType = 0;

        // Farby
        private readonly Color _bgDark = Color.FromArgb(20, 20, 26);
        private readonly Color _bgMedium = Color.FromArgb(30, 30, 40);
        private readonly Color _bgLight = Color.FromArgb(45, 45, 58);
        private readonly Color _accentBlue = Color.FromArgb(100, 150, 200);
        private readonly Color _accentGreen = Color.FromArgb(100, 200, 140);
        private readonly Color _accentPurple = Color.FromArgb(150, 100, 200);
        private readonly Color _textPrimary = Color.FromArgb(250, 250, 255);
        private readonly Color _textSecondary = Color.FromArgb(170, 170, 190);

        // UI komponenty
        private Panel _menuPanel;
        private Panel _contentPanel;
        private RoundedPanel _inputPanel;
        private RoundedPanel _resultPanel;
        private RoundedPanel _formulaPanel;
        private RoundedPanel _calculationTypePanel;
        private PictureBox _shapeImage;
        private Label _titleLabel;
        private Label _subtitleLabel;
        private List<RoundedButton> _menuButtons;
        private List<RoundedButton> _calculationTypeButtons;
        private List<TextBox> _inputFields;
        private List<Label> _inputLabels;
        private RichTextBox _resultTextBox;
        private RichTextBox _formulaTextBox;
        private RoundedButton _calculateButton;
        private RoundedButton _clearButton;
        private RoundedButton _exportButton;
        private ToolTip _toolTip;

        public MainForm()
        {
            InitializeComponent();
            
            _shapes2D = new List<Shape2D>
            {
                new Circle(),
                new Rectangle(),
                new Square(),
                new Triangle(),
                new Trapezoid(),
                new Rhombus()
            };

            _shapes3D = new List<Shape3D>
            {
                new Cube(),
                new Cuboid(),
                new Sphere(),
                new Cylinder(),
                new Cone(),
                new Pyramid()
            };

            _menuButtons = new List<RoundedButton>();
            _calculationTypeButtons = new List<RoundedButton>();
            _inputFields = new List<TextBox>();
            _inputLabels = new List<Label>();
            _toolTip = new ToolTip();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.BackColor = _bgDark;
            this.DoubleBuffered = true;
            SetupUI();
            
            if (_shapes2D.Count > 0)
            {
                SelectShape(_shapes2D[0]);
                if (_menuButtons.Count > 0)
                    _menuButtons[0].IsSelected = true;
            }
        }

        private void SetupUI()
        {
            // ĽAVÝ MENU PANEL
            _menuPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = _bgMedium,
                Padding = new Padding(12)
            };
            this.Controls.Add(_menuPanel);

            var logoLabel = new Label
            {
                Text = "📐 GeoCalc",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = _accentBlue,
                Dock = DockStyle.Top,
                Height = 70,
                TextAlign = ContentAlignment.MiddleCenter
            };
            _menuPanel.Controls.Add(logoLabel);

            var separator1 = new Panel
            {
                Dock = DockStyle.Top,
                Height = 2,
                BackColor = _accentBlue
            };
            _menuPanel.Controls.Add(separator1);

            var menuScrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = _bgMedium
            };
            _menuPanel.Controls.Add(menuScrollPanel);

            int buttonY = 15;

            var label2D = new Label
            {
                Text = "📊 2D TVARY",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = _accentGreen,
                Location = new Point(15, buttonY),
                AutoSize = true
            };
            menuScrollPanel.Controls.Add(label2D);
            buttonY += 35;

            foreach (var shape in _shapes2D)
            {
                var btn = CreateMenuButton(shape.Name, buttonY);
                btn.Tag = shape;
                btn.Click += OnShapeButtonClick;
                menuScrollPanel.Controls.Add(btn);
                _menuButtons.Add(btn);
                buttonY += 55;
            }

            buttonY += 20;

            var label3D = new Label
            {
                Text = "🎲 3D TVARY",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = _accentPurple,
                Location = new Point(15, buttonY),
                AutoSize = true
            };
            menuScrollPanel.Controls.Add(label3D);
            buttonY += 35;

            foreach (var shape in _shapes3D)
            {
                var btn = CreateMenuButton(shape.Name, buttonY);
                btn.Tag = shape;
                btn.Click += OnShapeButtonClick;
                menuScrollPanel.Controls.Add(btn);
                _menuButtons.Add(btn);
                buttonY += 55;
            }

            // HLAVNÝ OBSAHOVÝ PANEL
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _bgDark,
                Padding = new Padding(25),
                AutoScroll = true
            };
            this.Controls.Add(_contentPanel);

            _titleLabel = new Label
            {
                Font = new Font("Segoe UI", 32f, FontStyle.Bold),
                ForeColor = _accentBlue,
                Location = new Point(25, 15),
                AutoSize = true
            };
            _contentPanel.Controls.Add(_titleLabel);

            _subtitleLabel = new Label
            {
                Font = new Font("Segoe UI", 12f),
                ForeColor = _textSecondary,
                Location = new Point(27, 58),
                AutoSize = true
            };
            _contentPanel.Controls.Add(_subtitleLabel);

            // OBRÁZOK TVARU
            _shapeImage = new PictureBox
            {
                Size = new Size(180, 180),
                Location = new Point(25, 100),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            _contentPanel.Controls.Add(_shapeImage);

            // PANEL VÝBERU VÝPOČTU
            _calculationTypePanel = new RoundedPanel
            {
                BackColor = _bgMedium,
                Location = new Point(220, 100),
                Size = new Size(350, 200),
                BorderRadius = 18,
                BorderColor = _accentBlue,
                BorderWidth = 2
            };
            _contentPanel.Controls.Add(_calculationTypePanel);

            var calcTypeTitle = new Label
            {
                Text = "🎯 Vyber výpočet",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = _textPrimary,
                Location = new Point(15, 15),
                AutoSize = true
            };
            _calculationTypePanel.Controls.Add(calcTypeTitle);

            // PANEL VSTUPOV
            _inputPanel = new RoundedPanel
            {
                BackColor = _bgMedium,
                Location = new Point(590, 100),
                Size = new Size(360, 200),
                BorderRadius = 18,
                BorderColor = _accentGreen,
                BorderWidth = 2
            };
            _contentPanel.Controls.Add(_inputPanel);

            var inputTitle = new Label
            {
                Text = "📝 Zadajte rozmery",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = _textPrimary,
                Location = new Point(15, 15),
                AutoSize = true
            };
            _inputPanel.Controls.Add(inputTitle);

            // PANEL VZORCOV
            _formulaPanel = new RoundedPanel
            {
                BackColor = _bgMedium,
                Location = new Point(970, 100),
                Size = new Size(400, 500),
                BorderRadius = 18,
                BorderColor = _accentGreen,
                BorderWidth = 2
            };
            _contentPanel.Controls.Add(_formulaPanel);

            var formulaTitle = new Label
            {
                Text = "📖 Vzorce",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = _accentGreen,
                Location = new Point(15, 12),
                AutoSize = true
            };
            _formulaPanel.Controls.Add(formulaTitle);

            _formulaTextBox = new RichTextBox
            {
                Location = new Point(15, 45),
                Size = new Size(370, 440),
                BackColor = _bgLight,
                ForeColor = _accentGreen,
                Font = new Font("Consolas", 10f),
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                WordWrap = true
            };
            _formulaPanel.Controls.Add(_formulaTextBox);

            // PANEL VÝSLEDKOV
            _resultPanel = new RoundedPanel
            {
                BackColor = _bgMedium,
                Location = new Point(25, 330),
                Size = new Size(540, 270),
                BorderRadius = 18,
                BorderColor = _accentGreen,
                BorderWidth = 2
            };
            _contentPanel.Controls.Add(_resultPanel);

            var resultTitle = new Label
            {
                Text = "📊 Výsledky",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = _accentGreen,
                Location = new Point(15, 12),
                AutoSize = true
            };
            _resultPanel.Controls.Add(resultTitle);

            _resultTextBox = new RichTextBox
            {
                Location = new Point(15, 45),
                Size = new Size(510, 160),
                BackColor = _bgLight,
                ForeColor = _accentGreen,
                Font = new Font("Consolas", 11f),
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                WordWrap = true
            };
            _resultPanel.Controls.Add(_resultTextBox);

            // TLAČIDLÁ
            _calculateButton = new RoundedButton
            {
                Text = "🔢 Vypočítať",
                Size = new Size(155, 45),
                Location = new Point(15, 215),
                NormalColor = _accentBlue,
                SelectedColor = _accentBlue,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BorderRadius = 10
            };
            _calculateButton.Click += OnCalculateClick;
            _resultPanel.Controls.Add(_calculateButton);

            _clearButton = new RoundedButton
            {
                Text = "🗑️ Vyčistiť",
                Size = new Size(155, 45),
                Location = new Point(180, 215),
                NormalColor = Color.FromArgb(100, 100, 110),
                SelectedColor = Color.FromArgb(120, 120, 130),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BorderRadius = 10
            };
            _clearButton.Click += OnClearClick;
            _resultPanel.Controls.Add(_clearButton);

            _exportButton = new RoundedButton
            {
                Text = "💾 Exportovať",
                Size = new Size(155, 45),
                Location = new Point(345, 215),
                NormalColor = Color.FromArgb(100, 130, 90),
                SelectedColor = Color.FromArgb(120, 150, 110),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BorderRadius = 10
            };
            _exportButton.Click += OnExportClick;
            _resultPanel.Controls.Add(_exportButton);
        }

        private RoundedButton CreateMenuButton(string text, int y)
        {
            return new RoundedButton
            {
                Text = text,
                Size = new Size(220, 48),
                Location = new Point(10, y),
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                NormalColor = _bgLight,
                SelectedColor = _accentBlue,
                BorderRadius = 12
            };
        }

        private void OnShapeButtonClick(object sender, EventArgs e)
        {
            var button = sender as RoundedButton;
            if (button?.Tag is Shape shape)
            {
                foreach (var btn in _menuButtons)
                    btn.IsSelected = false;

                button.IsSelected = true;
                SelectShape(shape);
            }
        }

        private void SelectShape(Shape shape)
        {
            _currentShape = shape;
            _titleLabel.Text = shape.Name;
            _subtitleLabel.Text = shape is Shape2D ? "2D tvar • Vyberte typ výpočtu a zadajte rozmery" : "3D tvar • Vyberte typ výpočtu a zadajte rozmery";
            _formulaTextBox.Text = shape.GetFormulas();
            _resultTextBox.Clear();
            CreateInputFields(shape);
            CreateCalculationTypeButtons(shape);
            LoadShapeImage(shape);
        }

        private void CreateInputFields(Shape shape)
        {
            foreach (var field in _inputFields)
            {
                _inputPanel.Controls.Remove(field);
                field.Dispose();
            }
            foreach (var label in _inputLabels)
            {
                _inputPanel.Controls.Remove(label);
                label.Dispose();
            }
            _inputFields.Clear();
            _inputLabels.Clear();

            int y = 50;
            for (int i = 0; i < shape.ParameterNames.Length; i++)
            {
                var label = new Label
                {
                    Text = shape.ParameterNames[i] + ":",
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = _textPrimary,
                    Location = new Point(15, y + 3),
                    AutoSize = true
                };
                _inputPanel.Controls.Add(label);
                _inputLabels.Add(label);

                var textBox = new TextBox
                {
                    Size = new Size(130, 32),
                    Location = new Point(210, y),
                    BackColor = _bgLight,
                    ForeColor = _accentBlue,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Segoe UI", 11f, FontStyle.Bold)
                };

                if (i < shape.ParameterDescriptions.Length)
                    _toolTip.SetToolTip(textBox, shape.ParameterDescriptions[i]);

                textBox.KeyPress += (s, e) =>
                {
                    if (e.KeyChar == (char)Keys.Enter)
                    {
                        e.Handled = true;
                        OnCalculateClick(s, e);
                    }
                };

                _inputPanel.Controls.Add(textBox);
                _inputFields.Add(textBox);
                y += 38;
            }

            _inputPanel.Height = Math.Max(200, y + 20);
        }

        private void CreateCalculationTypeButtons(Shape shape)
        {
            foreach (var btn in _calculationTypeButtons)
            {
                _calculationTypePanel.Controls.Remove(btn);
                btn.Dispose();
            }
            _calculationTypeButtons.Clear();

            int y = 45;

            if (shape is Shape2D)
            {
                var btnPerimeter = new RoundedButton
                {
                    Text = "📏 Obvod",
                    Size = new Size(150, 40),
                    Location = new Point(15, y),
                    NormalColor = _bgLight,
                    SelectedColor = _accentGreen,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    IsSelected = true,
                    BorderRadius = 10
                };
                btnPerimeter.Click += (s, e) => SelectCalculationType(0);
                _calculationTypePanel.Controls.Add(btnPerimeter);
                _calculationTypeButtons.Add(btnPerimeter);
                y += 50;

                var btnArea = new RoundedButton
                {
                    Text = "📐 Obsah",
                    Size = new Size(150, 40),
                    Location = new Point(15, y),
                    NormalColor = _bgLight,
                    SelectedColor = _accentGreen,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    IsSelected = false,
                    BorderRadius = 10
                };
                btnArea.Click += (s, e) => SelectCalculationType(1);
                _calculationTypePanel.Controls.Add(btnArea);
                _calculationTypeButtons.Add(btnArea);
            }
            else if (shape is Shape3D)
            {
                var btnVolume = new RoundedButton
                {
                    Text = "🎲 Objem",
                    Size = new Size(150, 40),
                    Location = new Point(15, y),
                    NormalColor = _bgLight,
                    SelectedColor = _accentPurple,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    IsSelected = true,
                    BorderRadius = 10
                };
                btnVolume.Click += (s, e) => SelectCalculationType(0);
                _calculationTypePanel.Controls.Add(btnVolume);
                _calculationTypeButtons.Add(btnVolume);
                y += 50;

                var btnSurface = new RoundedButton
                {
                    Text = "🔲 Povrch",
                    Size = new Size(150, 40),
                    Location = new Point(15, y),
                    NormalColor = _bgLight,
                    SelectedColor = _accentPurple,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    IsSelected = false,
                    BorderRadius = 10
                };
                btnSurface.Click += (s, e) => SelectCalculationType(1);
                _calculationTypePanel.Controls.Add(btnSurface);
                _calculationTypeButtons.Add(btnSurface);
            }

            _selectedCalculationType = 0;
        }

        private void SelectCalculationType(int index)
        {
            foreach (var btn in _calculationTypeButtons)
                btn.IsSelected = false;

            if (index >= 0 && index < _calculationTypeButtons.Count)
            {
                _calculationTypeButtons[index].IsSelected = true;
                _selectedCalculationType = index;
            }
        }

        private void LoadShapeImage(Shape shape)
        {
            Bitmap bmp = new Bitmap(180, 180);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                using (Pen pen = new Pen(_accentBlue, 3))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(60, 100, 150, 200)))
                {
                    DrawShape(g, pen, brush, shape.Name);
                }
            }

            if (_shapeImage.Image != null)
                _shapeImage.Image.Dispose();
            _shapeImage.Image = bmp;
        }

        private void DrawShape(Graphics g, Pen pen, SolidBrush brush, string shapeName)
        {
            switch (shapeName)
            {
                case "Kruh":
                    g.FillEllipse(brush, 25, 25, 130, 130);
                    g.DrawEllipse(pen, 25, 25, 130, 130);
                    break;
                case "Obdĺžnik":
                    g.FillRectangle(brush, 20, 40, 140, 100);
                    g.DrawRectangle(pen, 20, 40, 140, 100);
                    break;
                case "Štvorec":
                    g.FillRectangle(brush, 30, 30, 120, 120);
                    g.DrawRectangle(pen, 30, 30, 120, 120);
                    break;
                case "Trojuholník":
                    Point[] triangle = { new Point(90, 15), new Point(165, 150), new Point(15, 150) };
                    g.FillPolygon(brush, triangle);
                    g.DrawPolygon(pen, triangle);
                    break;
                case "Lichobežník":
                    Point[] trapezoid = { new Point(40, 30), new Point(140, 30), new Point(160, 150), new Point(20, 150) };
                    g.FillPolygon(brush, trapezoid);
                    g.DrawPolygon(pen, trapezoid);
                    break;
                case "Kosoštvorec":
                    Point[] rhombus = { new Point(90, 10), new Point(170, 90), new Point(90, 170), new Point(10, 90) };
                    g.FillPolygon(brush, rhombus);
                    g.DrawPolygon(pen, rhombus);
                    break;
                case "Kocka":
                    Point[] front = { new Point(30, 50), new Point(120, 50), new Point(120, 140), new Point(30, 140) };
                    g.FillPolygon(brush, front);
                    g.DrawPolygon(pen, front);
                    Point[] top = { new Point(30, 50), new Point(70, 20), new Point(160, 20), new Point(120, 50) };
                    g.FillPolygon(new SolidBrush(Color.FromArgb(80, 100, 150, 200)), top);
                    g.DrawPolygon(pen, top);
                    break;
                case "Guľa":
                    g.FillEllipse(brush, 25, 25, 130, 130);
                    g.DrawEllipse(pen, 25, 25, 130, 130);
                    break;
                case "Valec":
                    g.FillRectangle(brush, 40, 50, 100, 80);
                    g.DrawLine(pen, 40, 50, 40, 130);
                    g.DrawLine(pen, 140, 50, 140, 130);
                    g.FillEllipse(new SolidBrush(Color.FromArgb(80, 100, 150, 200)), 40, 30, 100, 40);
                    g.DrawEllipse(pen, 40, 30, 100, 40);
                    break;
                case "Kužeľ":
                    Point[] cone = { new Point(90, 20), new Point(160, 140), new Point(20, 140) };
                    g.FillPolygon(brush, cone);
                    g.DrawLine(pen, 90, 20, 20, 140);
                    g.DrawLine(pen, 90, 20, 160, 140);
                    break;
                default:
                    g.FillRectangle(brush, 25, 25, 130, 130);
                    g.DrawRectangle(pen, 25, 25, 130, 130);
                    break;
            }
        }

        private void OnCalculateClick(object sender, EventArgs e)
        {
            if (_currentShape == null) return;

            double[] parameters = new double[_inputFields.Count];
            for (int i = 0; i < _inputFields.Count; i++)
            {
                if (!InputValidator.TryParseNumber(_inputFields[i].Text, out parameters[i]))
                {
                    MessageBox.Show($"Neplatná hodnota v poli '{_currentShape.ParameterNames[i]}'.", "Chyba vstupu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _inputFields[i].Focus();
                    return;
                }
            }

            if (!_currentShape.ValidateParameters(parameters, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Chyba validácie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string result = _currentShape.Calculate(parameters, _selectedCalculationType);
                _resultTextBox.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Pri výpočte nastala chyba: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnClearClick(object sender, EventArgs e)
        {
            foreach (var field in _inputFields)
                field.Clear();
            _resultTextBox.Clear();
            if (_inputFields.Count > 0)
                _inputFields[0].Focus();
        }

        private void OnExportClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_resultTextBox.Text))
            {
                MessageBox.Show("Najprv vykonajte výpočet.", "Nie je čo exportovať", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Textové súbory (*.txt)|*.txt";
                dialog.DefaultExt = "txt";
                dialog.FileName = $"GeoCalc_{_currentShape?.Name}_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string[] calcTypeNames = _currentShape is Shape2D ? new[] { "Obvod", "Obsah" } : new[] { "Objem", "Povrch" };
                        string content = $"GeoCalc - Export výsledkov\nDátum: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\nTvar: {_currentShape?.Name}\nTyp výpočtu: {calcTypeNames[_selectedCalculationType]}\n\n";
                        for (int i = 0; i < _inputFields.Count; i++)
                            content += $"{_currentShape.ParameterNames[i]}: {_inputFields[i].Text}\n";
                        content += $"\n{_formulaTextBox.Text}\n\n{_resultTextBox.Text}";
                        File.WriteAllText(dialog.FileName, content);
                        MessageBox.Show($"Výsledky boli exportované do: {dialog.FileName}", "Úspech", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Chyba pri ukladaní: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
