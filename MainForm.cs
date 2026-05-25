using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GeoCalc.Controls;
using GeoCalc.Helpers;
using GeoCalc.Shapes;

namespace GeoCalc
{
    /// <summary>
    /// Hlavný formulár aplikácie GeoCalc s vylepšeným dizajnom a funkcionalitou.
    /// </summary>
    public partial class MainForm : Form
    {
        // Kolekcie tvarov
        private readonly List<Shape2D> _shapes2D;
        private readonly List<Shape3D> _shapes3D;
        
        // Aktuálne vybraný tvar
        private Shape _currentShape;
        
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
        
        // Vybraný typ výpočtu
        private int _selectedCalculationType = 0;

        // Farby pre dark theme
        private readonly Color _bgDark = Color.FromArgb(20, 20, 26);
        private readonly Color _bgMedium = Color.FromArgb(30, 30, 40);
        private readonly Color _bgLight = Color.FromArgb(45, 45, 58);
        private readonly Color _accentBlue = Color.FromArgb(100, 150, 200);
        private readonly Color _accentGreen = Color.FromArgb(100, 200, 140);
        private readonly Color _accentPurple = Color.FromArgb(150, 100, 200);
        private readonly Color _textPrimary = Color.FromArgb(250, 250, 255);
        private readonly Color _textSecondary = Color.FromArgb(170, 170, 190);

        public MainForm()
        {
            // Inicializácia tvarov
            _shapes2D = new List<Shape2D>
            {
                new Circle(),
                new Shapes.Rectangle(),
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

            InitializeComponent();
            SetupUI();
            
            // Vybrať prvý tvar ako predvolený
            if (_shapes2D.Count > 0)
            {
                SelectShape(_shapes2D[0]);
                if (_menuButtons.Count > 0)
                    _menuButtons[0].IsSelected = true;
            }
        }

        /// <summary>
        /// Inicializácia vlastností formulára pri načítaní.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = Point.Empty;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = _bgDark;
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Vytvorí a nastaví všetky UI komponenty s novým dizajnom.
        /// </summary>
        private void SetupUI()
        {
            // === ĽAVÝ PANEL MENU ===
            _menuPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = _bgMedium,
                Padding = new Padding(12)
            };
            this.Controls.Add(_menuPanel);

            // Logo/Nadpis v menu
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

            // Separator
            var separator1 = new Panel
            {
                Dock = DockStyle.Top,
                Height = 2,
                BackColor = _accentBlue
            };
            _menuPanel.Controls.Add(separator1);

            // Scroll panel pre tlačidlá
            var menuScrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = _bgMedium
            };
            _menuPanel.Controls.Add(menuScrollPanel);

            int buttonY = 15;

            // Nadpis 2D tvary
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

            // Tlačidlá pre 2D tvary
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

            // Nadpis 3D tvary
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

            // Tlačidlá pre 3D tvary
            foreach (var shape in _shapes3D)
            {
                var btn = CreateMenuButton(shape.Name, buttonY);
                btn.Tag = shape;
                btn.Click += OnShapeButtonClick;
                menuScrollPanel.Controls.Add(btn);
                _menuButtons.Add(btn);
                buttonY += 55;
            }

            // === HLAVNÝ OBSAHOVÝ PANEL ===
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _bgDark,
                Padding = new Padding(25),
                AutoScroll = true
            };
            this.Controls.Add(_contentPanel);

            // Nadpis tvaru
            _titleLabel = new Label
            {
                Font = new Font("Segoe UI", 32f, FontStyle.Bold),
                ForeColor = _accentBlue,
                Location = new Point(25, 15),
                AutoSize = true
            };
            _contentPanel.Controls.Add(_titleLabel);

            // Podnadpis
            _subtitleLabel = new Label
            {
                Font = new Font("Segoe UI", 12f),
                ForeColor = _textSecondary,
                Location = new Point(27, 58),
                AutoSize = true
            };
            _contentPanel.Controls.Add(_subtitleLabel);

