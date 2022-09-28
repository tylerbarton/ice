namespace InstrumentMonitor
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTickerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearTickersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeDebugModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanelSymbols = new System.Windows.Forms.FlowLayoutPanel();
            this.labelNoSymbols = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.flowLayoutPanelSymbols.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(36)))), ((int)(((byte)(41)))));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(2348, 56);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTickerToolStripMenuItem,
            this.clearTickersToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(103, 52);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // addTickerToolStripMenuItem
            // 
            this.addTickerToolStripMenuItem.Name = "addTickerToolStripMenuItem";
            this.addTickerToolStripMenuItem.Size = new System.Drawing.Size(538, 66);
            this.addTickerToolStripMenuItem.Text = "Add Ticker";
            this.addTickerToolStripMenuItem.Click += new System.EventHandler(this.addTickerToolStripMenuItem_Click);
            // 
            // clearTickersToolStripMenuItem
            // 
            this.clearTickersToolStripMenuItem.Name = "clearTickersToolStripMenuItem";
            this.clearTickersToolStripMenuItem.Size = new System.Drawing.Size(538, 66);
            this.clearTickersToolStripMenuItem.Text = "Clear Tickers";
            this.clearTickersToolStripMenuItem.Click += new System.EventHandler(this.clearTickersToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourcesToolStripMenuItem});
            this.editToolStripMenuItem.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(108, 52);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // sourcesToolStripMenuItem
            // 
            this.sourcesToolStripMenuItem.Name = "sourcesToolStripMenuItem";
            this.sourcesToolStripMenuItem.Size = new System.Drawing.Size(342, 66);
            this.sourcesToolStripMenuItem.Text = "Sources";
            this.sourcesToolStripMenuItem.Click += new System.EventHandler(this.sourcesToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.executeDebugModeToolStripMenuItem});
            this.helpToolStripMenuItem.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(123, 52);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(559, 66);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // executeDebugModeToolStripMenuItem
            // 
            this.executeDebugModeToolStripMenuItem.Name = "executeDebugModeToolStripMenuItem";
            this.executeDebugModeToolStripMenuItem.Size = new System.Drawing.Size(559, 66);
            this.executeDebugModeToolStripMenuItem.Text = "Execute Debug Mode";
            this.executeDebugModeToolStripMenuItem.Click += new System.EventHandler(this.executeDebugModeToolStripMenuItem_Click);
            // 
            // flowLayoutPanelSymbols
            // 
            this.flowLayoutPanelSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanelSymbols.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(36)))), ((int)(((byte)(41)))));
            this.flowLayoutPanelSymbols.Controls.Add(this.labelNoSymbols);
            this.flowLayoutPanelSymbols.Location = new System.Drawing.Point(12, 59);
            this.flowLayoutPanelSymbols.Name = "flowLayoutPanelSymbols";
            this.flowLayoutPanelSymbols.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.flowLayoutPanelSymbols.Size = new System.Drawing.Size(679, 1084);
            this.flowLayoutPanelSymbols.TabIndex = 1;
            this.flowLayoutPanelSymbols.DoubleClick += new System.EventHandler(this.flowLayoutPanelSymbols_DoubleClick);
            // 
            // labelNoSymbols
            // 
            this.labelNoSymbols.AutoSize = true;
            this.labelNoSymbols.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.labelNoSymbols.Location = new System.Drawing.Point(3, 0);
            this.labelNoSymbols.Name = "labelNoSymbols";
            this.labelNoSymbols.Size = new System.Drawing.Size(668, 48);
            this.labelNoSymbols.TabIndex = 2;
            this.labelNoSymbols.Text = "Double-click the symbol panel to begin...";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(697, 59);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1639, 1075);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(20F, 48F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(2348, 1146);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.flowLayoutPanelSymbols);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "ICE Instrument Monitor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.flowLayoutPanelSymbols.ResumeLayout(false);
            this.flowLayoutPanelSymbols.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem addTickerToolStripMenuItem;
        private ToolStripMenuItem clearTickersToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private FlowLayoutPanel flowLayoutPanelSymbols;
        private Label labelNoSymbols;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private PictureBox pictureBox1;
        private ToolStripMenuItem sourcesToolStripMenuItem;
        private ToolStripMenuItem executeDebugModeToolStripMenuItem;
    }
}