using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Workshop.NewYork
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var store = new DocumentStore
            {
                Urls = new[]
                {
                    "https://a.newyork.dbs.local.ravendb.net"
                },
                Database = "nw",
                Certificate = new X509Certificate2(@"C:\Users\ayende\Downloads\newyork.Cluster.Settings\admin.client.certificate.newyork.pfx")
            })
            {
                store.Initialize();


                var a = store.Operations.Send(new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"
from Employees 
update {
    var parts = this.FullName.split(' ');
    this.FirstName = parts[0];
    this.LastName = parts[1];
    delete this.FullName;
    this.User = $user;
}
",
                    QueryParameters = new Raven.Client.Parameters
                    {
                        ["user"] = "users/2"
                    }
                })
                {
                    
                });

            }
        }
    }
}
