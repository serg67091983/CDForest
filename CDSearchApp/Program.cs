﻿using CDSearchFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CDSearchApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Folder to search:");
            var folder = Console.ReadLine();

            if (!Directory.Exists(folder))
            {
                Console.WriteLine("NOT FOUND");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Search text:");
            var search = Console.ReadLine().Split(' ').Select(s => s.ToLower());

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var checker = new FileAnalyseChecker();
            var reader = new FileAnalyseReader();
            List<FileRating> ratings = new List<FileRating>();
            var ratingAnalyser = new FileRatingAnalyser();
            foreach (var file in Directory.GetFiles(folder, "*.search.*"))
            {
                if (!checker.Check(file))
                {
                    //Console.WriteLine("Wrong Format {0}", file);
                    continue;
                }

                var fileResult = reader.Read(file);

                if (fileResult == null)
                {
                    continue;
                }

                var rating = ratingAnalyser.Analyse(file, fileResult, search);

                if (rating == null)
                {
                    continue;
                }

                ratings.Add(rating);
            }

            watch.Stop();

            foreach (var rating in ratings.OrderByDescending(r => r.Rating).Take(5))
            {
                Console.WriteLine(rating.FileName);
                Console.WriteLine("Found:{0}", string.Join(",", rating.Occurances));
                Console.WriteLine("Missed:{0}", string.Join(",", rating.Missing));
            }

            Console.WriteLine("Elapsed time: {0} sec", watch.ElapsedMilliseconds / 100);
            Console.ReadLine();
        }
    }
}
