namespace TimelineAssistant.Extensions
{
    public static class FileExtensions
    {
        private const string ExcelFileExtension = ".xlsx";
        public static string RemoveExcelExtension(this string fileName)
        {
            return fileName.Replace(ExcelFileExtension, "");
        }

        public static string AppendExcelExtension(this string fileName)
        {
            if (fileName.Contains(ExcelFileExtension))
            {
                return fileName;
            }

            return $"{fileName}{ExcelFileExtension}";
        }
    }
}
