using System;
using System.Collections.Generic;

namespace PracticaExperimentalUEA_Biblioteca
{
    // Clase que representa la entidad del mundo real: Libro
    public class Libro
    {
        public string Isbn { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }

        public Libro(string isbn, string titulo, string autor, string categoria)
        {
            Isbn = isbn;
            Titulo = titulo;
            Autor = autor;
            Categoria = categoria;
        }

        public override string ToString()
        {
            return $"[ISBN: {Isbn}] '{Titulo}' - {Autor} (Categoría: {Categoria})";
        }
    }

    // Clase gestora que implementa Conjuntos y Mapas
    public class BibliotecaManager
    {
        // Conjunto (HashSet) para garantizar la unicidad absoluta de los ISBN y evitar duplicados
        private readonly HashSet<string> _islandsRegistrados;

        // Mapa (Dictionary) para indexar libros por su ISBN como clave única
        private readonly Dictionary<string, Libro> _catalogoPorIsbn;

        // Mapa anidado (Dictionary de listas) para agrupar libros por su Categoría
        private readonly Dictionary<string, List<Libro>> _librosPorCategoria;

        public BibliotecaManager()
        {
            _islandsRegistrados = new HashSet<string>();
            _catalogoPorIsbn = new Dictionary<string, Libro>();
            _librosPorCategoria = new Dictionary<string, List<Libro>>();
        }

        // Operación de registro aplicando Conjuntos y Mapas
        public bool RegistrarLibro(string isbn, string titulo, string autor, string categoria)
        {
            // Verificación de unicidad utilizando HashSet (O(1))
            if (_islandsRegistrados.Contains(isbn))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[-] Error crítico: El ISBN '{isbn}' ya se encuentra registrado en el sistema.");
                Console.ResetColor();
                return false;
            }

            // Crear entidad
            Libro nuevoLibro = new Libro(isbn, titulo, autor, categoria);

            // 1. Agregar al Conjunto de control de unicidad
            _islandsRegistrados.Add(isbn);

            // 2. Agregar al Diccionario principal por ISBN
            _catalogoPorIsbn[isbn] = nuevoLibro;

            // 3. Organizar y agrupar en el Diccionario por Categoría
            if (!_librosPorCategoria.ContainsKey(categoria))
            {
                _librosPorCategoria[categoria] = new List<Libro>();
            }
            _librosPorCategoria[categoria].Add(nuevoLibro);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[+] ¡Éxito! Libro '{titulo}' registrado correctamente.");
            Console.ResetColor();
            return true;
        }

        // Funcionalidad de Reportería: Consultar libro por ISBN mediante Diccionario
        public void BuscarPorIsbn(string isbn)
        {
            Console.WriteLine($"\n--- CONSULTA DE LIBRO POR ISBN: {isbn} ---");
            if (_catalogoPorIsbn.TryGetValue(isbn, out Libro libroEncontrado))
            {
                Console.WriteLine($"[Resultado Encontrado]: {libroEncontrado}");
            }
            else
            {
                Console.WriteLine("[!] No se encontró ningún registro asociado a ese código ISBN.");
            }
        }

        // Funcionalidad de Reportería: Listar catálogo agrupado por Categoría
        public void MostrarCatalogoPorCategorias()
        {
            Console.WriteLine("\n=======================================================");
            Console.WriteLine("          REPORTE GENERAL: CATÁLOGO POR CATEGORÍA       ");
            Console.WriteLine("=======================================================");

            if (_librosPorCategoria.Count == 0)
            {
                Console.WriteLine("[La biblioteca se encuentra vacía actualmente]");
                return;
            }

            foreach (var par in _librosPorCategoria)
            {
                Console.WriteLine($"\n> Categoría: {par.Key} ({par.Value.Count} obras)");
                foreach (var libro in par.Value)
                {
                    Console.WriteLine($"    * {libro.Titulo} - {libro.Autor} (ISBN: {libro.Isbn})");
                }
            }
            Console.WriteLine("=======================================================\n");
        }
    }

    // Clase principal de orquestación
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "UEA - Práctica Experimental N°3 (Conjuntos y Mapas)";
            BibliotecaManager biblioteca = new BibliotecaManager();

            // 1. Registrar libros de prueba
            biblioteca.RegistrarLibro("978-0134685991", "Clean Code", "Robert C. Martin", "Ingeniería de Software");
            biblioteca.RegistrarLibro("978-0201633610", "Design Patterns", "Erich Gamma", "Ingeniería de Software");
            biblioteca.RegistrarLibro("978-0262033848", "Introduction to Algorithms", "Thomas H. Cormen", "Ciencias de la Computación");
            biblioteca.RegistrarLibro("978-1491957660", "Designing Data-Intensive Applications", "Martin Kleppmann", "Bases de Datos");

            // Prueba de control de duplicados mediante Conjunto (HashSet)
            Console.WriteLine("\n[Prueba de Control de Duplicados]:");
            biblioteca.RegistrarLibro("978-0134685991", "Clean Code Copia", "Autor Falso", "Ingeniería de Software");

            // 2. Ejecutar reportería general por categorías
            biblioteca.MostrarCatalogoPorCategorias();

            // 3. Ejecutar búsqueda instantánea por clave (Diccionario)
            biblioteca.BuscarPorIsbn("978-0262033848");

            Console.WriteLine("\nPráctica concluida exitosamente. Presione cualquier tecla para salir.");
            Console.ReadKey();
        }
    }
}