using HW02.BussinessContext.DB.Entities;
using HW02.LoggerContext;
using System.Reflection;
using System.Text;

namespace HW02.BussinessContext
{
    public static class Printer
    {
        public static void PrintInfo(string info)
        {
            Console.WriteLine($"-- {info}");
        }

        public static void PrintInfo(object sender, CommandEvaluatedEventArgs e)
        {
            if ((e.IsSuccessful != null && !(bool)!e.IsSuccessful) || e.ErrorMessage == null) return;

            PrintInfo(e.ErrorMessage);
        }

        public static void PrintEntitiesAsTable<T>(List<T> entities)
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.Append(GenerateHeader(entities, out var columnNamesByWidth));
            sb.Append(GenerateBody(entities, columnNamesByWidth));

            Console.WriteLine(sb);
        }

        private static int CalculateColumnWidth<T>(List<T> entities, PropertyInfo property)
        {
            var columnWidth = entities.Select(e => e.GetType().GetProperty(property.Name).GetValue(e)?.ToString()?.Length ?? 0).Max();
            columnWidth = Math.Max(columnWidth, property.Name.Length);
            
            return columnWidth;
        }

        private static string GenerateHeader<T>(List<T> entities, out Dictionary<string, int> columnNamesByWidth)
        {
            var sb = new StringBuilder();
            var entity = entities.FirstOrDefault();

            columnNamesByWidth = new Dictionary<string, int>();

            if (entity == null) return "";

            sb.Append('|');
            foreach (var property in entity.GetType().GetProperties())
            {
                var columnWidth = CalculateColumnWidth(entities, property);
                columnNamesByWidth.Add(property.Name, columnWidth);
                sb.Append($"{property.Name.PadRight(columnWidth)}|");
            }

            sb.AppendLine();
            sb.AppendLine(string.Concat(Enumerable.Repeat("-", sb.Length)));

            return sb.ToString();
        }

        private static string GenerateBody<T>(List<T> entities, Dictionary<string, int> columnNamesByWidth)
        {
            var sb = new StringBuilder();

            foreach (var entity in entities.Where(entity => entity != null))
            {
                sb.Append('|');
                foreach (var property in entity!.GetType().GetProperties())
                {
                    sb.Append($"{property.GetValue(entity)?.ToString()?.PadRight(columnNamesByWidth[property.Name])}|");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
