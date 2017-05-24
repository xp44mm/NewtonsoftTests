namespace NewtonsoftTests

open Xunit
open Xunit.Abstractions

open System.Linq
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System

type X() =
    member val Category = "" with get,set
    member val Count = 0 with get,set

type QueryingLINQ(output : ITestOutputHelper) = 
    let json = """{
        'channel': {
        'title': 'James Newton-King',
        'link': 'http://james.newtonking.com',
        'description': 'James Newton-King\'s blog.',
        'item': [
            {
            'title': 'Json.NET 1.3 + New license + Now on CodePlex',
            'description': 'Annoucing the release of Json.NET 1.3, the MIT license and the source on CodePlex',
            'link': 'http://james.newtonking.com/projects/json-net.aspx',
            'categories': [
                'Json.NET',
                'CodePlex'
            ]
            },
            {
            'title': 'LINQ to JSON beta',
            'description': 'Annoucing LINQ to JSON',
            'link': 'http://james.newtonking.com/projects/json-net.aspx',
            'categories': [
                'Json.NET',
                'LINQ'
            ]
            }
        ]
        }
    }"""

    let rss = JObject.Parse(json)

    /// Getting values by Property Name or Collection Index
    [<Fact>]
    member this.GettingJSONValues() = 
        let rssTitle = rss.["channel"].["title"]
        Assert.Equal("James Newton-King",string rssTitle)

        let itemTitle = rss.["channel"].["item"].[0].["title"]
        Assert.Equal("Json.NET 1.3 + New license + Now on CodePlex",string itemTitle)

        let categories = rss.["channel"].["item"].[0].["categories"] :?> JArray
        Assert.Equal("""["Json.NET","CodePlex"]""",categories.ToString(Formatting.None))
        

        let categoriesText = categories.Select(fun c -> string c).ToArray()
        Assert.Equal("Json.NET",categoriesText.[0])
        Assert.Equal("CodePlex",categoriesText.[1])
            
    /// Querying with LINQ
    [<Fact>]
    member this.QueryingJSON() = 
        let postTitles =
            rss.["channel"].["item"].Select(fun p -> string p.["title"]).ToArray()
        
        Assert.Equal("Json.NET 1.3 + New license + Now on CodePlex",postTitles.[0])
        Assert.Equal("LINQ to JSON beta",postTitles.[1])


        let categories =
            let values = rss.["channel"].["item"].SelectMany(fun i -> i.["categories"].ToArray()|> Seq.ofArray)

            //Values<>:数组中的每个成员强制转化成指定类型
            let groups = values.Values<string>().GroupBy(fun c -> c)

            groups.OrderByDescending(fun g -> g.Count())
                .Select(fun g -> new X(Category = g.Key, Count = g.Count())).ToArray()
            
        Assert.Equal("Json.NET",categories.[0].Category)
        Assert.Equal(2,categories.[0].Count)

        Assert.Equal("CodePlex",categories.[1].Category)
        Assert.Equal(1,categories.[1].Count)

        Assert.Equal("LINQ",categories.[2].Category)
        Assert.Equal(1,categories.[2].Count)

    [<Fact>]
    member this.DeserializingUsingLINQExample() = 
        let jsonText = """{
          'short': {
            'original': 'http://www.foo.com/',
            'short': 'krehqk',
            'error': {
              'code': 0,
              'msg': 'No action taken'
            }
          }
        }"""

        let json = JObject.Parse(jsonText);

        let shortie = Shortie(
                        Original = string json.["short"].["original"],
                        Short = string json.["short"].["short"],
                        Error = ShortieException(
                            Code = int json.["short"].["error"].["code"],
                            ErrorMessage = string json.["short"].["error"].["msg"])
                    )

        Assert.Equal("http://www.foo.com/",shortie.Original)
        Assert.Equal("No action taken",shortie.Error.ErrorMessage)
