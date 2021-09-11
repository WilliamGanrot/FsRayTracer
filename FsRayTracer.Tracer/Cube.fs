namespace RayTracer.Cube
open RayTracer.Constnats
open RayTracer.RayDomain
open RayTracer.ObjectDomain
open RayTracer.Helpers
open RayTracer.Matrix
open RayTracer.Material

open System
open RayTracer.Intersection


module Cube =
    let checkAxis origin direction =
        let tMinNumerator = (-1.) - origin
        let tMaxNumerator = 1. - origin

        let tMin, tMax =
            match Math.Abs(direction:float) >= epsilon with
            | true ->
                let tMin = tMinNumerator / direction
                let tMax = tMaxNumerator / direction
                (tMin, tMax)
            | false ->
                let tMin = tMinNumerator * infinity
                let tMax = tMaxNumerator * infinity
                (tMin, tMax)

        if tMin > tMax then (tMax, tMin) else (tMin, tMax)

    let localIntersect object ray =

        let (xtmin, xtmax) = checkAxis ray.origin.X ray.direction.X
        let (ytmin, ytmax) = checkAxis ray.origin.Y ray.direction.Y
        let (ztmin, ztmax) = checkAxis ray.origin.Z ray.direction.Z

        let tmin = [xtmin; ytmin; ztmin] |> List.max
        let tmax = [xtmax; ytmax; ztmax] |> List.min

        match tmin > tmax with
        | true -> []
        | false ->
            let i1 = Intersection.create object tmin
            let i2 = Intersection.create object tmax
        
            [i1; i2]

    let cube() =
        { transform = Matrix.identityMatrix 4;
          transformInverse= Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = Cube;
          id = newRandom();
          localIntersect = localIntersect }


