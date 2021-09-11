namespace RayTracer.Computation

open RayTracer.Point
open RayTracer.Vector
open RayTracer.Ray
open RayTracer.Intersection
open RayTracer.Object
open RayTracer.Constnats
open RayTracer.Helpers
open RayTracer.ObjectDomain
open RayTracer.RayDomain
open System

module Computation =

    let nValues intersection ray (xs:Intersection list) =

        let removeOrAppendToContainer (intersection: Intersection) (containers: RayTracer.ObjectDomain.ObjectDomain.Object list) =
            let objectIsInContainers =
                containers
                |> List.map (fun x -> x.id)
                |> List.exists ((=) (intersection.object.id))

            match objectIsInContainers with
            | true -> containers |> List.where(fun o -> o <> intersection.object)
            | false -> containers @ [intersection.object]

        let rec compute containers n1 n2 intersections =
            match intersections with
            | [] -> (n1, n2)
            | i::intersectionTail ->
                let hit = i .=. intersection

                let n1' =
                    match hit, containers with
                    | true, [] -> 1.
                    | true, _ -> (last containers).material.refractiveIndex
                    | false, _ -> n1

                let containers' = removeOrAppendToContainer i containers

                match hit, containers' with
                | true, [] -> (n1', 1.)
                | true, _ ->
                    let n2' = (last containers').material.refractiveIndex
                    (n1', n2')
                | false, _ -> compute containers' n1' n2 intersectionTail

        compute [] 1. 1. xs

    let prepare (r:Ray) (xs:Intersection list) (i:Intersection) =

        let t = i.t
        let object = i.object
        let point = Ray.position i.t r
        let eyev = r.direction * -1.
        let normalv = i.object |> Object.normal point
        let inside = (Vector.dot normalv eyev) < 0.
        let trueNormalv = if inside then normalv * -1. else normalv
        let n1,n2 = nValues i r xs

        { t = t;
          object = object;
          point = point;
          overPoint = point + trueNormalv * epsilon;
          eyev = eyev;
          inside = inside;
          normalv = trueNormalv;
          reflectv = Vector.reflect trueNormalv r.direction;
          n1 = n1;
          n2 = n2;
          underPoint = point - trueNormalv * epsilon }

    let shlick comps =

        let reflactance comps cos =
            let r0 = Math.Pow(((comps.n1 - comps.n2) / (comps.n1 + comps.n2)), 2.)
            (r0 + (1. - r0) * Math.Pow((1. - cos), 5.))

        let cos = Vector.dot comps.eyev comps.normalv
        match comps.n1 > comps.n2 with
        | true ->
            let n = (comps.n1 / comps.n2)
            let sin2t = (n*n) * (1. - (cos * cos))

            match sin2t > 1. with
            | true -> 1.
            | false ->
                let cosT = Math.Sqrt(1. - sin2t)
                reflactance comps cosT
        | false -> reflactance comps cos