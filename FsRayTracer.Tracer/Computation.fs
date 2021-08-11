namespace RayTracer.Computation

open RayTracer.Point
open RayTracer.Vector
open RayTracer.Ray
open RayTracer.Intersection
open RayTracer.Shape

[<AutoOpen>]
module Domain =

    type Computation =
        { t: float;
          object: Sphere;
          point:Point;
          eyev: Vector;
          inside: bool;
          normalv: Vector}

module Computation =

    let prepare (r:Ray) (i:Intersection) =

        let t = i.t
        let object = i.object
        let point = Ray.position i.t r
        let eyev = r.direction * -1.
        let normalv = i.object |> Shape.normal point
        let inside = (Vector.dot normalv eyev) < 0.

        { t = t;
          object = object;
          point = point
          eyev = eyev
          inside = inside;
          normalv = if inside then normalv * -1. else normalv }

