using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParakeetProducer
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            using var client = new HttpClient();

            Console.WriteLine("Please input number for getting the book: ");

            var num = Console.ReadLine();
            var url = $"http://www.gutenberg.org/cache/epub/{num}/pg{num}.txt";

            var completeBook = await client.GetStringAsync(url);
            var title = completeBook.Substring(0, completeBook.IndexOf('\n'));

            Console.WriteLine("The book title is " + title);

            completeBook = completeBook.Substring((title.Length + 1), (title.Length + 1) + 10000);

            Console.WriteLine("The book's first 10000 characters are: " + completeBook);
        }
    }
}
