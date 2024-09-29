using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    static void Main(string[] args)
    {
        // Путь к исходной и целевой папкам
        string sourceFolder = @"C:\SourceFolder";
        string targetFolder = @"C:\TargetFolder";

        // Убедимся, что целевая папка существует
        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }

        // Получаем список всех файлов изображений в исходной папке (например, с расширениями .jpg, .png)
        string[] imageFiles = Directory.GetFiles(sourceFolder, "*.*", SearchOption.TopDirectoryOnly)
                                       .Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("png"))
                                       .ToArray();

        // Параллельно обрабатываем каждое изображение
        Parallel.ForEach(imageFiles, (currentFile) =>
        {
            try
            {
                // Загружаем изображение
                using (Image image = Image.FromFile(currentFile))
                {
                    // Поворачиваем изображение на 180 градусов
                    image.RotateFlip(RotateFlipType.Rotate180FlipNone);

                    // Определяем имя файла
                    string fileName = Path.GetFileName(currentFile);
                    string targetPath = Path.Combine(targetFolder, fileName);

                    // Сохраняем изображение в целевой папке
                    image.Save(targetPath);
                }

                Console.WriteLine($"Изображение {currentFile} успешно обработано.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке файла {currentFile}: {ex.Message}");
            }
        });

        Console.WriteLine("Все изображения обработаны.");
    }
}
