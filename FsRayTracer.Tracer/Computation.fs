namespace RayTracer.Computation

open RayTracer.Point
open RayTracer.Vector
open RayTracer.Ray
open RayTracer.Intersection
open RayTracer.Object
open RayTracer.Constnats

[<AutoOpen>]
module Domain =

    type Computation =
        { t: float;
          object: Object;
          point:Point;
          overPoint: Point;
          eyev: Vector;
          inside: bool;
          normalv: Vector}

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
          normalv = trueNormalv }

