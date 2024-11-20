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
    public partial class FormLibros : Form
    {
        private Biblioteca.Biblioteca biblioteca;

        public FormLibros(Biblioteca.Biblioteca biblioteca)
        {
            InitializeComponent();
            this.biblioteca = biblioteca;
            CargarLibros();
        }

        private void CargarLibros()
        {
            // Lógica para cargar los libros en el DataGridView
            var libros = biblioteca.ObtenerLibros();
            dgvLibros.DataSource = libros;
        }
    }
}
