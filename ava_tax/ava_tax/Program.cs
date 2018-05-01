using Avalara.AvaTax.RestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine("Your calculated t tax was {0}", t.totalTax);

            var t2 = new TransactionBuilder(client, "DEFAULT", DocumentType.SalesInvoice, "ABC")
                .WithAddress(TransactionAddressType.ShipFrom, "123 Main Street", "Irvine", null, null, "CA", "92615", "US")
                .WithAddress(TransactionAddressType.ShipTo, "100 Ravine Lane NE", "Bainbridge Island", null, null, "WA", "98110", "US")
                .WithLine(100.0m)
                .WithLine(1234.56m) // Each line is added as a separate item on the invoice
                .WithExemptLine(50.0m, "NT") // An exempt item
                .WithLine(2000.0m) // The 2 addresses below apply to this $2000 line item
                .WithLineAddress(TransactionAddressType.ShipFrom, "123 Main Street", "Irvine", null, null, "CA","92615", "US")
                .WithLineAddress(TransactionAddressType.ShipTo, "1500 Broadway", "New York", null, null, "NY", "10019", "US")
                //.WithLine(50.0m, "FR010000") // shipping costs
                .Create();
            Console.WriteLine(t2);
            Console.WriteLine("Your calculated t2 tax was {0}", t2.totalTax);
            }
    }
}
