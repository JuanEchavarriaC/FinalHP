using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalHP
{
    public partial class FormMovimientos : Form
    {
        private Biblioteca.Biblioteca biblioteca;

        public FormMovimientos(Biblioteca.Biblioteca biblioteca)
        {
            InitializeComponent();
            this.biblioteca = biblioteca;
            CargarMovimientos();
        }

        private void CargarMovimientos()
        {
            // Lógica para cargar los movimientos en el DataGridView
            var movimientos = biblioteca.ObtenerMovimientos();
            dgvMovimientos.DataSource = movimientos;
        }
    }
}
