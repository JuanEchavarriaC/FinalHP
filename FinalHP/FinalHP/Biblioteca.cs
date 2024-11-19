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
            string[] values = csvLine.Split(',');
            return new Material(
                values[0],
                values[1],
                DateTime.Parse(values[2]),
                int.Parse(values[3])
            )
            {
                CantidadActual = int.Parse(values[4])
            };
        }
    }

    public class Persona
    {
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public Rol RolPersona { get; set; }
        private int materialesPrestados;

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
            Rol rol = (Rol)Enum.Parse(typeof(Rol), values[2]); // Ajuste para versiones antiguas
            return new Persona(values[1], values[0], rol)
            {
                materialesPrestados = int.Parse(values[3])
            };
        }
    }

    public class Biblioteca
    {
        private Dictionary<string, Material> materiales;
        private Dictionary<string, Persona> personas;

        public Biblioteca()
        {
            materiales = new Dictionary<string, Material>();
            personas = new Dictionary<string, Persona>();
        }

        public void AgregarMaterial(Material material)
        {
            if (materiales.ContainsKey(material.Identificador))
                throw new Exception($"El material con identificador {material.Identificador} ya existe.");
            materiales[material.Identificador] = material;
        }

        public void AgregarPersona(Persona persona)
        {
            if (personas.ContainsKey(persona.Cedula))
                throw new Exception($"La persona con cédula {persona.Cedula} ya está registrada.");
            personas[persona.Cedula] = persona;
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

        public void RegistrarPrestamo(string cedula, string identificador)
        {
            var persona = BuscarPersona(cedula);
            var material = BuscarMaterial(identificador);

            if (persona.RegistrarPrestamo() && material.Prestar())
            {
                Console.WriteLine($"Préstamo exitoso: {persona.Nombre} ha prestado {material.Titulo}.");
            }
            else
            {
                throw new Exception($"Préstamo no permitido: Verifique la disponibilidad o el límite de préstamos.");
            }
        }

        public void RegistrarDevolucion(string cedula, string identificador)
        {
            var persona = BuscarPersona(cedula);
            var material = BuscarMaterial(identificador);

            persona.RegistrarDevolucion();
            material.Devolver();
            Console.WriteLine($"Devolución exitosa: {persona.Nombre} ha devuelto {material.Titulo}.");
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

        public void CargarMateriales(string filePath)
        {
            if (!File.Exists(filePath)) return;
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Material material = Material.FromCsv(line);
                    materiales[material.Identificador] = material;
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

            try
            {
                biblioteca.AgregarMaterial(new Material("001", "C# Programming", DateTime.Now, 10));
                biblioteca.AgregarPersona(new Persona("Juan Perez", "123456789", Rol.Estudiante));

                biblioteca.RegistrarPrestamo("123456789", "001");
                biblioteca.RegistrarDevolucion("123456789", "001");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            biblioteca.GuardarMateriales("Materiales.txt");
            biblioteca.GuardarPersonas("Personas.txt");
        }
    }
}