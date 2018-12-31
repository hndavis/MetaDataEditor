namespace MetaDataEditor
{
	partial class frmSort
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
			this.lstAvailFlds = new System.Windows.Forms.ListBox();
			this.lstSortFlds = new System.Windows.Forms.ListBox();
			this.pbMoveRight = new System.Windows.Forms.Button();
			this.pbRemove = new System.Windows.Forms.Button();
			this.pbOk = new System.Windows.Forms.Button();
			this.pbCancel = new System.Windows.Forms.Button();
			this.pbUp = new System.Windows.Forms.Button();
			this.pbDown = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lstAvailFlds
			// 
			this.lstAvailFlds.FormattingEnabled = true;
			this.lstAvailFlds.ItemHeight = 16;
			this.lstAvailFlds.Location = new System.Drawing.Point(74, 51);
			this.lstAvailFlds.Name = "lstAvailFlds";
			this.lstAvailFlds.Size = new System.Drawing.Size(217, 180);
			this.lstAvailFlds.TabIndex = 0;
			// 
			// lstSortFlds
			// 
			this.lstSortFlds.FormattingEnabled = true;
			this.lstSortFlds.ItemHeight = 16;
			this.lstSortFlds.Location = new System.Drawing.Point(377, 51);
			this.lstSortFlds.Name = "lstSortFlds";
			this.lstSortFlds.Size = new System.Drawing.Size(212, 180);
			this.lstSortFlds.TabIndex = 1;
			// 
			// pbMoveRight
			// 
			this.pbMoveRight.Location = new System.Drawing.Point(310, 87);
			this.pbMoveRight.Name = "pbMoveRight";
			this.pbMoveRight.Size = new System.Drawing.Size(37, 23);
			this.pbMoveRight.TabIndex = 2;
			this.pbMoveRight.Text = ">>";
			this.pbMoveRight.UseVisualStyleBackColor = true;
			this.pbMoveRight.Click += new System.EventHandler(this.pbMoveRight_Click);
			// 
			// pbRemove
			// 
			this.pbRemove.Location = new System.Drawing.Point(310, 135);
			this.pbRemove.Name = "pbRemove";
			this.pbRemove.Size = new System.Drawing.Size(37, 23);
			this.pbRemove.TabIndex = 3;
			this.pbRemove.Text = "<<";
			this.pbRemove.UseVisualStyleBackColor = true;
			this.pbRemove.Click += new System.EventHandler(this.pbRemove_Click);
			// 
			// pbOk
			// 
			this.pbOk.Location = new System.Drawing.Point(340, 250);
			this.pbOk.Name = "pbOk";
			this.pbOk.Size = new System.Drawing.Size(75, 23);
			this.pbOk.TabIndex = 4;
			this.pbOk.Text = "Ok";
			this.pbOk.UseVisualStyleBackColor = true;
			this.pbOk.Click += new System.EventHandler(this.pbOk_Click);
			// 
			// pbCancel
			// 
			this.pbCancel.Location = new System.Drawing.Point(215, 250);
			this.pbCancel.Name = "pbCancel";
			this.pbCancel.Size = new System.Drawing.Size(75, 23);
			this.pbCancel.TabIndex = 5;
			this.pbCancel.Text = "Cancel";
			this.pbCancel.UseVisualStyleBackColor = true;
			this.pbCancel.Click += new System.EventHandler(this.pbCancel_Click);
			// 
			// pbUp
			// 
			this.pbUp.Location = new System.Drawing.Point(613, 87);
			this.pbUp.Name = "pbUp";
			this.pbUp.Size = new System.Drawing.Size(57, 31);
			this.pbUp.TabIndex = 6;
			this.pbUp.Text = "Up";
			this.pbUp.UseVisualStyleBackColor = true;
			this.pbUp.Click += new System.EventHandler(this.pbUp_Click);
			// 
			// pbDown
			// 
			this.pbDown.Location = new System.Drawing.Point(613, 134);
			this.pbDown.Name = "pbDown";
			this.pbDown.Size = new System.Drawing.Size(57, 35);
			this.pbDown.TabIndex = 7;
			this.pbDown.Text = "Down";
			this.pbDown.UseVisualStyleBackColor = true;
			this.pbDown.Click += new System.EventHandler(this.button1_Click);
			// 
			// frmSort
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(721, 293);
			this.Controls.Add(this.pbDown);
			this.Controls.Add(this.pbUp);
			this.Controls.Add(this.pbCancel);
			this.Controls.Add(this.pbOk);
			this.Controls.Add(this.pbRemove);
			this.Controls.Add(this.pbMoveRight);
			this.Controls.Add(this.lstSortFlds);
			this.Controls.Add(this.lstAvailFlds);
			this.Name = "frmSort";
			this.Text = "Choose Sort Fields";
			this.Load += new System.EventHandler(this.frmSort_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lstAvailFlds;
		private System.Windows.Forms.ListBox lstSortFlds;
		private System.Windows.Forms.Button pbMoveRight;
		private System.Windows.Forms.Button pbRemove;
		private System.Windows.Forms.Button pbOk;
		private System.Windows.Forms.Button pbCancel;
		private System.Windows.Forms.Button pbUp;
		private System.Windows.Forms.Button pbDown;
	}
}