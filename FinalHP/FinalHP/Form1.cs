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
        }

        private void btnRegistrarDevolucion_Click(object sender, EventArgs e)
        {
            // Lógica para registrar devolución
            string cedula = txtCedula.Text;
            string identificador = txtIdentificador.Text;

            biblioteca.RegistrarDevolucion(cedula, identificador);
            MessageBox.Show("Devolución registrada exitosamente.");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Guardar datos en archivos al cerrar el formulario
            biblioteca.GuardarMateriales("Materiales.txt");
            biblioteca.GuardarPersonas("Personas.txt");
        }
    }
}
