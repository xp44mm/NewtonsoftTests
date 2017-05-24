namespace NewtonsoftTests

open Xunit
open Xunit.Abstractions

open System.Linq
open System
open Newtonsoft.Json

type JsonConvertTest(output : ITestOutputHelper) = 
    let product = new Product()
    do
        product.Name <- "Apple"
        product.ExpiryDate <- new DateTime(2008, 12, 28)
        product.Price <- 3.99M
        product.Sizes <- [| "Small"; "Medium"; "Large" |]

    let json = """{"Name":"Apple","ExpiryDate":"2008-12-28T00:00:00","Price":3.99,"Sizes":["Small","Medium","Large"]}"""

    [<Fact>]
    member this.SerializeObject() = 
        let ss = JsonConvert.SerializeObject(product)
        Assert.Equal(json,ss)

    [<Fact>]
    member this.DeserializeObject() = 
        let deserializedProduct = JsonConvert.DeserializeObject<Product>(json)

        Assert.Equal(product.Name,deserializedProduct.Name)
        Assert.Equal(product.ExpiryDate,deserializedProduct.ExpiryDate)
        Assert.Equal(product.Price,deserializedProduct.Price)
        Assert.Equal<string[]>(product.Sizes,deserializedProduct.Sizes)
