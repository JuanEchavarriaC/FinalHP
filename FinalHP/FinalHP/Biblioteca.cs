using FinalHP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biblioteca
{
    public enum Rol
    {
        Estudiante,
        Profesor,
        Administrativo
    }
        class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Biblioteca biblioteca = new Biblioteca();

            biblioteca.CargarMateriales("Materiales.txt");
            biblioteca.CargarPersonas("Personas.txt");
        }
    }

    public class Material
    {
        public string Identificador { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int CantidadRegistrada { get; set; }
        public int CantidadActual { get; set; }

        public Material(string identificador, string titulo, DateTime fechaRegistro, int cantidadRegistrada)
        {
            Identificador = identificador;
            Titulo = titulo;
            FechaRegistro = fechaRegistro;
            CantidadRegistrada = cantidadRegistrada;
            CantidadActual = cantidadRegistrada;
        }

        public bool Prestar()
        {
            if (CantidadActual > 0)
            {
                CantidadActual--;
                return true;
            }
            return false;
        }

        public void Devolver()
        {
            CantidadActual++;
        }

        public string ToCsv()
        {
            return $"{Identificador},{Titulo},{FechaRegistro},{CantidadRegistrada},{CantidadActual}";
        }

        public static Material FromCsv(string csvLine)
        {
            try
            {
                string[] values = csvLine.Split(',');

                // Verificar que la línea tiene el número correcto de columnas
                if (values.Length != 5)
                {
                    throw new FormatException("La línea CSV no contiene el número correcto de campos.");
                }

                // Intentar parsear la fecha con TryParseExact (puedes ajustar el formato según sea necesario)
                string fechaString = values[2].Trim(); // Elimina espacios al principio o final
                DateTime fechaRegistro;
                string[] formatosFecha = { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" }; // Especificar posibles formatos de fecha

                if (!DateTime.TryParseExact(fechaString, formatosFecha, null, System.Globalization.DateTimeStyles.None, out fechaRegistro))
                {
                    throw new FormatException($"El campo de fecha '{fechaString}' no tiene un formato válido.");
                }

                // Intentar parsear las cantidades
                int cantidadRegistrada;
                if (!int.TryParse(values[3], out cantidadRegistrada))
                {
                    throw new FormatException($"El campo de cantidad registrada '{values[3]}' no tiene un formato válido.");
                }

                int cantidadActual;
                if (!int.TryParse(values[4], out cantidadActual))
                {
                    throw new FormatException($"El campo de cantidad actual '{values[4]}' no tiene un formato válido.");
                }

                // Crear y devolver el objeto Material
                return new Material(
                    values[0],
                    values[1],
                    fechaRegistro,
                    cantidadRegistrada
                )
                {
                    CantidadActual = cantidadActual
                };
            }
            catch (FormatException ex)
            {
                // Mostrar un mensaje de error adecuado y manejar la excepción
                Console.WriteLine($"Error al procesar la línea CSV: {ex.Message}");
                return null;  // Retornar null en caso de error, para que puedas manejarlo posteriormente
            }
        }
    }

    public class Persona
    {
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public Rol RolPersona { get; set; }
        public int materialesPrestados;

        public Persona(string nombre, string cedula, Rol rolPersona)
        {
            Nombre = nombre;
            Cedula = cedula;
            RolPersona = rolPersona;
            materialesPrestados = 0;
        }

        public bool PuedePrestar()
        {
            return materialesPrestados < MaxPrestamos();
        }

        public int MaxPrestamos()
        {
            switch (RolPersona)
            {
                case Rol.Estudiante:
                    return 5;
                case Rol.Profesor:
                    return 3;
                case Rol.Administrativo:
                    return 1;
                default:
                    return 0;
            }
        }

        public bool RegistrarPrestamo()
        {
            if (PuedePrestar())
            {
                materialesPrestados++;
                return true;
            }
            return false;
        }

        public void RegistrarDevolucion()
        {
            if (materialesPrestados > 0)
            {
                materialesPrestados--;
            }
        }

        public string ToCsv()
        {
            return $"{Cedula},{Nombre},{RolPersona},{materialesPrestados}";
        }

        public static Persona FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            Rol rol = (Rol)Enum.Parse(typeof(Rol), values[2]);
            return new Persona(values[1], values[0], rol)
            {
                materialesPrestados = int.Parse(values[3])
            };
        }
    }

    public class Movimiento
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Identificador { get; set; }
        public string Titulo { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } // Préstamo o Devolución

        public Movimiento(string cedula, string nombre, string identificador, string titulo, DateTime fecha, string tipo)
        {
            Cedula = cedula;
            Nombre = nombre;
            Identificador = identificador;
            Titulo = titulo;
            Fecha = fecha;
            Tipo = tipo;
        }
    }

    public class Biblioteca
    {
        private Dictionary<string, Material> materiales;
        private Dictionary<string, Persona> personas;
        private List<Movimiento> movimientos;

        public Biblioteca()
        {
            materiales = new Dictionary<string, Material>();
            personas = new Dictionary<string, Persona>();
            movimientos = new List<Movimiento>();
        }

        public void AgregarMaterial(Material material)
        {
            if (materiales.ContainsKey(material.Identificador))
            {
                materiales[material.Identificador].CantidadRegistrada += material.CantidadRegistrada;
                materiales[material.Identificador].CantidadActual += material.CantidadActual;
                MessageBox.Show($"El material con identificador {material.Identificador} ya existe. Se han sumado las nuevas unidades al inventario.",
                                "Material Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                materiales[material.Identificador] = material;
                MessageBox.Show("Material agregado exitosamente.", "Material Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        public void AgregarPersona(Persona persona)
        {
            if (personas.ContainsKey(persona.Cedula))
                throw new Exception($"La persona con cédula {persona.Cedula} ya está registrada.");
            personas[persona.Cedula] = persona;
        }

        public void EliminarPersona(string cedula)
        {
            if (personas.ContainsKey(cedula))
            {
                if (personas[cedula].materialesPrestados == 0)
                {
                    personas.Remove(cedula);
                }
                else
                {
                    throw new Exception("No se puede eliminar la persona porque tiene materiales prestados.");
                }
            }
            else
            {
                throw new Exception("La persona con la cédula especificada no existe.");
            }
        }
        public Material BuscarMaterial(string identificador)
        {
            if (materiales.TryGetValue(identificador, out Material material))
                return material;
            throw new Exception($"El material con identificador {identificador} no existe.");
        }

        public Persona BuscarPersona(string cedula)
        {
            if (personas.TryGetValue(cedula, out Persona persona))
                return persona;
            throw new Exception($"La persona con cédula {cedula} no está registrada.");
        }

        public void RegistrarPrestamo(string cedula, string identificador, int cantidad)
        {
            var persona = BuscarPersona(cedula);
            var material = BuscarMaterial(identificador);

            if (persona.PuedePrestar() && cantidad > 0)
            {
                for (int i = 0; i < cantidad; i++)
                {
                    if (!persona.RegistrarPrestamo())
                        throw new Exception("Límite de préstamos alcanzado.");
                }

                movimientos.Add(new Movimiento(cedula, persona.Nombre, identificador, material.Titulo, DateTime.Now, "Préstamo"));
                MessageBox.Show($"Préstamo exitoso: {cantidad} unidad(es) de {material.Titulo} prestada(s).");
            }
            else
            {
                throw new Exception("Préstamo no permitido: Verifique la cantidad ingresada o el límite de préstamos.");
            }
        }

        public void RegistrarDevolucion(string cedula, string identificador, int cantidad)
        {
            var persona = BuscarPersona(cedula);
            var material = BuscarMaterial(identificador);

            for (int i = 0; i < cantidad; i++)
            {
                persona.RegistrarDevolucion();
            }

            movimientos.Add(new Movimiento(cedula, persona.Nombre, identificador, material.Titulo, DateTime.Now, "Devolución"));
            MessageBox.Show($"Devolución exitosa: {cantidad} unidad(es) de {material.Titulo} devuelta(s).");
        }

        public void CargarMateriales(string filePath)
        {
            if (!File.Exists(filePath)) return;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Material material = Material.FromCsv(line);

                    // Verificar si el material es null antes de agregarlo
                    if (material != null)
                    {
                        materiales[material.Identificador] = material;
                    }
                    else
                    {
                        Console.WriteLine($"Error al procesar la línea CSV: La línea no pudo ser convertida a Material.");
                    }
                }
            }
        }
        public void GuardarMateriales(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var material in materiales.Values)
                {
                    writer.WriteLine(material.ToCsv());
                }
            }
        }

        public void GuardarPersonas(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var persona in personas.Values)
                {
                    writer.WriteLine(persona.ToCsv());
                }
            }
        }

        public void CargarPersonas(string filePath)
        {
            if (!File.Exists(filePath)) return;
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Persona persona = Persona.FromCsv(line);
                    personas[persona.Cedula] = persona;
                }
            }
        }

        public List<Movimiento> ObtenerMovimientos()
        {
            return movimientos;
        }

        public List<Persona> ObtenerPersonas()
        {
            return new List<Persona>(personas.Values);
        }

        public List<Movimiento> ObtenerMovimientosPorPersona(string cedula)
        {
            return movimientos.FindAll(m => m.Cedula == cedula);
        }

        public List<Material> ObtenerLibros()
        {
            return new List<Material>(materiales.Values);
        }
    }
}
