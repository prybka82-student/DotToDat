using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace DotToDat
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = GetPath(args);

            var data = ReadFile(path);

            var (name, vertices, edges) = ParseGraphData(data);

            data = ConvertGraphData(vertices, edges);

            path = GetOutputFilePath(path);

            SaveFile(data, path);

        }

        private static void SaveFile(string data, string path)
        {
            try
            {
                File.WriteAllText(path, data);

                Console.WriteLine($"Data successfully saved as {path}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An exception occurred during saving file to {path}: {e.Message}$");
                throw e;
            }
        }

        private static string GetOutputFilePath(string path)
        {
            var oldExt = Path.GetExtension(path);
            var newExt = ".dat";

            if (oldExt.Length > 0)
                return path.Replace(oldExt, newExt);

            return string.Concat(path, newExt);
        }

        private static string ConvertGraphData(IEnumerable<string> vertices, IEnumerable<(string, string)> edges)
        {
            try
            {
                var result = new StringBuilder();
                var flatEdges = edges.SelectMany(x => new[] { x.Item1, x.Item2 });

                result.AppendLine("data;");
                result.AppendLine($"param n := {vertices.Count()};");
                result.AppendLine($"set E := {string.Join(" ", flatEdges)};");
                result.AppendLine("end;");

                return result.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine($"The following exception occurred during converting graph data into string: {e.Message}");
                throw e;
            }
        }

        private static (string name, IEnumerable<string> vertices, IEnumerable<(string, string)> edges) ParseGraphData(string data)
        {
            try
            {
                RemoveGraphKeyword(ref data);

                var name = RemoveGraphName(ref data);

                RemoveCurlyBrackets(ref data);
                RemoveNewlineCharacters(ref data);

                var verticesAndEdges = GetVerticesAndEdges(data);

                return (name, verticesAndEdges.vertices, verticesAndEdges.edges);
            }
            catch (Exception e)
            {
                Console.WriteLine($"The following exception occurred while converting parsing graph data: {e.Message}");
                throw e;
            }
        }

        private static (IEnumerable<string> vertices, IEnumerable<(string, string)> edges) GetVerticesAndEdges(string data)
        {
            var parts = data.Split(';');
            var vertices = new List<string>();
            var edges = new List<(string, string)>();

            foreach (var part in parts)
            {
                if (part.Contains('-'))
                {
                    var edgeParts = part.Split('-');

                    var edge1 = edgeParts[0].Replace(">", "").Replace("<", "").Trim();
                    var edge2 = edgeParts[2].Replace(">", "").Replace("<", "").Trim();

                    if (edge1.Length > 0 && edge2.Length > 0)
                        edges.Add((edge1, edge2));
                }
                else
                    if (part.Length > 0)
                        vertices.Add(part);
            }

            return (vertices, edges);
        }

        private static void RemoveNewlineCharacters(ref string data)
        {
            data = data.Replace(Environment.NewLine, "");
        }

        private static void RemoveCurlyBrackets(ref string data)
        {
            data = data.Replace("{", "").Replace("}", "");
        }

        private static string RemoveGraphName(ref string data)
        {
            var name = data.Split(' ')[0];

            data = data.Replace(name, "").Trim();

            return name;
        }

        private static void RemoveGraphKeyword(ref string data)
        {
            data = data.Replace("graph", "").Trim();
        }

        private static string ReadFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Console.WriteLine($"The following exception occurred while reading the file: {path}");
                throw e;
            }
        }

        private static string GetPath(string[] args)
        {
            var path = "";

            try
            {
                if (args.Length == 0) throw new ArgumentNullException("File path was not provided");

                path = args[0];

                if (!File.Exists(path)) throw new FileNotFoundException($"The file {path} doesn't exists");

                return args[0];
            }
            catch (Exception e)
            {
                Console.WriteLine($"The following expection occurred while reading the file {path}: {e.Message}");
                throw e;
            }
        }
    }
}
