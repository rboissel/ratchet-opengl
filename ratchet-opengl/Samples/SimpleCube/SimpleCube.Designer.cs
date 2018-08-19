namespace SimpleCube
{
    partial class SimpleCube
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.refresh = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // refresh
            // 
            this.refresh.Enabled = true;
            this.refresh.Interval = 20;
            this.refresh.Tick += new System.EventHandler(this.refresh_Tick);
            // 
            // SimpleCube
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 544);
            this.Name = "SimpleCube";
            this.Text = "Simple Cube";
            this.Load += new System.EventHandler(this.SimpleCube_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SimpleCube_Paint);
            this.Resize += new System.EventHandler(this.SimpleCube_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer refresh;
    }
}

