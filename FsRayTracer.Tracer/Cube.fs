namespace RayTracer.Cube
open RayTracer.Constnats
open RayTracer.RayDomain
open RayTracer.ObjectDomain
open RayTracer.Helpers
open RayTracer.Matrix
open RayTracer.Vector
open RayTracer.Material
open RayTracer.BoundingBox

open System
open RayTracer.Intersection


module Cube =


    let localIntersect object ray =

        let (xtmin, xtmax) = BoundingBox.checkAxis ray.origin.X ray.direction.X -1. 1.
        let (ytmin, ytmax) = BoundingBox.checkAxis ray.origin.Y ray.direction.Y -1. 1.
        let (ztmin, ztmax) = BoundingBox.checkAxis ray.origin.Z ray.direction.Z -1. 1.

        let tmin = [xtmin; ytmin; ztmin] |> List.max
        let tmax = [xtmax; ytmax; ztmax] |> List.min

        match tmin > tmax with
        | true -> []
        | false ->
            let i1 = Intersection.create object tmin
            let i2 = Intersection.create object tmax
        
            [i1; i2]

    let localNormalAt shape objectPoint i =
        let maxc =
            [objectPoint.X; objectPoint.Y; objectPoint.Z]
            |> List.map (fun v -> Math.Abs(v:float))
            |> List.max

        match maxc with
        | _ when (FloatHelper.equal maxc (Math.Abs(objectPoint.X:float))) -> Vector.create objectPoint.X 0. 0.
        | _ when (FloatHelper.equal maxc (Math.Abs(objectPoint.Y:float))) -> Vector.create 0. objectPoint.Y 0.
        | _                                                               -> Vector.create 0. 0. objectPoint.Z

    let create() =
        { transform = Matrix.identityMatrix 4;
          transformInverse= Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = Cube;
          id = newRandom();
          localIntersect = localIntersect;
          localNormalAt = localNormalAt;
          bounds = BoundingBox.boundsOf Cube}


