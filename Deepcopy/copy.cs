
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
		TestCopy test = new TestCopy();
        test.Age = 30;
        test.IDCode = "12333";
        test.param = new Param();
        test.param.Name = "Dep1";

    	TestCopy copy = test.Clone() as TestCopy;
    	Console.WriteLine(copy.Age);
        Console.WriteLine(copy.IDCode);
        Console.WriteLine(copy.param.Name);
    
        Console.WriteLine("copy change test: ");
        test.Age = 60;
        test.IDCode = "23333";
        test.param.Name = "Dep2";
        Console.WriteLine(copy.Age);
        Console.WriteLine(copy.IDCode);
        Console.WriteLine(copy.param.Name);

		TestCopy testDeep = new TestCopy();
        testDeep.Age = 30;
        testDeep.IDCode = "12333";
        testDeep.param = new Param();
        testDeep.param.Name = "Dep3";

		Console.WriteLine("TestDeepCopy");
		TestCopy copyDeep = testDeep.DeepClone();
        Console.WriteLine(copyDeep.Age);
        Console.WriteLine(copyDeep.IDCode);
        Console.WriteLine(copyDeep.param.Name);

		Console.WriteLine("deepcopy change test: ");
        testDeep.Age = 60;
        testDeep.IDCode = "23333";
        testDeep.param.Name = "Dep4";
        Console.WriteLine(copyDeep.Age);
        Console.WriteLine(copyDeep.IDCode);
        Console.WriteLine(copyDeep.param.Name);
    }
}

[Serializable]
class TestCopy : ICloneable{

    public string IDCode {get; set;}
    public int Age{get; set;}

    public Param param {get; set;}


    public object Clone(){
        return this.MemberwiseClone();
    }

    public TestCopy DeepClone(){
        using (var ms = new MemoryStream()){
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, this);
            ms.Position = 0;
            return (TestCopy)formatter.Deserialize(ms);
        }
    }
}

[Serializable]
class Param{
    public string Name{get; set;}
    public override string ToString(){
        return this.Name;
    }
}