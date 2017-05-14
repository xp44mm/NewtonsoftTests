namespace jsonnetTests

open System

type Product() = 
    member val Name = "" with get,set
    member val ExpiryDate = DateTime.Now with get,set
    member val Price = 0M with get,set
    member val Sizes:string[] = [||] with get,set


