using System;
using System.IO;
using Xunit;

namespace P371.ASDML.Tests
{
    public class StressTests
    {
        const int TRESHOLD = 20;

        public StressTests()
        {
            Func<string> random = () => "ASDML_" + Path.GetRandomFileName().Replace(".", "");
            using (var asdml = new StreamWriter($"../../../data{TRESHOLD}.asdml"))
            {
                for (int i = 0; i < TRESHOLD; i++)
                {
                    asdml.WriteLine($"{random()} {{");
                    for (int j = 0; j < TRESHOLD; j++)
                    {
                        asdml.WriteLine($"  .{random()} {random()}");
                    }
                    for (int j = 0; j < TRESHOLD; j++)
                    {
                        asdml.WriteLine($"  {random()} {{");
                        for (int k = 0; k < TRESHOLD; k++)
                        {
                            asdml.WriteLine($"    .{random()} {random()}");
                        }
                        for (int k = 0; k < TRESHOLD; k++)
                        {
                            asdml.WriteLine($"    {random()} {{");
                            for (int l = 0; l < TRESHOLD; l++)
                            {
                                asdml.WriteLine($"      .{random()} {random()}");
                            }
                            asdml.WriteLine($"    }}");
                        }
                        asdml.WriteLine($"  }}");
                    }
                    asdml.WriteLine($"}}");
                }
            }
        }

        [Fact]
        public void StressTest1()
        {
            Parser parser = new Parser(new FileInfo($"../../../data{TRESHOLD}.asdml"));
            var output = parser.Parse();
        }
    }
}
