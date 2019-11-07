using Nito.AsyncEx;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncConsole
{
    delegate void MessageHandler(int message);
    class Program
    {
        static MessageHandler messageHandler = B;
        public static void A(int i)
        {
            Console.WriteLine($"A - {i}");
            //messageHandler.Invoke(i);
        }

        public static void B(int i)
        {
            Console.WriteLine($"B - {i}");
        }

        public async static Task CAsync(int i)
        {
            //await Task.Delay(5);  //// выполнится по завершении (асинхронно)
            Task.Delay(5);  // выполнится в одном потоке
            Console.WriteLine($"C - {i} async");
        }

        public async static Task<string> First()
        {
            await Task.Delay(5000);
            return "+++++++++++++++++++++first+++++++++++++++++++++";
        }

        static void Main(string[] args)
        {

            Task.Run(async () =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Task<string> firstResult = null;
                    if (i == 0)
                    {
                        firstResult = First();
                    }
                    First();
                    A(i);
                    B(i);
                    CAsync(i);

                    if (firstResult != null)
                    {
                        var res = await firstResult;
                        Console.WriteLine(res);
                    }

                    if (firstResult != null)
                    {
                        var res = await firstResult;
                        Console.WriteLine(res);
                    }
                }

                Console.WriteLine("======================================");

                //BaseClass baseClass = new BaseClass();
                //SubClass subClass = new SubClass();
                //for (int i = 0; i < 100; i++)
                //{
                //    var baseC = baseClass.AlexsMethod(i);
                //    //Console.WriteLine($"Base - {baseC}");
                //    var sub = subClass.AlexsMethod(i);
                //    //Console.WriteLine($"Sub - {sub}");
                //}
            }).GetAwaiter().GetResult();


            Console.ReadLine();
        }


        class BaseClass
        {
            public virtual async Task<int> AlexsMethod(int i)
            {
                await Task.Delay(500);
                Console.WriteLine($"Base - {i}");
                return i;
            }
        }

        class SubClass : BaseClass
        {
            //// Переопределяет метод базового класса с async AlexsMethod
            //public async override Task<int> AlexsMethod(int i)
            //{
            //    await Task.Delay(500);
            //    Console.WriteLine($"Sub - {i}");
            //    return i;
            //}

            //ИЛИ

            // Переопределяет метод базового класса без async AlexsMethod
            public override Task<int> AlexsMethod(int i)
            {
                //await await Task.Delay(500);  //ошибка
                Console.WriteLine($"Sub - {i}");
                return Task.FromResult(i);
            }
        }
    }
}
