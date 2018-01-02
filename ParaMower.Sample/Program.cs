using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ParaMower.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SampleSchema destData = null;
            try
            {
                destData = args.Length == 0 ? ParamConvertor.InteractiveLoad<SampleSchema>(LoadingDel) : ParamConvertor.Load<SampleSchema>(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data load failed with exception: {ex}");
            }

            if (destData == null)
            {
                Console.WriteLine("Press enter to exit...");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Data Loaded :");
            Console.WriteLine(JsonConvert.SerializeObject(destData, Formatting.Indented,new StringEnumConverter()));
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        private static string LoadingDel(string paramAlias, string paramDesc)
        {
            Console.WriteLine($@"Enter the parameter(-{paramAlias}), For the usage: {paramDesc}");
            return Console.ReadLine();
        }
    }
}
