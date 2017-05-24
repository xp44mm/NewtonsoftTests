namespace NewtonsoftTests

//Deserializing Using LINQ Objects
type ShortieException() =
    member val Code = 0 with get,set
    member val ErrorMessage = "" with get,set

type Shortie() =
    member val Original = "" with get,set
    member val Shortened = "" with get,set
    member val Short = "" with get,set
    member val Error = ShortieException() with get,set

