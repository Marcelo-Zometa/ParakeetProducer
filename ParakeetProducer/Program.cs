using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParakeetProducer
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            using var client = new HttpClient();
            string APIurl = "http://144.17.24.80:30081";


            //Getting book from user
            Console.WriteLine("Please input number for getting the book: ");
            var num = Console.ReadLine();


            Console.WriteLine("What is the batch size: ");
            var batch = Console.ReadLine();
            var batchNum = Convert.ToInt32(batch);

            for (int j = 0; j < batchNum; j++)
            {
                var url = $"http://www.gutenberg.org/cache/epub/{num}/pg{num}.txt";

                //Getting parts of the book.
                var completeBook = await client.GetStringAsync(url);
                var title = completeBook.Substring(0, completeBook.IndexOf('\n'));
                var partOfBook = completeBook.Substring((title.Length + 1), (title.Length + 1) + 10000);

                Console.WriteLine("The book title is " + title);
                Console.WriteLine("The book's first 10000 characters are: " + completeBook);

                await PostToAPI(APIurl, title, partOfBook);

                var numInt = Convert.ToInt32(num);
                numInt += 1;
                num = Convert.ToString(numInt);
            }

        }

        private static async Task PostToAPI(string APIurl, string title, string book)
        {
            var postingService = RestService.For<ITrigramParserAPI>(APIurl);

            try
            {
                await postingService.CreateTrigrams(new RequestBody
                {
                    Title = title,
                    Text = book
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("Error, could not add book");
                Console.WriteLine(e.ToString());
            }

        }

        public interface ITrigramParserAPI
        {
            [Post("/api/trygram/createtrygrams")]
            Task CreateTrigrams(RequestBody request);
        }
    }

}
