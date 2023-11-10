namespace ACadSvgStudio {
    partial class ConversionInfoForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._conversionLogScintilla = new ScintillaNET.Scintilla();
            this._conversionLoglabel = new System.Windows.Forms.Label();
            this._occurringEntitiesListBox = new System.Windows.Forms.ListBox();
            this._entitiesLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // _splitContainer
            // 
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.Location = new System.Drawing.Point(0, 0);
            this._splitContainer.Name = "_splitContainer";
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this._conversionLogScintilla);
            this._splitContainer.Panel1.Controls.Add(this._conversionLoglabel);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._occurringEntitiesListBox);
            this._splitContainer.Panel2.Controls.Add(this._entitiesLabel);
            this._splitContainer.Size = new System.Drawing.Size(946, 717);
            this._splitContainer.SplitterDistance = 623;
            this._splitContainer.TabIndex = 0;
            // 
            // _conversionLogScintilla
            // 
            this._conversionLogScintilla.AutoCMaxHeight = 9;
            this._conversionLogScintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this._conversionLogScintilla.Location = new System.Drawing.Point(0, 15);
            this._conversionLogScintilla.Name = "_conversionLogScintilla";
            this._conversionLogScintilla.Size = new System.Drawing.Size(623, 702);
            this._conversionLogScintilla.TabIndents = true;
            this._conversionLogScintilla.TabIndex = 1;
            this._conversionLogScintilla.Text = "scintilla1";
            // 
            // _conversionLoglabel
            // 
            this._conversionLoglabel.AutoSize = true;
            this._conversionLoglabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._conversionLoglabel.Location = new System.Drawing.Point(0, 0);
            this._conversionLoglabel.Name = "_conversionLoglabel";
            this._conversionLoglabel.Size = new System.Drawing.Size(90, 15);
            this._conversionLoglabel.TabIndex = 0;
            this._conversionLoglabel.Text = "Conversion Log";
            // 
            // _occurringEntitiesListBox
            // 
            this._occurringEntitiesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._occurringEntitiesListBox.FormattingEnabled = true;
            this._occurringEntitiesListBox.ItemHeight = 15;
            this._occurringEntitiesListBox.Location = new System.Drawing.Point(0, 15);
            this._occurringEntitiesListBox.Name = "_occurringEntitiesListBox";
            this._occurringEntitiesListBox.Size = new System.Drawing.Size(319, 702);
            this._occurringEntitiesListBox.TabIndex = 1;
            // 
            // _entitiesLabel
            // 
            this._entitiesLabel.AutoSize = true;
            this._entitiesLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._entitiesLabel.Location = new System.Drawing.Point(0, 0);
            this._entitiesLabel.Name = "_entitiesLabel";
            this._entitiesLabel.Size = new System.Drawing.Size(101, 15);
            this._entitiesLabel.TabIndex = 0;
            this._entitiesLabel.Text = "Occurring Entities";
            // 
            // ConversionInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 717);
            this.Controls.Add(this._splitContainer);
            this.Name = "ConversionInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Conversion Info:";
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel1.PerformLayout();
            this._splitContainer.Panel2.ResumeLayout(false);
            this._splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer _splitContainer;
        private ScintillaNET.Scintilla _conversionLogScintilla;
        private Label _conversionLoglabel;
        private ListBox _occurringEntitiesListBox;
        private Label _entitiesLabel;
    }
}