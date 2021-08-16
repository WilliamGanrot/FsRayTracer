namespace RayTracer.Intersection

open RayTracer.Object
open RayTracer.Vector
open RayTracer.Point

[<AutoOpen>]
module Domain =
    type Intersection = { t: float; object: Object }


module Intersection =
    let create (o:Object) (t:float) =
        { object = o; t = t }

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

