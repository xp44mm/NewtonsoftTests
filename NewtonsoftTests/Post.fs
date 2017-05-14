namespace NewtonsoftTests

open System

type Post() = 
    member val Title = "" with get,set
    member val Description = "" with get,set
    member val Link = "" with get,set
    member val Categories:string[] = [||] with get,set


