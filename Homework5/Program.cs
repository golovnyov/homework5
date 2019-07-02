using System;
using System.Collections.Generic;
using System.Threading;

namespace Homework5
{
    class Program
    {
        private static ThreadSafeLinkedList<DateTime> ts = new ThreadSafeLinkedList<DateTime>();
        private static LinkedList<DateTime> control = new LinkedList<DateTime>();

        private static ManualResetEvent mre = new ManualResetEvent(false);

        public static void Main()
        {
            // Create two threads, name them, and start them. The
            // thread will block on mre.
            Thread t1 = new Thread(TestThread);
            t1.Name = "Thread 1";
            t1.Start();

            Thread t2 = new Thread(TestThread);
            t2.Name = "Thread 2";
            t2.Start();

            Thread t3 = new Thread(TestThread);
            t3.Name = "Thread 3";
            t3.Start();

            // Now let the threads begin adding random numbers to 
            // the total.
            mre.Set();

            // Wait until all the threads are done.
            t1.Join();
            t2.Join();
            t3.Join();

            var ts_node = ts.Head;
            var t_us_node = control.First;

            int count = 0;

            while (ts_node != null && t_us_node != null)
            {
                ts_node = ts_node.Next;

                t_us_node = t_us_node.Next;

                if (ts_node.Item.Ticks != t_us_node.Value.Ticks)
                {
                    Console.WriteLine($"Thread safe: {ts_node.Item.Ticks} | Ordinary Double: {t_us_node.Value.Ticks}");

                    count++;
                }

                if (count > 100)
                {
                    break;
                }
            }

            Console.ReadLine();
        }

        private static void TestThread()
        {
            // Wait until the signal.
            mre.WaitOne();

            for (int i = 1; i <= 1000000; i++)
            {
                var dateTime = DateTime.Now;

                control.AddFirst(dateTime);

                ts.Add(dateTime);
            }
        }
    }
}
