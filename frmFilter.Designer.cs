namespace MetaDataEditor
{
	partial class frmFilter
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
			this.cmbxFld1 = new System.Windows.Forms.ComboBox();
			this.lbl1 = new System.Windows.Forms.Label();
			this.cmbxLogic1 = new System.Windows.Forms.ComboBox();
			this.cmbxVal1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.pbCancel = new System.Windows.Forms.Button();
			this.pbOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cmbxFld1
			// 
			this.cmbxFld1.FormattingEnabled = true;
			this.cmbxFld1.Location = new System.Drawing.Point(12, 48);
			this.cmbxFld1.Name = "cmbxFld1";
			this.cmbxFld1.Size = new System.Drawing.Size(309, 24);
			this.cmbxFld1.TabIndex = 0;
			this.cmbxFld1.SelectedIndexChanged += new System.EventHandler(this.cmbxFld1_SelectedIndexChanged);
			// 
			// lbl1
			// 
			this.lbl1.AutoSize = true;
			this.lbl1.Location = new System.Drawing.Point(13, 25);
			this.lbl1.Name = "lbl1";
			this.lbl1.Size = new System.Drawing.Size(75, 17);
			this.lbl1.TabIndex = 1;
			this.lbl1.Text = "FieldName";
			// 
			// cmbxLogic1
			// 
			this.cmbxLogic1.FormattingEnabled = true;
			this.cmbxLogic1.Location = new System.Drawing.Point(354, 48);
			this.cmbxLogic1.Name = "cmbxLogic1";
			this.cmbxLogic1.Size = new System.Drawing.Size(53, 24);
			this.cmbxLogic1.TabIndex = 2;
			this.cmbxLogic1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// cmbxVal1
			// 
			this.cmbxVal1.FormattingEnabled = true;
			this.cmbxVal1.Location = new System.Drawing.Point(437, 46);
			this.cmbxVal1.Name = "cmbxVal1";
			this.cmbxVal1.Size = new System.Drawing.Size(279, 24);
			this.cmbxVal1.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(437, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 17);
			this.label1.TabIndex = 4;
			this.label1.Text = "Value";
			// 
			// pbCancel
			// 
			this.pbCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.pbCancel.Location = new System.Drawing.Point(213, 408);
			this.pbCancel.Name = "pbCancel";
			this.pbCancel.Size = new System.Drawing.Size(108, 25);
			this.pbCancel.TabIndex = 5;
			this.pbCancel.Text = "Cancel";
			this.pbCancel.UseVisualStyleBackColor = true;
			this.pbCancel.Click += new System.EventHandler(this.pbCancel_Click);
			// 
			// pbOK
			// 
			this.pbOK.Location = new System.Drawing.Point(452, 408);
			this.pbOK.Name = "pbOK";
			this.pbOK.Size = new System.Drawing.Size(75, 25);
			this.pbOK.TabIndex = 6;
			this.pbOK.Text = "Ok";
			this.pbOK.UseVisualStyleBackColor = true;
			this.pbOK.Click += new System.EventHandler(this.pbOK_Click);
			// 
			// frmFilter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.pbOK);
			this.Controls.Add(this.pbCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbxVal1);
			this.Controls.Add(this.cmbxLogic1);
			this.Controls.Add(this.lbl1);
			this.Controls.Add(this.cmbxFld1);
			this.Name = "frmFilter";
			this.Text = "Choose to fields and values to filter";
			this.Load += new System.EventHandler(this.frmFilter_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cmbxFld1;
		private System.Windows.Forms.Label lbl1;
		private System.Windows.Forms.ComboBox cmbxLogic1;
		private System.Windows.Forms.ComboBox cmbxVal1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button pbCancel;
		private System.Windows.Forms.Button pbOK;
	}
}