            // === PANEL S OBRÁZKOM TVARU ===
            _shapeImage = new PictureBox
            {
                Size = new Size(180, 180),
                Location = new Point(25, 100),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            _contentPanel.Controls.Add(_shapeImage);

            // === PANEL VÝBERU VÝPOČTU ===
            _calculationTypePanel = new RoundedPanel
            {
                BackColor = _bgMedium,
                Location = new Point(220, 100),
                Size = new Size(350, 200),
                BorderRadius = 18
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

            // === PANEL VSTUPOV ===
            _inputPanel = new RoundedPanel
            {
                BackColor = _bgMedium,
                Location = new Point(590, 100),
                Size = new Size(360, 200),
                BorderRadius = 18
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

            // === PANEL VZORCOV ===
            _formulaPanel = new RoundedPanel
            {
                BackColor = _bgMedium,
                Location = new Point(970, 100),
                Size = new Size(400, 500),
                BorderRadius = 18
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

            // === PANEL VÝSLEDKOV ===
            _resultPanel = new RoundedPanel
            {
                BackColor = _bgMedium,
                Location = new Point(25, 330),
                Size = new Size(540, 270),
                BorderRadius = 18
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

            // Tlačidlá
            _calculateButton = new RoundedButton
            {
                Text = "🔢 Vypočítať",
                Size = new Size(155, 45),
                Location = new Point(15, 215),
                NormalColor = _accentBlue,
                SelectedColor = _accentBlue,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold)
            };
            _calculateButton.Click += OnCalculateClick;
            _resultPanel.Controls.Add(_calculateButton);

            _clearButton = new RoundedButton
            {
                Text = "🗑️ Vyčistiť",
                Size = new Size(155, 45),
                Location = new Point(180, 215),
                NormalColor = Color.FromArgb(100, 100, 110),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold)
            };
            _clearButton.Click += OnClearClick;
            _resultPanel.Controls.Add(_clearButton);

            _exportButton = new RoundedButton
            {
                Text = "💾 Exportovať",
                Size = new Size(155, 45),
                Location = new Point(345, 215),
                NormalColor = Color.FromArgb(100, 130, 90),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold)
            };
            _exportButton.Click += OnExportClick;
            _resultPanel.Controls.Add(_exportButton);

            // Resize handler
            this.Resize += OnFormResize;
        }

        /// <summary>
        /// Vytvorí tlačidlo menu.
        /// </summary>
        private RoundedButton CreateMenuButton(string text, int y)
        {
            return new RoundedButton
            {
                Text = text,
                Size = new Size(220, 48),
                Location = new Point(10, y),
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                NormalColor = _bgLight,
                SelectedColor = _accentBlue
            };
        }

        /// <summary>
        /// Spracuje kliknutie na tlačidlo tvaru.
        /// </summary>
        private void OnShapeButtonClick(object sender, EventArgs e)
        {
            var button = sender as RoundedButton;
            if (button?.Tag is Shape shape)
            {
                // Zrušiť výber všetkých tlačidiel
                foreach (var btn in _menuButtons)
                    btn.IsSelected = false;

                // Vybrať aktuálne tlačidlo
                button.IsSelected = true;

                // Vybrať tvar
                SelectShape(shape);
            }
        }

        /// <summary>
        /// Nastaví aktuálny tvar a aktualizuje UI.
        /// </summary>
        private void SelectShape(Shape shape)
        {
            _currentShape = shape;
            
            // Aktualizovať nadpis
            _titleLabel.Text = shape.Name;
            
            // Aktualizovať podnadpis
            if (shape is Shape2D)
                _subtitleLabel.Text = "2D tvar • Vyberte typ výpočtu a zadajte rozmery";
            else
                _subtitleLabel.Text = "3D tvar • Vyberte typ výpočtu a zadajte rozmery";

            // Aktualizovať vzorce
            _formulaTextBox.Text = shape.GetFormulas();

            // Vyčistiť výsledky
            _resultTextBox.Clear();

            // Vytvoriť vstupné polia
            CreateInputFields(shape);
            
            // Vytvoriť tlačidlá výpočtu
            CreateCalculationTypeButtons(shape);

            // Načítať obrázok
            LoadShapeImage(shape);
        }

        /// <summary>
        /// Vytvorí vstupné polia podľa parametrov tvaru.
        /// </summary>
        private void CreateInputFields(Shape shape)
        {
            // Odstrániť existujúce polia
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

            // Vytvoriť nové polia
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
                
                // Pridať tooltip
                if (i < shape.ParameterDescriptions.Length)
                {
                    _toolTip.SetToolTip(textBox, shape.ParameterDescriptions[i]);
                }

                // Enter key handler
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

            // Upraviť výšku panelu
            _inputPanel.Height = Math.Max(200, y + 20);
        }

        /// <summary>
        /// Vytvorí tlačidlá pre výber typu výpočtu.
        /// </summary>
        private void CreateCalculationTypeButtons(Shape shape)
        {
            // Odstrániť existujúce tlačidlá
            foreach (var btn in _calculationTypeButtons)
            {
                _calculationTypePanel.Controls.Remove(btn);
                btn.Dispose();
            }
            _calculationTypeButtons.Clear();

            int y = 45;
            int index = 0;

            if (shape is Shape2D shape2D)
            {
                // Tlačidlo pre obvod
                var btnPerimeter = new RoundedButton
                {
                    Text = "📏 Obvod",
                    Size = new Size(150, 40),
                    Location = new Point(15, y),
                    NormalColor = _bgLight,
                    SelectedColor = _accentGreen,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    IsSelected = (index == 0)
                };
                btnPerimeter.Click += (s, e) => SelectCalculationType(0);
                _calculationTypePanel.Controls.Add(btnPerimeter);
                _calculationTypeButtons.Add(btnPerimeter);
                y += 50;
                index++;

                // Tlačidlo pre obsah
                var btnArea = new RoundedButton
                {
                    Text = "📐 Obsah",
                    Size = new Size(150, 40),
                    Location = new Point(15, y),
                    NormalColor = _bgLight,
                    SelectedColor = _accentGreen,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    IsSelected = (index == 0)
                };
                btnArea.Click += (s, e) => SelectCalculationType(1);
                _calculationTypePanel.Controls.Add(btnArea);
                _calculationTypeButtons.Add(btnArea);

                _calculationTypePanel.Height = 140;
            }
            else if (shape is Shape3D shape3D)
            {
                // Tlačidlo pre objem
                var btnVolume = new RoundedButton
                {
                    Text = "🎲 Objem",
                    Size = new Size(150, 40),
                    Location = new Point(15, y),
                    NormalColor = _bgLight,
                    SelectedColor = _accentPurple,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    IsSelected = (index == 0)
                };
                btnVolume.Click += (s, e) => SelectCalculationType(0);
                _calculationTypePanel.Controls.Add(btnVolume);
                _calculationTypeButtons.Add(btnVolume);
                y += 50;
                index++;

                // Tlačidlo pre povrch
                var btnSurface = new RoundedButton
                {
                    Text = "🔲 Povrch",
                    Size = new Size(150, 40),
                    Location = new Point(15, y),
                    NormalColor = _bgLight,
                    SelectedColor = _accentPurple,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    IsSelected = (index == 0)
                };
                btnSurface.Click += (s, e) => SelectCalculationType(1);
                _calculationTypePanel.Controls.Add(btnSurface);
                _calculationTypeButtons.Add(btnSurface);

                _calculationTypePanel.Height = 140;
            }

            _selectedCalculationType = 0;
        }

        /// <summary>
        /// Vyberie typ výpočtu.
        /// </summary>
        private void SelectCalculationType(int index)
        {
            // Zrušiť výber všetkých tlačidiel
            foreach (var btn in _calculationTypeButtons)
                btn.IsSelected = false;

            // Vybrať aktuálne tlačidlo
            if (index >= 0 && index < _calculationTypeButtons.Count)
            {
                _calculationTypeButtons[index].IsSelected = true;
                _selectedCalculationType = index;
            }
        }

        /// <summary>
        /// Načíta obrázok tvaru (generovaný programovo).
        /// </summary>
        private void LoadShapeImage(Shape shape)
        {
            // Vytvoriť jednoduchý obrázok tvaru programovo
            Bitmap bmp = new Bitmap(180, 180);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                using (Pen pen = new Pen(_accentBlue, 3))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(60, 100, 150, 200)))
                {
                    switch (shape.Name)
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
                            DrawCube(g, pen, brush);
                            break;
                            
                        case "Kváder":
                            DrawCuboid(g, pen, brush);
                            break;
                            
                        case "Guľa":
                            g.FillEllipse(brush, 25, 25, 130, 130);
                            g.DrawEllipse(pen, 25, 25, 130, 130);
                            g.DrawArc(new Pen(_accentBlue, 2), 35, 60, 110, 60, 0, 180);
                            break;
                            
                        case "Valec":
                            DrawCylinder(g, pen, brush);
                            break;
                            
                        case "Kužeľ":
                            DrawCone(g, pen, brush);
                            break;
                            
                        case "Ihlan":
                            DrawPyramid(g, pen, brush);
                            break;
                            
                        default:
                            g.FillRectangle(brush, 25, 25, 130, 130);
                            g.DrawRectangle(pen, 25, 25, 130, 130);
                            break;
                    }
                }
            }

            if (_shapeImage.Image != null)
                _shapeImage.Image.Dispose();
            _shapeImage.Image = bmp;
        }

        private void DrawCube(Graphics g, Pen pen, SolidBrush brush)
        {
            Point[] front = { new Point(30, 50), new Point(120, 50), new Point(120, 140), new Point(30, 140) };
            g.FillPolygon(brush, front);
            g.DrawPolygon(pen, front);
            
            Point[] top = { new Point(30, 50), new Point(70, 20), new Point(160, 20), new Point(120, 50) };
            g.FillPolygon(new SolidBrush(Color.FromArgb(80, 100, 150, 200)), top);
            g.DrawPolygon(pen, top);
            
            Point[] right = { new Point(120, 50), new Point(160, 20), new Point(160, 110), new Point(120, 140) };
            g.FillPolygon(new SolidBrush(Color.FromArgb(40, 100, 150, 200)), right);
            g.DrawPolygon(pen, right);
        }

        private void DrawCuboid(Graphics g, Pen pen, SolidBrush brush)
        {
            Point[] front = { new Point(20, 60), new Point(130, 60), new Point(130, 140), new Point(20, 140) };
            g.FillPolygon(brush, front);
            g.DrawPolygon(pen, front);
            
            Point[] top = { new Point(20, 60), new Point(60, 25), new Point(170, 25), new Point(130, 60) };
            g.FillPolygon(new SolidBrush(Color.FromArgb(80, 100, 150, 200)), top);
            g.DrawPolygon(pen, top);
            
            Point[] right = { new Point(130, 60), new Point(170, 25), new Point(170, 105), new Point(130, 140) };
            g.FillPolygon(new SolidBrush(Color.FromArgb(40, 100, 150, 200)), right);
            g.DrawPolygon(pen, right);
        }

        private void DrawCylinder(Graphics g, Pen pen, SolidBrush brush)
        {
            g.FillRectangle(brush, 40, 50, 100, 80);
            g.DrawLine(pen, 40, 50, 40, 130);
            g.DrawLine(pen, 140, 50, 140, 130);
            
            g.FillEllipse(new SolidBrush(Color.FromArgb(80, 100, 150, 200)), 40, 30, 100, 40);
            g.DrawEllipse(pen, 40, 30, 100, 40);
            
            g.DrawArc(pen, 40, 110, 100, 40, 0, 180);
        }

        private void DrawCone(Graphics g, Pen pen, SolidBrush brush)
        {
            Point[] cone = { new Point(90, 20), new Point(160, 140), new Point(20, 140) };
            g.FillPolygon(brush, cone);
            g.DrawLine(pen, 90, 20, 20, 140);
            g.DrawLine(pen, 90, 20, 160, 140);
            
            g.FillEllipse(new SolidBrush(Color.FromArgb(60, 100, 150, 200)), 20, 120, 140, 45);
            g.DrawEllipse(pen, 20, 120, 140, 45);
        }

        private void DrawPyramid(Graphics g, Pen pen, SolidBrush brush)
        {
            Point[] front = { new Point(90, 15), new Point(150, 130), new Point(30, 130) };
            g.FillPolygon(brush, front);
            g.DrawPolygon(pen, front);
            
            Point[] right = { new Point(90, 15), new Point(150, 130), new Point(130, 160), new Point(90, 95) };
            g.FillPolygon(new SolidBrush(Color.FromArgb(40, 100, 150, 200)), right);
            g.DrawLine(pen, 90, 15, 130, 160);
            g.DrawLine(pen, 150, 130, 130, 160);
            
            Point[] baseP = { new Point(30, 130), new Point(150, 130), new Point(130, 160), new Point(50, 160) };
            g.DrawPolygon(new Pen(_accentBlue, 2), baseP);
        }

        /// <summary>
        /// Spracuje kliknutie na tlačidlo Vypočítať.
        /// </summary>
        private void OnCalculateClick(object sender, EventArgs e)
        {
            if (_currentShape == null)
                return;

            // Získať hodnoty
            double[] parameters = new double[_inputFields.Count];
            
            for (int i = 0; i < _inputFields.Count; i++)
            {
                if (!InputValidator.TryParseNumber(_inputFields[i].Text, out parameters[i]))
                {
                    MessageBox.Show(
                        $"Neplatná hodnota v poli '{_currentShape.ParameterNames[i]}'.\n\n" +
                        "Zadajte kladné číslo (môžete použiť desatinnú čiarku alebo bodku).",
                        "Chyba vstupu",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    _inputFields[i].Focus();
                    _inputFields[i].SelectAll();
                    return;
                }
            }

            // Validovať parametre
            if (!_currentShape.ValidateParameters(parameters, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Chyba validácie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Vypočítať a zobraziť výsledok
            try
            {
                string result = _currentShape.Calculate(parameters, _selectedCalculationType);
                _resultTextBox.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Pri výpočte nastala chyba:\n\n{ex.Message}",
                    "Chyba výpočtu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Vyčistí vstupné polia a výsledky.
        /// </summary>
        private void OnClearClick(object sender, EventArgs e)
        {
            foreach (var field in _inputFields)
            {
                field.Clear();
            }
            _resultTextBox.Clear();

            if (_inputFields.Count > 0)
                _inputFields[0].Focus();
        }

        /// <summary>
        /// Exportuje výsledky do TXT súboru.
        /// </summary>
        private void OnExportClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_resultTextBox.Text))
            {
                MessageBox.Show(
                    "Najprv vykonajte výpočet.",
                    "Nie je čo exportovať",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Textové súbory (*.txt)|*.txt|Všetky súbory (*.*)|*.*";
                dialog.DefaultExt = "txt";
                dialog.FileName = $"GeoCalc_{_currentShape?.Name ?? "vysledok"}_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string[] calcTypeNames = _currentShape is Shape2D 
                            ? new[] { "Obvod", "Obsah" }
                            : new[] { "Objem", "Povrch" };

                        string content = $"GeoCalc - Export výsledkov\n" +
                                        $"Dátum: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\n" +
                                        $"Tvar: {_currentShape?.Name}\n" +
                                        $"Typ výpočtu: {calcTypeNames[_selectedCalculationType]}\n\n" +
                                        $"ZADANÉ HODNOTY:\n";

                        for (int i = 0; i < _inputFields.Count; i++)
                        {
                            content += $"  {_currentShape.ParameterNames[i]}: {_inputFields[i].Text}\n";
                        }

                        content += $"\n{_formulaTextBox.Text}\n\n";
                        content += _resultTextBox.Text;

                        File.WriteAllText(dialog.FileName, content);

                        MessageBox.Show(
                            $"Výsledky boli úspešne exportované do:\n{dialog.FileName}",
                            "Export dokončený",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Chyba pri ukladaní súboru:\n{ex.Message}",
                            "Chyba exportu",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Spracuje zmenu veľkosti okna.
        /// </summary>
        private void OnFormResize(object sender, EventArgs e)
        {
            // Dynamické prispôsobenie rozloženia
            if (this.Width < 1400)
            {
                _formulaPanel.Visible = false;
            }
            else
            {
                _formulaPanel.Visible = true;
            }
        }
    }
}
