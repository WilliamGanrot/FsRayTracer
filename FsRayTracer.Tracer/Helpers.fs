namespace RayTracer.Helpers
open RayTracer.Constnats

[<AutoOpen>]
module Helpers =

    let lines (s:string) = s.Split "\n"

    let (|IsEven|_|) n =
      if n % 2 = 0 then Some () else None

module FloatHelper =
    let equal a b = abs(a - b) < epsilon

