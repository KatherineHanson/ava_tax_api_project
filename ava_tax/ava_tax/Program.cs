using System;
using Avalara.AvaTax.RestClient;

namespace ava_tax
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Connect to AvaTax
            var client = new AvaTaxClient("CodefellowsTestApp", "1.0",
                Environment.MachineName, AvaTaxEnvironment.Production)
                .WithSecurity("ted+codefellows@spence.net", "4C87ABA1091");

            // Test connection!
            var pingResult = client.Ping();
            Console.WriteLine(pingResult.ToString());

            // Find a tax code for Sushi!
            var taxCodeResult = client.ListTaxCodes("description contains Sushi", null, null, null);
            Console.WriteLine(taxCodeResult.ToString());

            // Let's sell some sushi!
            var t = new TransactionBuilder(client, null, DocumentType.SalesOrder, "ABC")
                .WithLine(10.0m, 1, "PF160026", "Sushi for Lunch", null, null)
                .WithAddress(TransactionAddressType.SingleLocation, "2000 Main Street", null, null,
                            "Irvine", "CA", "92614", "US")
                .Create();
            Console.WriteLine(t);
        }
    }
}
