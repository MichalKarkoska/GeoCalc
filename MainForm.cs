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
    /// Hlavný formulár aplikácie GeoCalc.
    /// Obsahuje menu, vstupné polia a zobrazenie výsledkov.
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
        private PictureBox _shapeImage;
        private Label _titleLabel;
        private Label _subtitleLabel;
        private List<RoundedButton> _menuButtons;
        private List<TextBox> _inputFields;
        private List<Label> _inputLabels;
        private RichTextBox _resultTextBox;
        private RichTextBox _formulaTextBox;
        private RoundedButton _calculateButton;
        private RoundedButton _clearButton;
        private RoundedButton _exportButton;
        private ToolTip _toolTip;

        // Farby pre dark theme
        private readonly Color _bgDark = Color.FromArgb(30, 30, 35);
        private readonly Color _bgMedium = Color.FromArgb(40, 40, 48);
        private readonly Color _bgLight = Color.FromArgb(50, 50, 60);
        private readonly Color _accentBlue = Color.FromArgb(70, 130, 180);
        private readonly Color _accentGreen = Color.FromArgb(80, 180, 120);
        private readonly Color _textPrimary = Color.FromArgb(240, 240, 245);
        private readonly Color _textSecondary = Color.FromArgb(160, 160, 170);

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
            this.Size = new Size(1200, 750);
            this.MinimumSize = new Size(1000, 650);
            this.BackColor = _bgDark;
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Vytvorí a nastaví všetky UI komponenty.
        /// </summary>
        private void SetupUI()
        {
            // === ĽAVÝ PANEL MENU ===
            _menuPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = _bgMedium,
                Padding = new Padding(10)
            };
            this.Controls.Add(_menuPanel);

            // Logo/Nadpis v menu
            var logoLabel = new Label
            {
                Text = "📐 GeoCalc",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = _textPrimary,
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };
            _menuPanel.Controls.Add(logoLabel);

            // Separator
            var separator1 = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Color.FromArgb(70, 70, 80)
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

            int buttonY = 10;

            // Nadpis 2D tvary
            var label2D = new Label
            {
                Text = "2D TVARY",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = _textSecondary,
                Location = new Point(15, buttonY),
                AutoSize = true
            };
            menuScrollPanel.Controls.Add(label2D);
            buttonY += 30;

            // Tlačidlá pre 2D tvary
            foreach (var shape in _shapes2D)
            {
                var btn = CreateMenuButton(shape.Name, buttonY);
                btn.Tag = shape;
                btn.Click += OnShapeButtonClick;
                menuScrollPanel.Controls.Add(btn);
                _menuButtons.Add(btn);
                buttonY += 50;
            }

            buttonY += 15;

            // Nadpis 3D tvary
            var label3D = new Label
            {
                Text = "3D TVARY",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = _textSecondary,
                Location = new Point(15, buttonY),
                AutoSize = true
            };
            menuScrollPanel.Controls.Add(label3D);
            buttonY += 30;

            // Tlačidlá pre 3D tvary
            foreach (var shape in _shapes3D)
            {
                var btn = CreateMenuButton(shape.Name, buttonY);
                btn.Tag = shape;
                btn.Click += OnShapeButtonClick;
                menuScrollPanel.Controls.Add(btn);
                _menuButtons.Add(btn);
                buttonY += 50;
            }

            // === HLAVNÝ OBSAHOVÝ PANEL ===
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _bgDark,
                Padding = new Padding(20)
            };
            this.Controls.Add(_contentPanel);

            // Nadpis tvaru
            _titleLabel = new Label
            {
                Font = new Font("Segoe UI", 24f, FontStyle.Bold),
                ForeColor = _textPrimary,
                Location = new Point(20, 15),
                AutoSize = true
            };
            _contentPanel.Controls.Add(_titleLabel);

            // Podnadpis
            _subtitleLabel = new Label
            {
                Font = new Font("Segoe UI", 11f),
                ForeColor = _textSecondary,
                Location = new Point(22, 55),
                AutoSize = true
            };
            _contentPanel.Controls.Add(_subtitleLabel);

            // === PANEL S OBRÁZKOM TVARU ===
            _shapeImage = new PictureBox
            {
                Size = new Size(150, 150),
                Location = new Point(20, 90),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            _contentPanel.Controls.Add(_shapeImage);

            // === PANEL VSTUPOV ===
            _inputPanel = new RoundedPanel
            {
                BackColor = _bgMedium,
                Location = new Point(190, 90),
                Size = new Size(320, 280),
                BorderRadius = 15
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

            // Tlačidlá
            _calculateButton = new RoundedButton
            {
                Text = "🔢 Vypočítať",
                Size = new Size(135, 40),
                Location = new Point(15, 230),
                NormalColor = _accentBlue,
                SelectedColor = _accentBlue
            };
            _calculateButton.Click += OnCalculateClick;
            _inputPanel.Controls.Add(_calculateButton);

            _clearButton = new RoundedButton
            {
                Text = "🗑️ Vyčistiť",
                Size = new Size(135, 40),
                Location = new Point(165, 230)
            };
            _clearButton.Click += OnClearClick;
            _inputPanel.Controls.Add(_clearButton);

            // === PANEL VZORCOV ===
            _formulaPanel = new RoundedPanel
            {
                BackColor = _bgMedium,
                Location = new Point(530, 90),
                Size = new Size(400, 150),
                BorderRadius = 15
            };
            _contentPanel.Controls.Add(_formulaPanel);

            var formulaTitle = new Label
            {
                Text = "📖 Vzorce",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = _textPrimary,
                Location = new Point(15, 10),
                AutoSize = true
            };
            _formulaPanel.Controls.Add(formulaTitle);

            _formulaTextBox = new RichTextBox
            {
                Location = new Point(15, 40),
                Size = new Size(370, 100),
                BackColor = _bgLight,
                ForeColor = _textPrimary,
                Font = new Font("Consolas", 9f),
                ReadOnly = true,
                BorderStyle = BorderStyle.None
            };
            _formulaPanel.Controls.Add(_formulaTextBox);

            // === PANEL VÝSLEDKOV ===
            _resultPanel = new RoundedPanel
            {
                BackColor = _bgMedium,
                Location = new Point(530, 260),
                Size = new Size(400, 180),
                BorderRadius = 15
            };
            _contentPanel.Controls.Add(_resultPanel);

            var resultTitle = new Label
            {
                Text = "📊 Výsledky",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = _textPrimary,
                Location = new Point(15, 10),
                AutoSize = true
            };
            _resultPanel.Controls.Add(resultTitle);

            _resultTextBox = new RichTextBox
            {
                Location = new Point(15, 40),
                Size = new Size(370, 90),
                BackColor = _bgLight,
                ForeColor = _accentGreen,
                Font = new Font("Consolas", 10f),
                ReadOnly = true,
                BorderStyle = BorderStyle.None
            };
            _resultPanel.Controls.Add(_resultTextBox);

            _exportButton = new RoundedButton
            {
                Text = "💾 Exportovať do TXT",
                Size = new Size(180, 35),
                Location = new Point(200, 138),
                NormalColor = Color.FromArgb(70, 100, 80)
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
                Size = new Size(190, 42),
                Location = new Point(10, y),
                Font = new Font("Segoe UI", 10f)
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
                _subtitleLabel.Text = "2D tvar • Obsah a obvod";
            else
                _subtitleLabel.Text = "3D tvar • Objem a povrch";

            // Aktualizovať vzorce
            _formulaTextBox.Text = shape.GetFormulas();

            // Vyčistiť výsledky
            _resultTextBox.Clear();

            // Vytvoriť vstupné polia
            CreateInputFields(shape);

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
                    Font = new Font("Segoe UI", 10f),
                    ForeColor = _textPrimary,
                    Location = new Point(15, y + 3),
                    AutoSize = true
                };
                _inputPanel.Controls.Add(label);
                _inputLabels.Add(label);

                var textBox = new TextBox
                {
                    Size = new Size(130, 30),
                    Location = new Point(165, y),
                    BackColor = _bgLight,
                    ForeColor = _textPrimary,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Segoe UI", 11f)
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

                y += 40;
            }

            // Posunúť tlačidlá ak je veľa parametrov
            int buttonY = Math.Max(230, y + 20);
            _calculateButton.Location = new Point(15, buttonY);
            _clearButton.Location = new Point(165, buttonY);
            
            // Upraviť výšku panelu
            _inputPanel.Height = buttonY + 55;
        }

        /// <summary>
        /// Načíta obrázok tvaru (generovaný programovo).
        /// </summary>
        private void LoadShapeImage(Shape shape)
        {
            // Vytvoriť jednoduchý obrázok tvaru programovo
            Bitmap bmp = new Bitmap(150, 150);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                using (Pen pen = new Pen(_accentBlue, 3))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(50, 70, 130, 180)))
                {
                    switch (shape.Name)
                    {
                        case "Kruh":
                            g.FillEllipse(brush, 25, 25, 100, 100);
                            g.DrawEllipse(pen, 25, 25, 100, 100);
                            break;
                            
                        case "Obdĺžnik":
                            g.FillRectangle(brush, 20, 40, 110, 70);
                            g.DrawRectangle(pen, 20, 40, 110, 70);
                            break;
                            
                        case "Štvorec":
                            g.FillRectangle(brush, 30, 30, 90, 90);
                            g.DrawRectangle(pen, 30, 30, 90, 90);
                            break;
                            
                        case "Trojuholník":
                            Point[] triangle = { new Point(75, 20), new Point(130, 120), new Point(20, 120) };
                            g.FillPolygon(brush, triangle);
                            g.DrawPolygon(pen, triangle);
                            break;
                            
                        case "Lichobežník":
                            Point[] trapezoid = { new Point(40, 30), new Point(110, 30), new Point(130, 110), new Point(20, 110) };
                            g.FillPolygon(brush, trapezoid);
                            g.DrawPolygon(pen, trapezoid);
                            break;
                            
                        case "Kosoštvorec":
                            Point[] rhombus = { new Point(75, 15), new Point(135, 75), new Point(75, 135), new Point(15, 75) };
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
                            g.FillEllipse(brush, 25, 25, 100, 100);
                            g.DrawEllipse(pen, 25, 25, 100, 100);
                            g.DrawArc(new Pen(_accentBlue, 1), 35, 50, 80, 50, 0, 180);
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
                            g.FillRectangle(brush, 25, 25, 100, 100);
                            g.DrawRectangle(pen, 25, 25, 100, 100);
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
            // Predná stena
            Point[] front = { new Point(30, 50), new Point(100, 50), new Point(100, 120), new Point(30, 120) };
            g.FillPolygon(brush, front);
            g.DrawPolygon(pen, front);
            
            // Horná stena
            Point[] top = { new Point(30, 50), new Point(60, 25), new Point(130, 25), new Point(100, 50) };
            g.FillPolygon(new SolidBrush(Color.FromArgb(70, 70, 130, 180)), top);
            g.DrawPolygon(pen, top);
            
            // Pravá stena
            Point[] right = { new Point(100, 50), new Point(130, 25), new Point(130, 95), new Point(100, 120) };
            g.FillPolygon(new SolidBrush(Color.FromArgb(30, 70, 130, 180)), right);
            g.DrawPolygon(pen, right);
        }

        private void DrawCuboid(Graphics g, Pen pen, SolidBrush brush)
        {
            Point[] front = { new Point(20, 55), new Point(100, 55), new Point(100, 115), new Point(20, 115) };
            g.FillPolygon(brush, front);
            g.DrawPolygon(pen, front);
            
            Point[] top = { new Point(20, 55), new Point(55, 30), new Point(135, 30), new Point(100, 55) };
            g.FillPolygon(new SolidBrush(Color.FromArgb(70, 70, 130, 180)), top);
            g.DrawPolygon(pen, top);
            
            Point[] right = { new Point(100, 55), new Point(135, 30), new Point(135, 90), new Point(100, 115) };
            g.FillPolygon(new SolidBrush(Color.FromArgb(30, 70, 130, 180)), right);
            g.DrawPolygon(pen, right);
        }

        private void DrawCylinder(Graphics g, Pen pen, SolidBrush brush)
        {
            // Telo
            g.FillRectangle(brush, 35, 45, 80, 70);
            g.DrawLine(pen, 35, 45, 35, 115);
            g.DrawLine(pen, 115, 45, 115, 115);
            
            // Horná elipsa
            g.FillEllipse(new SolidBrush(Color.FromArgb(70, 70, 130, 180)), 35, 30, 80, 30);
            g.DrawEllipse(pen, 35, 30, 80, 30);
            
            // Dolná elipsa
            g.DrawArc(pen, 35, 100, 80, 30, 0, 180);
        }

        private void DrawCone(Graphics g, Pen pen, SolidBrush brush)
        {
            // Telo
            Point[] cone = { new Point(75, 25), new Point(125, 110), new Point(25, 110) };
            g.FillPolygon(brush, cone);
            g.DrawLine(pen, 75, 25, 25, 110);
            g.DrawLine(pen, 75, 25, 125, 110);
            
            // Základňa
            g.FillEllipse(new SolidBrush(Color.FromArgb(50, 70, 130, 180)), 25, 95, 100, 30);
            g.DrawEllipse(pen, 25, 95, 100, 30);
        }

        private void DrawPyramid(Graphics g, Pen pen, SolidBrush brush)
        {
            // Predná stena
            Point[] front = { new Point(75, 20), new Point(120, 100), new Point(30, 100) };
            g.FillPolygon(brush, front);
            g.DrawPolygon(pen, front);
            
            // Pravá stena
            Point[] right = { new Point(75, 20), new Point(120, 100), new Point(100, 120), new Point(75, 85) };
            g.FillPolygon(new SolidBrush(Color.FromArgb(30, 70, 130, 180)), right);
            g.DrawLine(pen, 75, 20, 100, 120);
            g.DrawLine(pen, 120, 100, 100, 120);
            
            // Základňa
            Point[] baseP = { new Point(30, 100), new Point(120, 100), new Point(100, 120), new Point(50, 120) };
            g.DrawPolygon(new Pen(_accentBlue, 1), baseP);
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
                string result = _currentShape.Calculate(parameters);
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
                        string content = $"GeoCalc - Export výsledkov\n" +
                                        $"Dátum: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\n" +
                                        $"Tvar: {_currentShape?.Name}\n\n" +
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
            // Upraviť pozície panelov pri resize
            int availableWidth = _contentPanel.Width - 40;
            int panelWidth = Math.Min(420, (availableWidth - _inputPanel.Right - 20));

            if (panelWidth > 250)
            {
                _formulaPanel.Width = panelWidth;
                _resultPanel.Width = panelWidth;
                _formulaTextBox.Width = panelWidth - 30;
                _resultTextBox.Width = panelWidth - 30;
                
                int newX = _contentPanel.Width - panelWidth - 20;
                _formulaPanel.Location = new Point(newX, _formulaPanel.Top);
                _resultPanel.Location = new Point(newX, _resultPanel.Top);
                
                _exportButton.Location = new Point(panelWidth - 195, _exportButton.Top);
            }
        }
    }
}