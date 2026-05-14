using System.Drawing;

namespace loginlast
{
    partial class ProductCard
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

        private void InitializeComponent()
        {
            this.pbPhoto = new System.Windows.Forms.PictureBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // pbPhoto
            this.pbPhoto.Location = new System.Drawing.Point(10, 10);
            this.pbPhoto.Name = "pbPhoto";
            this.pbPhoto.Size = new System.Drawing.Size(120, 120);
            this.pbPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // lblInfo
            this.lblInfo.AutoSize = false;
            this.lblInfo.Location = new System.Drawing.Point(140, 10);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(320, 120);
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);

            // lblDiscount
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Location = new System.Drawing.Point(480, 45);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(80, 40);
            this.lblDiscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblDiscount.ForeColor = Color.Red;
            this.lblDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pbPhoto);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblDiscount);
            this.Name = "ProductCard";
            this.Size = new System.Drawing.Size(580, 140);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.PictureBox pbPhoto;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblDiscount;
    }
}