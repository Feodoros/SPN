using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchPerfectNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            List<int> Numbers1 = new List<int>();
            List<int> Numbers2 = new List<int>();

            List<int> numbers = new List<int>();
            for (int i = 0; i < 10000; i++)
                numbers.Add(i);

            Stopwatch stopwatch1 = new Stopwatch();
            Stopwatch stopwatch2 = new Stopwatch();


            //Parallel
            Task taskPar = Task.Run(() =>
            {
                Console.WriteLine("Считаем параллельным способом...");
                stopwatch2.Start();
                Numbers2 = (from number in numbers.AsParallel() where PerfectNumberTrueOrFalse(number) select number).ToList();
                stopwatch2.Stop();
                Console.WriteLine("Параллельным способом посчитано.");
            });


            //Ordinary
            Task taskOrd = Task.Run(() =>
            {
                Console.WriteLine("Считаем обычным способом...");
                stopwatch1.Start();
                Numbers1 = (from number in numbers where PerfectNumberTrueOrFalse(number) select number).ToList(); 
                stopwatch1.Stop();
                Console.WriteLine("Обычным способом посчитано.");                
            });


            Task finalTask = Task.Factory.ContinueWhenAll(new Task[] { taskOrd, taskPar }, ant =>
            {
                Console.WriteLine();
                Console.WriteLine("Совершенные числа: ");
                foreach (var number in Numbers1)
                    Console.WriteLine(number);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;

                double x = Math.Round((double)stopwatch1.ElapsedMilliseconds / 1000, 3);
                double y = Math.Round((double)stopwatch2.ElapsedMilliseconds / 1000, 3);

                Console.WriteLine("Время работы обычным способом: " + x + " секунд.");
                Console.WriteLine("Время работы параллельным способом: " + y + " секунд.");
                Console.WriteLine();
                Console.WriteLine("Параллельным способом быстрее на {0} секунд.", x-y);
            });

            Console.ReadKey();
        }


        public static bool PerfectNumberTrueOrFalse(int num)
        {
            int sum = 1;
            for (int i = 2; i < num / 2 + 1; i++)
                if (num % i == 0)
                    sum += i;
            return (sum == num);
        }
    }
}

