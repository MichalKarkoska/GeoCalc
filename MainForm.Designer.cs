namespace GeoCalc
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.btnCalculate = new GeoCalc.Controls.RoundedButton();
            this.panelFormulaCard = new GeoCalc.Controls.RoundedPanel();
            this.txtFormula = new System.Windows.Forms.RichTextBox();
            this.panelOutputCard = new GeoCalc.Controls.RoundedPanel();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.panelInputCard = new GeoCalc.Controls.RoundedPanel();
            this.lblSelectedShape = new System.Windows.Forms.Label();
            this.panelSidebar.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelFormulaCard.SuspendLayout();
            this.panelOutputCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(35)))));
            this.panelSidebar.Controls.Add(this.lblTitle);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(220, 681);
            this.panelSidebar.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.lblTitle.Location = new System.Drawing.Point(12, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(196, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📐 GeoCalc";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(24)))));
            this.panelContent.Controls.Add(this.btnCalculate);
            this.panelContent.Controls.Add(this.panelFormulaCard);
            this.panelContent.Controls.Add(this.panelOutputCard);
            this.panelContent.Controls.Add(this.panelInputCard);
            this.panelContent.Controls.Add(this.lblSelectedShape);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(220, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(844, 681);
            this.panelContent.TabIndex = 1;
            // 
            // btnCalculate
            // 
            this.btnCalculate.BorderRadius = 12;
            this.btnCalculate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCalculate.FlatAppearance.BorderSize = 0;
            this.btnCalculate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalculate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCalculate.ForeColor = System.Drawing.Color.White;
            this.btnCalculate.IsSelected = false;
            this.btnCalculate.Location = new System.Drawing.Point(30, 355);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnCalculate.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(149)))), ((int)(((byte)(237)))));
            this.btnCalculate.Size = new System.Drawing.Size(380, 45);
            this.btnCalculate.TabIndex = 4;
            this.btnCalculate.Text = "VYPOČÍTAŤ";
            this.btnCalculate.UseVisualStyleBackColor = false;
            // 
            // panelFormulaCard
            // 
            this.panelFormulaCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.panelFormulaCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.panelFormulaCard.BorderRadius = 15;
            this.panelFormulaCard.BorderWidth = 1;
            this.panelFormulaCard.Controls.Add(this.txtFormula);
            this.panelFormulaCard.Location = new System.Drawing.Point(435, 80);
            this.panelFormulaCard.Name = "panelFormulaCard";
            this.panelFormulaCard.Size = new System.Drawing.Size(380, 570);
            this.panelFormulaCard.TabIndex = 3;
            // 
            // txtFormula
            // 
            this.txtFormula.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.txtFormula.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFormula.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtFormula.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(215)))), ((int)(((byte)(0)))));
            this.txtFormula.Location = new System.Drawing.Point(15, 15);
            this.txtFormula.Name = "txtFormula";
            this.txtFormula.ReadOnly = true;
            this.txtFormula.Size = new System.Drawing.Size(350, 540);
            this.txtFormula.TabIndex = 0;
            this.txtFormula.Text = "";
            // 
            // panelOutputCard
            // 
            this.panelOutputCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.panelOutputCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.panelOutputCard.BorderRadius = 15;
            this.panelOutputCard.BorderWidth = 1;
            this.panelOutputCard.Controls.Add(this.txtOutput);
            this.panelOutputCard.Location = new System.Drawing.Point(30, 410);
            this.panelOutputCard.Name = "panelOutputCard";
            this.panelOutputCard.Size = new System.Drawing.Size(380, 240);
            this.panelOutputCard.TabIndex = 2;
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.txtOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOutput.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(238)))), ((int)(((byte)(144)))));
            this.txtOutput.Location = new System.Drawing.Point(15, 15);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(350, 210);
            this.txtOutput.TabIndex = 0;
            this.txtOutput.Text = "";
            // 
            // panelInputCard
            // 
            this.panelInputCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.panelInputCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.panelInputCard.BorderRadius = 15;
            this.panelInputCard.BorderSize = 1;
            this.panelInputCard.Location = new System.Drawing.Point(30, 80);
            this.panelInputCard.Name = "panelInputCard";
            this.panelInputCard.Size = new System.Drawing.Size(380, 260);
            this.panelInputCard.TabIndex = 1;
            // 
            // lblSelectedShape
            // 
            this.lblSelectedShape.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblSelectedShape.ForeColor = System.Drawing.Color.White;
            this.lblSelectedShape.Location = new System.Drawing.Point(30, 20);
            this.lblSelectedShape.Name = "lblSelectedShape";
            this.lblSelectedShape.Size = new System.Drawing.Size(400, 40);
            this.lblSelectedShape.TabIndex = 0;
            this.lblSelectedShape.Text = "Vyberte geometrický tvar";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 681);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GeoCalc - Geometrická kalkulačka";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelSidebar.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.panelFormulaCard.ResumeLayout(false);
            this.panelOutputCard.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblSelectedShape;
        private GeoCalc.Controls.RoundedPanel panelInputCard;
        private GeoCalc.Controls.RoundedPanel panelOutputCard;
        private GeoCalc.Controls.RoundedPanel panelFormulaCard;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.RichTextBox txtFormula;
        private GeoCalc.Controls.RoundedButton btnCalculate;
    }
}