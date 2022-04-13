using System;
using System.IO;

namespace FinalProjectModule_8_1
{
    class Program
    {
        // Напишите программу, которая чистит нужную нам папку от файлов  и папок,
        // которые не использовались более 30 минут
        static void Main(string[] args)
        {
            // На вход программа принимает путь до папки.
            if (args.Length == 0)
            {
                Console.WriteLine("Не задан путь до папки!"); 
                Console.ReadLine();
                return;
            }

            long delDir = 0;
            long delFile = 0;
            var path = args[0];
            var directory = new DirectoryInfo(path);
            if (directory.Exists) // предусмотрена проверка на наличие папки по заданному пути
            {
                Console.WriteLine($"Папка: {path}\n");
                try // предусмотрена обработка исключений при доступе к папке
                {
                    Cleaning(directory, ref delDir, ref delFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка чистки папки: " + ex); // логирует исключение в консоль
                }

                Console.WriteLine($"Папка очистилась от файлов ({delFile}) и папок ({delDir})," +
                                            "\nкоторые не использовались более 30 минут");
            }
            else
            {
                Console.WriteLine("Заданой папки не существует!");
            }

            Console.ReadLine();
        }

        static void Cleaning(DirectoryInfo directory, ref long delDir, ref long delFile)
        {
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                TimeSpan interval = DateTime.Now - file.LastWriteTime;
                if (interval.Minutes > 30)
                {
                    file.Delete();
                    ++delFile;
                }
            }

            DirectoryInfo[] dirs = directory.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                Cleaning(dir, ref delDir, ref delFile); // код должен удалять папки рекурсивно

                TimeSpan interval = DateTime.Now - dir.LastWriteTime;
                // Удаляем папку если в ней нет папок и файлов и она не изменялась больше 30 минут
                if (interval.Minutes > 30 && dir.GetDirectories().Length == 0 && dir.GetFiles().Length == 0)
                {
                    dir.Delete(true);
                    ++delDir;
                }
            }
        }
    }
}
