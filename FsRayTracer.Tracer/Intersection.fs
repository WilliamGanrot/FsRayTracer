namespace RayTracer.Intersection

open RayTracer.Shape

[<AutoOpen>]
module Domain =
    type Intersection = {t:float; object:Sphere}

module Intersection =
    let create (s:Sphere) (t:float) =
        {object = s; t = t}

    let intersections (l: Intersection list) : Intersection list =
        l

    let hit (l: Intersection list) : Option<Intersection>=
        let hitsInfrontOfOrigin =
            l
            |> List.filter (fun i -> i.t > 0.)
            |> List.sortBy (fun i -> i.t)

        match hitsInfrontOfOrigin with
        | [] -> None
        | h::_ -> Some h