namespace NewtonsoftTests

open Xunit
open Xunit.Abstractions

open System.Linq
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System

///LINQ to JSON
type LINQ2JSONTest(output : ITestOutputHelper) = 
    ///Using LINQ for JSON
    [<Fact>]
    member this.overview() = 
        let o = JObject.Parse("""{
                  'CPU': 'Intel',
                  'Drives': [
                    'DVD read/writer',
                    '500 gigabyte hard drive'
                  ]
                }""");

        let cpu = o.["CPU"]
        Assert.Equal("Intel",string cpu)

        let firstDrive = o.["Drives"].[0]
        Assert.Equal("DVD read/writer",string firstDrive)

        let allDrives = o.["Drives"].Select(fun t -> string t).ToArray()
        Assert.Equal<string[]>([|"DVD read/writer";"500 gigabyte hard drive"|],allDrives)
    (*Parsing JSON*)

    /// Parsing a JSON Object from text
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
        //output.WriteLine(sprintf "%A" o)

        let cpu = o.["CPU"]
        Assert.Equal("Intel",string cpu)
        

    /// Parsing a JSON Array from text
    [<Fact>]
    member this.ParsingJSONArray() = 
        let json = """[
          'Small',
          'Medium',
          'Large'
        ]"""

        let a = JArray.Parse(json)
        //output.WriteLine(sprintf "%A" a)

        let item = a.[0]
        Assert.Equal("Small",string item)

    (*Creating JSON*)


    /// Manually Creating JSON
    [<Fact>]
    member this.CreatingJSON() = 
        let array = JArray()
        let text = JValue("Manual text")
        let date = JValue(DateTime(2000, 5, 23))

        array.Add(text)
        array.Add(date)

        let json = array.ToString(Formatting.None)
        Assert.Equal("""["Manual text","2000-05-23T00:00:00"]""",json)


    /// Creating JSON Declaratively
    [<Fact>]
    member this.CreatingJSONDeclaratively() = 
        let posts = [| Post()|]

        let rss =
            JObject(
                JProperty("channel",
                    JObject(
                        JProperty("title", "James Newton-King"),
                        JProperty("link", "http://james.newtonking.com"),
                        JProperty("description", "James Newton-King's blog."),
                        JProperty("item",
                            JArray(
                                posts
                                    .OrderBy(fun p -> p.Title)
                                    .Select(fun p ->
                                        JObject(
                                            JProperty("title", p.Title),
                                            JProperty("description", p.Description),
                                            JProperty("link", p.Link),
                                            JProperty("category",
                                                JArray(
                                                    p.Categories.Select(fun c -> JValue(c)))))))
                             )
                        )
                )
            )
        let json = rss.ToString(Formatting.None)
        let expected ="""{"channel":{"title":"James Newton-King","link":"http://james.newtonking.com","description":"James Newton-King's blog.","item":[{"title":"","description":"","link":"","category":[]}]}}""" 
        //output.WriteLine(json)

        Assert.Equal(expected,json)

    /// Creating JSON from an object
    //JObject.FromObject









