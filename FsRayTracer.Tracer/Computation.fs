namespace RayTracer.Computation

open RayTracer.Point
open RayTracer.Vector
open RayTracer.Ray
open RayTracer.Intersection
open RayTracer.Object
open RayTracer.Constnats
open RayTracer.Helpers

[<AutoOpen>]
module Domain =

    type Computation =
        { t: float;
          object: Object;
          point:Point;
          overPoint: Point;
          eyev: Vector;
          inside: bool;
          normalv: Vector;
          reflectv: Vector;
          n1: float;
          n2: float;}

module Computation =

    let prepare (r:Ray) (i:Intersection) =

        let t = i.t
        let object = i.object
        let point = Ray.position i.t r
        let eyev = r.direction * -1.
        let normalv = i.object |> Object.normal point
        let inside = (Vector.dot normalv eyev) < 0.
        let trueNormalv = if inside then normalv * -1. else normalv

        { t = t;
          object = object;
          point = point;
          overPoint = point + trueNormalv * epsilon;
          eyev = eyev;
          inside = inside;
          normalv = trueNormalv;
          reflectv = Vector.reflect trueNormalv r.direction;
          n1 = 0.;
          n2 = 0.}


    let nValues intersection ray (xs:Intersection list) =

        //might be shakey comparison
        let removeOrAppendToContainer (intersection: Intersection) (containers: Object list) =
            let objectIsInContainers =
                containers
                |> List.map (fun x -> x.GetHashCode())
                |> List.exists ((=) (intersection.object.GetHashCode()))

            match objectIsInContainers with
            | true -> containers |> List.where(fun o -> o <> intersection.object)
            | false -> containers @ [intersection.object]

        let rec compute containers n1 n2 intersections =
            match intersections with
            | [] -> (n1, n2)
            | i::intersectionTail ->
                let hit = i = intersection

                let n1' =
                    match hit, containers with
                    | true, [] -> 1.
                    | true, _ -> (last containers).material.reflectiveIndex
                    | false, _ -> n1

                let containers' = removeOrAppendToContainer i containers

                match hit, containers' with
                | true, [] -> (n1', 1.)
                | true, _ ->
                    let n2' = (last containers').material.reflectiveIndex
                    (n1', n2')
                | false, _ -> compute containers' n1' n2 intersectionTail

        compute [] 1. 1. xs