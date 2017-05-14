namespace NewtonsoftTests

open Xunit
open Xunit.Abstractions

open System.Linq
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System

type LINQ2JSONTest(output : ITestOutputHelper) = 

    [<Fact>]
    member this.overview() = 
        let o = JObject.Parse("""{
                  'CPU': 'Intel',
                  'Drives': [
                    'DVD read/writer',
                    '500 gigabyte hard drive'
                  ]
                }""");

        let cpu = string o.["CPU"]
        Assert.Equal("Intel",cpu)

        let firstDrive = string o.["Drives"].[0]
        Assert.Equal("DVD read/writer",firstDrive)


        let allDrives = o.["Drives"].Select(fun t -> string t).ToArray()
        Assert.Equal<string[]>([|"DVD read/writer";"500 gigabyte hard drive"|],allDrives)


    [<Fact>]
    member this.ParsingJSONObject() = 
        let json = """{
          CPU: 'Intel',
          Drives: [
            'DVD read/writer',
            '500 gigabyte hard drive'
          ]
        }"""

        let o = JObject.Parse(json)
        output.WriteLine(sprintf "%A" o)


    [<Fact>]
    member this.ParsingJSONArray() = 
        let json = """[
          'Small',
          'Medium',
          'Large'
        ]"""

        let a = JArray.Parse(json)
        output.WriteLine(sprintf "%A" a)

    [<Fact>]
    member this.CreatingJSON() = 
        let array = new JArray()
        let text = new JValue("Manual text")
        let date = new JValue(new DateTime(2000, 5, 23))

        array.Add(text)
        array.Add(date)

        let json = array.ToString()

        output.WriteLine(json)

    [<Fact>]
    member this.CreatingJSONDeclaratively() = 
        let posts = [| new Post()|]

        let rss =
            new JObject(
                new JProperty("channel",
                    new JObject(
                        new JProperty("title", "James Newton-King"),
                        new JProperty("link", "http://james.newtonking.com"),
                        new JProperty("description", "James Newton-King's blog."),
                        new JProperty("item",
                            new JArray(
                                posts
                                    .OrderBy(fun p -> p.Title)
                                    .Select(fun p ->
                                        new JObject(
                                            new JProperty("title", p.Title),
                                            new JProperty("description", p.Description),
                                            new JProperty("link", p.Link),
                                            new JProperty("category",
                                                new JArray(
                                                    p.Categories.Select(fun c -> new JValue(c)))))))))));

        output.WriteLine(rss.ToString());








