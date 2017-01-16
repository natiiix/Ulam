using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Ulam
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Stopwatch sw = new Stopwatch();
            const int PRIME_COUNT_MAX = 1048576;
            int[] primes = new int[PRIME_COUNT_MAX];
            int counter = 0;

            Console.WriteLine("Press Enter to start the computation...");
            Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Computing...");

            sw.Restart();

            for (int i = 2; counter < PRIME_COUNT_MAX; i++)
                if (isPrime(i))
                    primes[counter++] = i;

            sw.Stop();
            
            Console.Clear();
            Console.WriteLine("Computing...");
            
            Console.Clear();
            Console.WriteLine("Computation completed!" + Environment.NewLine +
                              "Elapsed time: " + sw.ElapsedMilliseconds.ToString() + " ms" + Environment.NewLine +
                              counter.ToString() + ". prime number: " + primes.Last().ToString());
            Console.ReadLine();*/

            const int PRIME_COUNT_MAX = 1048576;
            //const int PRIME_COUNT_MAX = 65536;
            const int SAMPLE_COUNT = 32;

            Task[] taskUnits = new Task[SAMPLE_COUNT];
            long[] taskTimes = new long[SAMPLE_COUNT];
            Stopwatch swRealTime = new Stopwatch();

            Console.WriteLine("Press Enter to start the computation...");
            Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Computing...");

            swRealTime.Restart();
            
            for (int i = 0; i < SAMPLE_COUNT; i++)
            {
                int iClone = i;
                taskUnits[i] = new Task(() => taskTimes[iClone] = computePrimes(PRIME_COUNT_MAX/*, iClone*/));
            }

            //Console.WriteLine("Task units initialized. [" + swRealTime.ElapsedMilliseconds.ToString() + " ms]");

            for (int i = 0; i < taskUnits.Length; i++)
                taskUnits[i].Start();

            /*for (int i = 0; i < SAMPLE_COUNT; i++)
            {
                int iClone = i;
                taskUnits[i] = Task.Factory.StartNew(() => taskTimes[iClone] = computePrimes(PRIME_COUNT_MAX, iClone));
            }*/

            //Console.WriteLine("Task units started. [" + swRealTime.ElapsedMilliseconds.ToString() + " ms]");
            
            Task.WaitAll(taskUnits);

            swRealTime.Stop();

            Console.Clear();
            Console.WriteLine("Computation completed!" + Environment.NewLine +
                              "Elapsed real time: " + swRealTime.ElapsedMilliseconds.ToString() + " ms" + Environment.NewLine +
                              "Total thread time: " + taskTimes.Sum().ToString() + " ms");
            Console.ReadLine();
        }

        private static long computePrimes(int _count/*, int _taskID*/)
        {
            int[] primes = new int[_count];
            int counter = 0;
            Stopwatch sw = new Stopwatch();

            sw.Restart();

            for (int i = 2; counter < _count; i++)
                if (isPrime(i))
                    primes[counter++] = i;

            sw.Stop();

            //Console.WriteLine("Computation finished! #{0} [{1}] ({2} ms)", _taskID, _count, sw.ElapsedMilliseconds);

            return sw.ElapsedMilliseconds;
        }

        private static bool isPrime(int n)
        {
            // There is no prime lower than 2
            if (n < 2)
                return false;
            
            // Get n's square root
            int r = (int)Math.Sqrt(n);

            // Find a whole number divisor
            for (int i = 2; i <= r; i++)
                if (n % i == 0)
                    // If such divisor was found the number can't be prime
                    return false;

            // If no valid divisor was found the number is prime
            return true;
        }
    }
}
