namespace RayTracer.Intersection

open RayTracer.Vector
open RayTracer.Point
open RayTracer.ObjectDomain


module Intersection =

    let create (o:Object) (t:float) =
        { object = o; t = t; uv = None }

    let tuplesToIntersections l : Intersection list =
        let rec c l accl =
            match l with
            | []    -> accl
            | (t,o)::rest  -> c rest (accl @ [create o t])
        c l []

    let hit (l: Intersection list) : Option<Intersection>=
        let hitsInfrontOfOrigin =
            l
            |> List.filter (fun i -> i.t > 0.)
            |> List.sortBy (fun i -> i.t)

        match hitsInfrontOfOrigin with
        | [] -> None
        | h::_ -> Some h

    let intersectWithUV t u v object =
        { object = object; t = t; uv = Some (u,v) }
