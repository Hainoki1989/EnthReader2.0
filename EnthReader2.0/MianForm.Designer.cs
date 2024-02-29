namespace EnthReader2._0
{
    partial class MianForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.b_ExportOBJ = new System.Windows.Forms.Button();
            this.b_LoadFile = new System.Windows.Forms.Button();
            this.t_LODDisplay = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.b_ExportOBJ);
            this.splitContainer1.Panel1.Controls.Add(this.b_LoadFile);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.t_LODDisplay);
            this.splitContainer1.Size = new System.Drawing.Size(845, 524);
            this.splitContainer1.SplitterDistance = 229;
            this.splitContainer1.TabIndex = 0;
            // 
            // b_ExportOBJ
            // 
            this.b_ExportOBJ.Location = new System.Drawing.Point(12, 41);
            this.b_ExportOBJ.Name = "b_ExportOBJ";
            this.b_ExportOBJ.Size = new System.Drawing.Size(185, 23);
            this.b_ExportOBJ.TabIndex = 4;
            this.b_ExportOBJ.Text = "Export OBJ";
            this.b_ExportOBJ.UseVisualStyleBackColor = true;
            this.b_ExportOBJ.Click += new System.EventHandler(this.b_ExportOBJ_Click);
            // 
            // b_LoadFile
            // 
            this.b_LoadFile.Location = new System.Drawing.Point(12, 12);
            this.b_LoadFile.Name = "b_LoadFile";
            this.b_LoadFile.Size = new System.Drawing.Size(185, 23);
            this.b_LoadFile.TabIndex = 3;
            this.b_LoadFile.Text = "Load File";
            this.b_LoadFile.UseVisualStyleBackColor = true;
            this.b_LoadFile.Click += new System.EventHandler(this.b_LoadFile_Click);
            // 
            // t_LODDisplay
            // 
            this.t_LODDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.t_LODDisplay.Location = new System.Drawing.Point(0, 0);
            this.t_LODDisplay.Name = "t_LODDisplay";
            this.t_LODDisplay.Size = new System.Drawing.Size(612, 524);
            this.t_LODDisplay.TabIndex = 2;
            this.t_LODDisplay.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.t_LODDisplay_AfterSelect);
            // 
            // MianForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 524);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MianForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button b_ExportOBJ;
        private System.Windows.Forms.Button b_LoadFile;
        private System.Windows.Forms.TreeView t_LODDisplay;
    }
}

