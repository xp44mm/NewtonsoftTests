namespace NewtonsoftTests

open Xunit
open Xunit.Abstractions

open System.Linq
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System

type QueryingSelectToken(output : ITestOutputHelper) = 
    let o = JObject.Parse("""{
        'Stores': [
        'Lambton Quay',
        'Willis Street'
        ],
        'Manufacturers': [
        {
            'Name': 'Acme Co',
            'Products': [
            {
                'Name': 'Anvil',
                'Price': 50
            }
            ]
        },
        {
            'Name': 'Contoso',
            'Products': [
            {
                'Name': 'Elbow Grease',
                'Price': 99.95
            },
            {
                'Name': 'Headlight Fluid',
                'Price': 4
            }
            ]
        }
        ]
    }""")

    [<Fact>]
    member this.SelectTokenExample() = 
        let name = string <| o.SelectToken("Manufacturers[0].Name")
        Assert.Equal("Acme Co",name)
        
        let productPrice = decimal <| o.SelectToken("Manufacturers[0].Products[0].Price")
        Assert.Equal(50M,productPrice)
        
        let productName = string <| o.SelectToken("Manufacturers[1].Products[0].Name")
        Assert.Equal("Elbow Grease",productName)

    ///$.根元素
    ///?()过滤器
    ///@当前元素
    [<Fact>]
    member this.SelectTokenJSONPath() =         
        // manufacturer with the name 'Acme Co'
        let acme = o.SelectToken("$.Manufacturers[?(@.Name == 'Acme Co')]");

        output.WriteLine(acme.ToString())
        
        // name of all products priced 50 and above
        let pricyProducts = o.SelectTokens("$..Products[?(@.Price >= 50)].Name")
        
        for item in pricyProducts do
            output.WriteLine(item.ToString())

    [<Fact>]
    member this.SelectTokenLINQ() = 
        let storeNames = o.SelectToken("Stores").Select(string).ToArray()
        Assert.Equal("Lambton Quay",storeNames.[0])
        Assert.Equal("Willis Street",storeNames.[1])

        let firstProductNames = o.["Manufacturers"].Select(fun m -> string <| m.SelectToken("Products[1].Name")).ToArray()
        Assert.True(String.IsNullOrEmpty(firstProductNames.[0]))
        Assert.Equal("Headlight Fluid",firstProductNames.[1])

        let totalPrice = o.["Manufacturers"].Sum(fun m -> decimal <| m.SelectToken("Products[0].Price"))
        Assert.Equal(149.95M,totalPrice)