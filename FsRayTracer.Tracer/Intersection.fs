namespace RayTracer.Intersection

open RayTracer.Shape

[<AutoOpen>]
module Domain =
    type Intersection = {t:float; object:Sphere}

module Intersection =
    let create (s:Sphere) (t:float) =
        {object = s; t = t}

    let intersections i1 i2 : Intersection list =
        [i1;i2]

