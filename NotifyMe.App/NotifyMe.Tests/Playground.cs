using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotifyMe.App.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyMe.Tests
{
    [TestFixture]
    public class Playground
    {
        [Test]
        public void TestOne()
        {
            try
            {
                var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var path = string.Format("{0}\\{1}", directory, "data.txt");
                string data = File.ReadAllText(path);

                data = data.Substring(1, data.Length - 2);
                var response = JsonConvert.DeserializeObject<ProvidersResponse[]>(data);
            }
            catch (Exception ex)
            {

            }
            
        }
    }
}
