﻿namespace FinalHP
{
    partial class FormPersonas
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
            this.dgvPersonas = new System.Windows.Forms.DataGridView();
            this.dgvMovimientosPersona = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientosPersona)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPersonas
            // 
            this.dgvPersonas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPersonas.Location = new System.Drawing.Point(12, 12);
            this.dgvPersonas.Name = "dgvPersonas";
            this.dgvPersonas.RowHeadersWidth = 51;
            this.dgvPersonas.RowTemplate.Height = 24;
            this.dgvPersonas.Size = new System.Drawing.Size(690, 271);
            this.dgvPersonas.TabIndex = 0;
            // 
            // dgvMovimientosPersona
            // 
            this.dgvMovimientosPersona.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMovimientosPersona.Location = new System.Drawing.Point(12, 300);
            this.dgvMovimientosPersona.Name = "dgvMovimientosPersona";
            this.dgvMovimientosPersona.RowHeadersWidth = 51;
            this.dgvMovimientosPersona.RowTemplate.Height = 24;
            this.dgvMovimientosPersona.Size = new System.Drawing.Size(690, 331);
            this.dgvMovimientosPersona.TabIndex = 1;
            // 
            // FormPersonas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 640);
            this.Controls.Add(this.dgvMovimientosPersona);
            this.Controls.Add(this.dgvPersonas);
            this.Name = "FormPersonas";
            this.Text = "Personas";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientosPersona)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPersonas;
        private System.Windows.Forms.DataGridView dgvMovimientosPersona;
    }
}