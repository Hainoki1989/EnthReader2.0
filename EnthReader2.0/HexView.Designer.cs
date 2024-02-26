namespace EnthReader2._0
{
    partial class HexView
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
            this.t_hexView = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // t_hexView
            // 
            this.t_hexView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.t_hexView.Location = new System.Drawing.Point(0, 0);
            this.t_hexView.Multiline = true;
            this.t_hexView.Name = "t_hexView";
            this.t_hexView.Size = new System.Drawing.Size(634, 450);
            this.t_hexView.TabIndex = 0;
            this.t_hexView.TextChanged += new System.EventHandler(this.t_hexView_TextChanged);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 450);
            this.Controls.Add(this.t_hexView);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox t_hexView;
    }
}