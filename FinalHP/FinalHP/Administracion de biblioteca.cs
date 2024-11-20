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
        // Declarar la instancia de Biblioteca
        private Biblioteca.Biblioteca biblioteca;

        public Form1()
        {
            InitializeComponent();
            // Inicializar la instancia de Biblioteca
            biblioteca = new Biblioteca.Biblioteca();
            // Cargar datos desde archivos
            biblioteca.CargarMateriales("Materiales.txt");
            biblioteca.CargarPersonas("Personas.txt");
        }



        private void btnAgregarMaterial_Click(object sender, EventArgs e)
        {
            // Lógica para agregar material
            string identificador = txtIdentificador.Text;
            string titulo = txtTitulo.Text;
            DateTime fechaRegistro = dtpFechaRegistro.Value;
            int cantidadRegistrada = int.Parse(txtCantidadRegistrada.Text);
            int cantidadActual = int.Parse(txtCantidadActual.Text);

            Material material = new Material(identificador, titulo, fechaRegistro, cantidadRegistrada);
            biblioteca.AgregarMaterial(material);
            MessageBox.Show("Material agregado exitosamente.");

            // Actualizar los cuadros de texto para mostrar la cantidad
            txtCantidadRegistrada.Text = cantidadRegistrada.ToString();
            txtCantidadActual.Text = cantidadActual.ToString();
        }

        private void btnAgregarPersona_Click(object sender, EventArgs e)
        {
            // Lógica para agregar persona
            string cedula = txtCedula.Text;
            string nombre = txtNombre.Text;
            Rol rol = (Rol)Enum.Parse(typeof(Rol), cmbRol.SelectedItem.ToString());

            Persona persona = new Persona(nombre, cedula, rol);
            biblioteca.AgregarPersona(persona);
            MessageBox.Show("Persona agregada exitosamente.");
        }
        private void btnRegistrarPrestamo_Click(object sender, EventArgs e)
        {
            // Lógica para registrar préstamo
            string cedula = txtCedula.Text;
            string identificador = txtIdentificador.Text;

            biblioteca.RegistrarPrestamo(cedula, identificador);
            MessageBox.Show("Préstamo registrado exitosamente.");

            // Actualizar el cuadro de texto para mostrar la cantidad actual
            Material material = biblioteca.BuscarMaterial(identificador);
            txtCantidadActual.Text = material.CantidadActual.ToString();
        }
        private void btnRegistrarDevolucion_Click(object sender, EventArgs e)
        {
            // Lógica para registrar devolución
            string cedula = txtCedula.Text;
            string identificador = txtIdentificador.Text;

            biblioteca.RegistrarDevolucion(cedula, identificador);
            MessageBox.Show("Devolución registrada exitosamente.");

            // Actualizar el cuadro de texto para mostrar la cantidad actual
            Material material = biblioteca.BuscarMaterial(identificador);
            txtCantidadActual.Text = material.CantidadActual.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Guardar datos en archivos al cerrar el formulario
            biblioteca.GuardarMateriales("Materiales.txt");
            biblioteca.GuardarPersonas("Personas.txt");
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
    }
}
