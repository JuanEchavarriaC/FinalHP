using Biblioteca;
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
    public partial class Form1 : Form
    {
        private Biblioteca.Biblioteca biblioteca;

        public Form1()
        {
            InitializeComponent();
            biblioteca = new Biblioteca.Biblioteca();
            biblioteca.CargarMateriales("Materiales.txt");
            biblioteca.CargarPersonas("Personas.txt");
            biblioteca.CargarMovimientos("Movimientos.txt");
        }

        private void btnAgregarMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                string identificador = txtIdentificador.Text;
                string titulo = txtTitulo.Text;
                DateTime fechaRegistro = dtpFechaRegistro.Value;
                int cantidadRegistrada = int.Parse(txtCantidadRegistrada.Text);
                int cantidadActual = int.Parse(txtCantidadRegistrada.Text);

                Material material = new Material(identificador, titulo, fechaRegistro, cantidadRegistrada);
                biblioteca.AgregarMaterial(material);
                MessageBox.Show("Material agregado exitosamente.");

                txtCantidadRegistrada.Text = cantidadRegistrada.ToString();
                biblioteca.GuardarMateriales("Materiales.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al agregar el material: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarPersona_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCedula.Text))
                {
                    MessageBox.Show("Por favor, ingresa una cédula.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("Por favor, ingresa un nombre.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cmbRol.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, selecciona un rol.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Rol rol = (Rol)Enum.Parse(typeof(Rol), cmbRol.SelectedItem.ToString());

                string cedula = txtCedula.Text;
                string nombre = txtNombre.Text;
                Persona persona = new Persona(nombre, cedula, rol);
                biblioteca.AgregarPersona(persona);

                MessageBox.Show("Persona agregada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                biblioteca.GuardarPersonas("Personas.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al agregar la persona: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrarPrestamo_Click(object sender, EventArgs e)
        {
            string cedula = txtCedula.Text;
            string identificador = txtIdentificador.Text;
            int cantidad = (int)numCantidad.Value;

            try
            {
                biblioteca.RegistrarPrestamo(cedula, identificador, cantidad);
                biblioteca.GuardarPersonas("Personas.txt");
                MessageBox.Show("Préstamo registrado con éxito.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrarDevolucion_Click(object sender, EventArgs e)
        {
            string cedula = txtCedula.Text;
            string identificador = txtIdentificador.Text;
            int cantidad = (int)numCantidad.Value;

            if (!TienePrestamos(cedula))
            {
                MessageBox.Show("Este usuario no tiene préstamos activos. No puede realizar una devolución.");
                return;
            }

            try
            {
                biblioteca.RegistrarDevolucion(cedula, identificador, cantidad);
                biblioteca.GuardarPersonas("Personas.txt");
                MessageBox.Show("Devolución registrada con éxito.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            biblioteca.GuardarMateriales("Materiales.txt");
            biblioteca.GuardarPersonas("Personas.txt");
            biblioteca.GuardarMovimientos("Movimientos.txt");
        }

        private void Administra_Load(object sender, EventArgs e)
        {
        }

        private void btnVerMovimientos_Click(object sender, EventArgs e)
        {
            FormMovimientos formMovimientos = new FormMovimientos(biblioteca);
            formMovimientos.Show();
        }

        private void btnVerPersonas_Click(object sender, EventArgs e)
        {
            FormPersonas formPersonas = new FormPersonas(biblioteca);
            formPersonas.Show();
        }

        private void btnVerLibros_Click(object sender, EventArgs e)
        {
            FormLibros formLibros = new FormLibros(biblioteca);
            formLibros.Show();
        }

        private void btnEliminarPersona_Click(object sender, EventArgs e)
        {
            try
            {
                string cedula = txtCedula.Text;
                biblioteca.EliminarPersona(cedula);
                MessageBox.Show("Persona eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            biblioteca.GuardarPersonas("Personas.txt");
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                biblioteca.GuardarMateriales("Materiales.txt");
                biblioteca.GuardarPersonas("Personas.txt");
                MessageBox.Show("Datos guardados exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnConsultarLibro_Click(object sender, EventArgs e)
        {
            try
            {
                string identificador = txtIdentificador.Text;
                var material = biblioteca.BuscarMaterial(identificador);
                MessageBox.Show($"Título: {material.Titulo}\nCantidad Actual: {material.CantidadActual}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtIdentificador_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIdentificador.Text, out _))
            {
                MessageBox.Show("Por favor, ingrese solo números enteros en el Identificador.");
                txtIdentificador.Clear();
            }
        }

        private bool bloqueandoEvento = false;

        private void txtCedula_TextChanged(object sender, EventArgs e)
        {
            if (bloqueandoEvento) return;

            string textoActual = txtCedula.Text;

            if (string.IsNullOrWhiteSpace(textoActual))
                return;

            if (textoActual.Length > 15)
            {
                bloqueandoEvento = true;
                MessageBox.Show("La cédula no puede tener más de 15 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCedula.Clear();
                bloqueandoEvento = false;
                return;
            }

            if (!decimal.TryParse(textoActual, out _))
            {
                bloqueandoEvento = true;
                MessageBox.Show("Por favor, ingrese solo números enteros en la Cédula.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCedula.Clear();
                bloqueandoEvento = false;
            }
        }

        private bool TienePrestamos(string cedula)
        {
            var prestamosUsuario = biblioteca.ObtenerPrestamosPorCedula(cedula);
            return prestamosUsuario.Any();
        }
    }
}