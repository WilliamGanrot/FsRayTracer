namespace RayTracer.Plane
open System
open RayTracer.ObjectDomain
open RayTracer.RayDomain
open RayTracer.Helpers
open RayTracer.Constnats
open RayTracer.Intersection
open RayTracer.Matrix
open RayTracer.Material
open RayTracer.Vector



module Plane =

    let localIntersect object ray =
        match Math.Abs(ray.direction.Y) < epsilon with
        | true  -> []
        | false ->
            let i =
                let t = -(ray.origin.Y / ray.direction.Y)
                Intersection.create object t
            [i]

    let localNormalAt shape objectPoint =
        Vector.create 0. 1. 0.

    let create() = 
        { transform = Matrix.identityMatrix 4;
          transformInverse = Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = Plane;
          id = newRandom();
          localIntersect = localIntersect;
          localNormalAt = localNormalAt;
          parent = None; }



