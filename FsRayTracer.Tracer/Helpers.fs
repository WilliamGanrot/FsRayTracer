namespace RayTracer.Helpers
open RayTracer.Constnats

[<AutoOpen>]
module Helpers =

    let lines (s:string) = s.Split "\n"

    let (|IsEven|_|) n =
      if n % 2 = 0 then Some () else None

module FloatHelper =
    let equal a b = abs(a - b) < epsilon


[<AutoOpen>]
module Operations =
    let rec insert v i l =
        match i, l with
        | 0, xs -> v::xs
        | i, x::xs -> x::insert v (i - 1) xs
        | i, [] -> failwith "index out of range"

