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
    public partial class FormPersonas : Form
    {
        private Biblioteca.Biblioteca biblioteca;

        public FormPersonas(Biblioteca.Biblioteca biblioteca)
        {
            InitializeComponent();
            this.biblioteca = biblioteca;
            CargarPersonas();
        }

        private void CargarPersonas()
        {
            // Lógica para cargar las personas en el DataGridView
            var personas = biblioteca.ObtenerPersonas();
            dgvPersonas.DataSource = personas;
        }

        private void dgvPersonas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPersonas.SelectedRows.Count > 0)
            {
                string cedula = dgvPersonas.SelectedRows[0].Cells["Cedula"].Value.ToString();
                var movimientos = biblioteca.ObtenerMovimientosPorPersona(cedula);
                dgvMovimientosPersona.DataSource = movimientos;
            }
        }
    }
}
