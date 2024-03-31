namespace Aula4
{
    public static class FileManager
    {
        public static FileStream CreateFile(string fileName)
        {
            using var file = File.Create(fileName, bufferSize: 1024, FileOptions.WriteThrough);
            return file;
        }

        public static void ReadFileContent(string fileName)
        {
            ArgumentException.ThrowIfNullOrEmpty(fileName);

            if (!File.Exists(fileName))
                throw new FileNotFoundException();

            using var file = File.OpenRead(fileName);

            ArgumentNullException.ThrowIfNull(file);

            if (!file.CanRead) return;

            using var reader = new StreamReader(file);
            var text = reader.ReadLine();

            while (text != null)
            {
                Console.WriteLine(text);
                text = reader.ReadLine();
            }
        }

        public static void ClearFile(string fileName)
        {
            ArgumentException.ThrowIfNullOrEmpty(fileName);

            if (!File.Exists(fileName))
                throw new FileNotFoundException();

            File.WriteAllText(fileName, string.Empty);
        }

        public static void DeleteFile(string fileName)
        {
            ArgumentException.ThrowIfNullOrEmpty(fileName);

            if (!File.Exists(fileName))
                throw new FileNotFoundException();

            File.Delete(fileName);
        }

        public static void WriteOnFile(string fileName, string text)
        {
            ArgumentException.ThrowIfNullOrEmpty(fileName);
            ArgumentException.ThrowIfNullOrEmpty(text);

            if (!File.Exists(fileName))
                throw new FileNotFoundException();

            using var writer = new StreamWriter(fileName, append: true);
            writer.WriteLine(text);
        }
    }
}
