namespace RayTracer.Helpers
open RayTracer.Constnats

[<AutoOpen>]
module Helpers =

    let r = System.Random() 
    let newRandom() = r.Next()

    let lines (s:string) = s.Split "\n"

    let (|IsEven|_|) n =
      if n % 2 = 0 then Some () else None


    let rec last list = 
        match list with
        | h::[] -> h
        | h::t -> last t
        | _ -> failwith "not found"

[<AutoOpen>]
module FloatHelper =
    let equal a b = abs(a - b) < epsilon

    let (/.) (x:float) (s:float) =

        match (x/s) with
        | f when System.Double.IsNaN(f) -> 0.
        | f -> f




[<AutoOpen>]
module Operations =
    let rec insert v i l =
        match i, l with
        | 0, xs -> v::xs
        | i, x::xs -> x::insert v (i - 1) xs
        | i, [] -> failwith "index out of range"

