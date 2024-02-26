namespace EnthReader2._0
{
    partial class Form1
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
            this.t_hexDisplay = new System.Windows.Forms.TextBox();
            this.c_MeshBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.t_LODDisplay = new System.Windows.Forms.TreeView();
            this.b_LoadFile = new System.Windows.Forms.Button();
            this.b_ExportOBJ = new System.Windows.Forms.Button();
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
            this.splitContainer1.Panel1.Controls.Add(this.t_LODDisplay);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.c_MeshBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.t_hexDisplay);
            this.splitContainer1.Size = new System.Drawing.Size(845, 524);
            this.splitContainer1.SplitterDistance = 229;
            this.splitContainer1.TabIndex = 0;
            // 
            // t_hexDisplay
            // 
            this.t_hexDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.t_hexDisplay.Location = new System.Drawing.Point(0, 0);
            this.t_hexDisplay.Multiline = true;
            this.t_hexDisplay.Name = "t_hexDisplay";
            this.t_hexDisplay.Size = new System.Drawing.Size(612, 524);
            this.t_hexDisplay.TabIndex = 0;
            // 
            // c_MeshBox
            // 
            this.c_MeshBox.FormattingEnabled = true;
            this.c_MeshBox.Location = new System.Drawing.Point(86, 10);
            this.c_MeshBox.Name = "c_MeshBox";
            this.c_MeshBox.Size = new System.Drawing.Size(115, 21);
            this.c_MeshBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Vertex Block";
            // 
            // t_LODDisplay
            // 
            this.t_LODDisplay.Location = new System.Drawing.Point(16, 37);
            this.t_LODDisplay.Name = "t_LODDisplay";
            this.t_LODDisplay.Size = new System.Drawing.Size(185, 190);
            this.t_LODDisplay.TabIndex = 2;
            this.t_LODDisplay.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.t_LODDisplay_AfterSelect);
            // 
            // b_LoadFile
            // 
            this.b_LoadFile.Location = new System.Drawing.Point(16, 234);
            this.b_LoadFile.Name = "b_LoadFile";
            this.b_LoadFile.Size = new System.Drawing.Size(185, 23);
            this.b_LoadFile.TabIndex = 3;
            this.b_LoadFile.Text = "Load File";
            this.b_LoadFile.UseVisualStyleBackColor = true;
            this.b_LoadFile.Click += new System.EventHandler(this.b_LoadFile_Click);
            // 
            // b_ExportOBJ
            // 
            this.b_ExportOBJ.Location = new System.Drawing.Point(16, 264);
            this.b_ExportOBJ.Name = "b_ExportOBJ";
            this.b_ExportOBJ.Size = new System.Drawing.Size(185, 23);
            this.b_ExportOBJ.TabIndex = 4;
            this.b_ExportOBJ.Text = "Export OBJ";
            this.b_ExportOBJ.UseVisualStyleBackColor = true;
            this.b_ExportOBJ.Click += new System.EventHandler(this.b_ExportOBJ_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 524);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button b_ExportOBJ;
        private System.Windows.Forms.Button b_LoadFile;
        private System.Windows.Forms.TreeView t_LODDisplay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox c_MeshBox;
        private System.Windows.Forms.TextBox t_hexDisplay;
    }
}

